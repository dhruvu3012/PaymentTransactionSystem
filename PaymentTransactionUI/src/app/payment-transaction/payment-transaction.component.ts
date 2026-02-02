import { Component, OnInit } from '@angular/core';
import { PaymentTransactionService } from "../shared/payment-transaction.service"
import { CommonModule } from "@angular/common";
import { PaymentTransactionFormComponent } from "./payment-transaction-form/payment-transaction-form.component";
import { ToastrService } from 'ngx-toastr';
import { interval } from 'rxjs';
import { APIResponse } from '../shared/payment-transaction.model';

@Component({
  selector: 'app-payment-transaction',
  standalone:true,
  imports: [CommonModule, PaymentTransactionFormComponent],
  templateUrl: './payment-transaction.html',
  styles: ``,
})
export class PaymentTransactionComponent implements OnInit {

  constructor(public service: PaymentTransactionService, private toastr: ToastrService) {

  }

  ngOnInit(): void {
    interval()
      this.service.refereshList();
  }

  simulateWebhook(providerReference: string = "", status: number = 2) {
    this.service.updateStatus = {
      providerReference: providerReference,
      status: status
    }
    this.service.simulateWebhook().subscribe({
      next: res => {
        var apiResponse = res as unknown as APIResponse;
        this.toastr.success(apiResponse.message, 'Success');
        this.service.refereshList();
      },
      error: err => {
        this.toastr.error(err.error.message || 'Transaction Failed', 'Error');
      },
    })
  }

}
