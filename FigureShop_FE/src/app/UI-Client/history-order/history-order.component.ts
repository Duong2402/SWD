import { Component, OnDestroy, OnInit } from '@angular/core';
import { Order } from '../../UI-Admin/Order/Model/Order';
import { Subject } from 'rxjs/internal/Subject';
import { OrderService } from '../../UI-Admin/Order/order.service';
import { AuthService } from '../../core/services/auth.service';
import { pipe, takeUntil } from 'rxjs';
import { PagedResult } from '../../core/Model/PageResult';
import { CommonModule } from '@angular/common';
import { NavigationComponent } from "../../core/navigation/navigation.component";

@Component({
  selector: 'app-history-order',
  standalone: true,
  imports: [CommonModule, NavigationComponent],
  templateUrl: './history-order.component.html',
  styleUrl: './history-order.component.css'
})
export class HistoryOrderComponent implements OnInit, OnDestroy {

  orders?: PagedResult<Order>;
  private destroy$ = new Subject<void>();
  page: number = 1;
  pageSize: number = 5;
  constructor(private orderService: OrderService, private auth: AuthService) {
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  ngOnInit(): void {
    this.loadOrder();
  }

  loadOrder() {
    var user = this.auth.getUserId();
    this.orderService.getAllOrder(user!, this.page, this.pageSize)
      .pipe(takeUntil(this.destroy$)) // Thêm dấu chấm trước pipe
      .subscribe({
        next: (res) => {
          this.orders = res;
        },
        error: (err) => {
          console.error(err);
        }
      });
  }

}
