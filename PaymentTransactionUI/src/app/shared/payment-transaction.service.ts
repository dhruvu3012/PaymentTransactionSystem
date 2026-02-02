import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { Console } from 'console';
import { APIResponse, PaymentTransaction, PaymentTransactionCreate, UpdateStatus } from './payment-transaction.model';
import { NgForm } from '@angular/forms';
import * as CryptoJS from 'crypto-js';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class PaymentTransactionService {
  getUrl = environment.apiBaseUrl + "/Transaction/Get";
  createTransactionUrl = environment.apiBaseUrl + "/Transaction/Create";
  paymentUrl = environment.apiBaseUrl + '/Transaction/Process';
  formData: PaymentTransactionCreate = new PaymentTransactionCreate();
  formSubmitted: boolean = false
  updateStatus: UpdateStatus = new UpdateStatus();
  public transactions = signal<any[]>([]);

  constructor(private http: HttpClient, public toastr: ToastrService) {
  }

  refereshList() {
    this.http.get<PaymentTransaction[]>(this.getUrl)
      .subscribe({
        next: res => {
          var apiResponse = res as unknown as APIResponse;
          this.transactions.set(JSON.parse(apiResponse.result) as PaymentTransaction[])
        },

        error: err => {
          this.toastr.error(err.error.message, 'Error');
        }
      })
  }

  createTransaction() {
    return this.http.post(`${this.createTransactionUrl}`, this.formData)
  }

  resetForm(form: NgForm) {
    form.form.reset()
    this.formData = new PaymentTransactionCreate()
    this.formSubmitted = false;
  }

  simulateWebhook() {
    return this.http.post(this.paymentUrl, this.updateStatus);
  }
}
