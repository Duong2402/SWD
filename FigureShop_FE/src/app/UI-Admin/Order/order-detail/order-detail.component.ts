import { Component, OnDestroy, OnInit } from '@angular/core';
import { Order } from '../Model/Order';
import { OrderService } from '../order.service';
import { Subject } from 'rxjs/internal/Subject';
import { ActivatedRoute, Route, RouterLink } from '@angular/router';
import { takeUntil } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './order-detail.component.html',
  styleUrl: './order-detail.component.css'
})
export class OrderDetailComponent implements OnInit, OnDestroy {
  orderDetail?: Order;
  idParams?: string;
  private destroy$ = new Subject<void>();
  constructor(private orderService: OrderService, private router: ActivatedRoute) { }


  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngOnInit(): void {
    this.router.params.subscribe(params => {
      this.idParams = params['id'];
      this.getOrderDetail();
    })
  }

  getOrderDetail(): void {
    console.log("ID: ", this.idParams);
    if (this.idParams) {
      this.orderService.getOrderDetail(this.idParams).pipe(takeUntil(this.destroy$)).subscribe({
        next: (order) => {
          this.orderDetail = order;
          console.log("Order Detail Response: ", order);
          console.log("Order Detail: ", this.orderDetail.items);
        },
        error: (error) => {
          console.log("Error: ", error);
        }
      });
    }
  }



}
