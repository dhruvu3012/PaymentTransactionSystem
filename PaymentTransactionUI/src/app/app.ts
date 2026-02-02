import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PaymentTransactionComponent } from './payment-transaction/payment-transaction.component';

@Component({
  selector: 'app-root',
  imports: [PaymentTransactionComponent],
  templateUrl: './app.html',
  styles: [],
})
export class App {
  protected readonly title = signal('PaymentTransactionUI');
}
