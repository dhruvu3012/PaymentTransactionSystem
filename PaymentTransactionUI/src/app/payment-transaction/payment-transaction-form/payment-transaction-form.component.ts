import { Component, Directive, ElementRef, HostListener } from '@angular/core';
import { PaymentTransactionService } from '../../shared/payment-transaction.service';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { APIResponse } from '../../shared/payment-transaction.model';
import { hostname } from 'node:os';

@Component({
  selector: 'app-payment-transaction-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './payment-transaction-form.html',
  styles: ``,
})

export class PaymentTransactionFormComponent {


  constructor(public service: PaymentTransactionService, private toastr: ToastrService) {

  }

  onSubmit(form: NgForm) {
    this.service.formSubmitted = true;
    if (form.valid) {

      this.service.createTransaction()
        .subscribe({
          next: res => {
            var apiResponse = res as unknown as APIResponse;
            this.service.refereshList();
            this.service.resetForm(form);
            this.toastr.success(apiResponse.message, 'Success');
            this.service.formSubmitted = false;
          },
          error: err => {
            this.toastr.error(err.error.message, 'Error');
          }
        })
    }
  }
}
