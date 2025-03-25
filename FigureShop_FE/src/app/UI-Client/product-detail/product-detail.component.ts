import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationComponent } from "../../core/navigation/navigation.component";
import { CartComponent } from "../cart/cart.component";
import { FooterComponent } from "../../core/footer/footer.component";
import { FigureService } from '../../UI-Admin/Product/services/figure.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CartItem, ProductDetail } from '../../UI-Admin/Product/Model/Figure';
import { Subject, takeUntil } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CheckoutService } from '../services/checkout.service';
import { CartService } from '../../UI-Admin/Cart/cart.service';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [NavigationComponent, FooterComponent, CommonModule, FormsModule],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.css'
})
export class ProductDetailComponent implements OnInit, OnDestroy {

  idParam?: string;
  product?: ProductDetail;
  cartItem?: CartItem;

  private destroy$ = new Subject<void>();

  constructor(private service: FigureService, private router: ActivatedRoute,
    private route: Router, private checkoutService: CheckoutService,
    private cartService: CartService) {

  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  ngOnInit(): void {
    this.router.params.subscribe(params => {
      this.idParam = params['id'];
      this.getProductDetail();

    });
  }

  formatCurrency(price: number | undefined): string {
    if (price === undefined) {
      return ''; // Or handle the undefined case as needed
    }
    const formatter = new Intl.NumberFormat('vi-VN', {
      style: 'currency',
      currency: 'VND',
    });
    return formatter.format(price);
  }

  convertToUrlFormat(text: string | undefined): string {
    if (text === undefined) {
      return '';
    }
    return text.trim().toLowerCase().replace(/\s+/g, '-');
  }


  getProductDetail() {
    console.log("ID: ", this.idParam);
    if (this.idParam) {
      this.service.getDetail(this.idParam).pipe(takeUntil(this.destroy$)).subscribe({
        next: (product) => {
          this.product = product;

          this.cartItem = {
            productId: this.product.id,
            quantity: 1,
          };
        },
        error: (error) => {
          console.log("Error: ", error);
        }
      });
    }
  }

  onAddToCart(productId: string, quantity: number): void {
    const userId = "8B86282E-B3DE-4692-2DD2-08DD679719EA";
    this.cartService.addToCart(userId, productId, quantity).pipe(takeUntil(this.destroy$)).subscribe({
      next: (response) => {
        console.log('Product add successfully', response);
      },
      error: (err) => {
        console.error('Error when adding product to cart.', err);
      }
    });
  }

  incrementQuantity() {
    if (this.cartItem && this.product) {
      if (this.cartItem.quantity < this.product.stockQuantity) {
        this.cartItem.quantity++;
        console.log("Cart item: ", this.cartItem);
      }
    } else {
      console.log("Cart item chưa được khởi tạo!");
    }
  }

  decrementQuantity() {
    if (this.cartItem && this.product) {
      if (this.cartItem.quantity > 1) {
        this.cartItem.quantity--;
        console.log("Cart item: ", this.cartItem);
      }
    } else {
      console.log("Cart item chưa được khởi tạo!");
    }
  }

  gotoCheckout() {
    if (!this.product) {
      console.error("Không có sản phẩm để checkout!");
      return;
    }

    const quantity = 1;
    const price = this.product.price ?? 0; // Đảm bảo price có giá trị
    const total = price * quantity;

    this.checkoutService.setCheckoutData({
      id: this.product.id,
      name: this.product.name,
      price: price,
      quantity: quantity,
      imageUrl: this.product.imageUrl?.length ? this.product.imageUrl[0].url : null,
      total: total
    });

    this.route.navigate(['/checkout']);
  }




}
