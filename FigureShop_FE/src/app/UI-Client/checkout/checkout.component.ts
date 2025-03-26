import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationComponent } from "../../core/navigation/navigation.component";
import { FooterComponent } from "../../core/footer/footer.component";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CheckoutService } from '../services/checkout.service';
import { Subject } from 'rxjs/internal/Subject';
import { takeUntil } from 'rxjs';
import { OrderService } from '../../UI-Admin/Order/order.service';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [NavigationComponent, FooterComponent, FormsModule, CommonModule],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})
export class CheckoutComponent implements OnInit, OnDestroy {
  products: any[] = [];
  totalAmount: number = 0;
  private destroy$ = new Subject<void>();
  constructor(private checkout: CheckoutService, private order: OrderService,
    private router: Router, private auth: AuthService
  ) { }


  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  ngOnInit() {
    this.checkout.checkoutData$
      .pipe(takeUntil(this.destroy$))
      .subscribe(data => {
        if (Array.isArray(data)) {
          this.products = data;
        } else {
          this.products = [data]; // Nếu chỉ có 1 sản phẩm thì đưa vào mảng
        }
        this.calculateTotal();
      });
  }

  calculateTotal() {
    this.totalAmount = this.products.reduce((sum, p) => sum + (p.price ?? 0) * (p.quantity ?? 1), 0);
  }

  submitOrder() {
    if (!this.products || this.products.length === 0) {
      return;
    }

    const listOrder = {
      userId: this.auth.getUserId(),
      items: this.products.map(p => ({
        productId: p.id,
        quantity: p.quantity || 1
      })),
    };

    this.order.createOrder(listOrder)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res) => {
          console.log("Đơn hàng đã được tạo:", res);
          alert("Đơn hàng của bạn đã được đặt thành công!");
          this.router.navigate(['/shop']);
        },
        error: (err) => {
          console.error("Lỗi khi tạo đơn hàng:", err);
          alert("Đã có lỗi xảy ra, vui lòng thử lại sau!");
        }
      });
  }

}
