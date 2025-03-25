import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationComponent } from "../../core/navigation/navigation.component";
import { FooterComponent } from "../../core/footer/footer.component";
import { CommonModule } from '@angular/common';
import { CartItem } from '../../UI-Admin/Cart/Model/CartItem ';
import { CartService } from '../../UI-Admin/Cart/cart.service';
import { Subject } from 'rxjs/internal/Subject';
import { takeUntil } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router'; 

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [NavigationComponent, FooterComponent,CommonModule,FormsModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit, OnDestroy{
  cart?: CartItem[] = [];
  cartTotal: number = 0; 
      private destroy$ = new Subject<void>();
  
  constructor(private cartService: CartService, private router: Router) {
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
}
  ngOnInit(): void {
    this.getAllCart();
  }

  getAllCart(){
    this.cartService.getCart().pipe(takeUntil(this.destroy$)).subscribe({
      next: data =>{
        console.log(data);
        this.cart = data;    
        this.calculateCartTotal()    
      },
      error: err =>{
        console.log(err);
      }
    })
  }

  calculateCartTotal(): void {
    if (this.cart) {
      this.cartTotal = this.cart.reduce((acc, item) => acc + item.total, 0);
    } else {
      this.cartTotal = 0;
    }
  }

removeFromCart(cartId: string, productId: string): void {

  this.cartService.removeFromCart(cartId, productId).pipe(takeUntil(this.destroy$)).subscribe({
    next: (response) => {
      console.log('Product removed successfully', response);
      this.cart = this.cart?.filter(cartItem => cartItem.productId !== productId);
      this.calculateCartTotal(); 
    },
    error: (err) => {
      console.error('Error removing product from cart', err);
    }
  });
}

updateQuantity(productId: string, quantity: number): void {
  const userId = "C1E15921-E8F6-4CBC-EACD-08DD67BB3796";
  const item = this.cart?.find(item => item.productId === productId);
  if (item) {
    item.quantity = quantity; 
  }
  this.cartService.updateQuantity(userId, productId, quantity).pipe(takeUntil(this.destroy$)).subscribe({
    next: (response) => {
      console.log('The shopping cart has been updated.', response);
    },
    error: (error) => {
      console.error('Error updating the shopping cart.', error);
    }
  });
}
}
