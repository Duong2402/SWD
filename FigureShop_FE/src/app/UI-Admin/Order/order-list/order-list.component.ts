import { Component, OnDestroy, OnInit } from '@angular/core';
import { PagedResult } from '../../../core/Model/PageResult';
import { Order } from '../Model/Order';
import { Subject } from 'rxjs/internal/Subject';
import { OrderService } from '../order.service';
import { Router, RouterLink } from '@angular/router';
import { takeUntil } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SideBarComponent } from "../../core/side-bar/side-bar.component";

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterLink, SideBarComponent],
  templateUrl: './order-list.component.html',
  styleUrl: './order-list.component.css'
})
export class OrderListComponent implements OnInit, OnDestroy {
  orders?: PagedResult<Order>;
  selectedOrderId?: string;
  selectedStatus: number = 0;
  private destroy$ = new Subject<void>();
  page?: number = 1;
  pageSize: number = 2;
  constructor(private service: OrderService, private router: Router) { }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  ngOnInit(): void {
    this.getAllOrder();
  }

  getAllOrder(): void {
    this.service.getAllOrder('', this.page, this.pageSize).pipe(takeUntil(this.destroy$)).subscribe({
      next: (res) => {
        console.log(res);
        console.log("hêlo");
        this.orders = res;
        console.log(this.orders);
      },
      error: (err) => {
        console.log(err);
      }
    });
  }

  OrderStatus(orderId: string) {
    this.selectedOrderId = orderId;
  }

  updateOrderStatus() {
    console.log(this.selectedStatus);
    console.log(this.selectedOrderId);
    const statusParse = Number(this.selectedStatus);
    this.service.updateOrderStatus(this.selectedOrderId!, statusParse).pipe(takeUntil(this.destroy$)).subscribe({
      next: (res) => {
        console.log(res);
        console.log("Update status order success");
        this.getAllOrder();
      },
      error: (err) => {
        console.log(err);
      }
    });
  }

  goToPreviousPage() {
    if (this.orders?.pageNumber && this.orders.pageNumber > 1) {
      this.page = this.orders.pageNumber - 1;
      this.getAllOrder();
    }
  }
  goToPage(pageIn?: number) {
    this.page = pageIn;
    this.getAllOrder();
  }
  goToNext() {
    if (this.orders?.pageNumber && this.orders.pageNumber < this.orders.totalPages) {
      this.page = this.orders.pageNumber + 1;
      this.getAllOrder();
    }
  }

  numberPage(): number[] {
    if (this.orders == null || this.orders.totalPages <= 0) {
      return [];
    }

    const totalPage = this.orders.totalPages;
    const current = this.orders.pageNumber;

    const pageStart = Math.max(1, current - 1);
    const pageEnd = Math.min(totalPage, current + 1);

    const result: number[] = [];

    for (let page = pageStart; page <= pageEnd; page++) {
      result.push(page);
    }
    return result;
  }



}
