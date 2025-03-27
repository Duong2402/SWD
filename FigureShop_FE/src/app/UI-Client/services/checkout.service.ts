import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  private checkoutData = new BehaviorSubject<any>(null);
  checkoutData$ = this.checkoutData.asObservable();

  setCheckoutData(data: any) {
    this.checkoutData.next(data);
  }
  constructor() { }
}
