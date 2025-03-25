import { Component} from '@angular/core';
import { NavigationComponent } from "../../core/navigation/navigation.component";
import { CartComponent } from "../cart/cart.component";
import { FooterComponent } from "../../core/footer/footer.component";
import { CartService } from '../../UI-Admin/Cart/cart.service';
import { takeUntil } from 'rxjs';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [NavigationComponent, FooterComponent],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.css'
})
export class ProductDetailComponent{
  private destroy$ = new Subject<void>();
  constructor(private cartService: CartService) { }
  onAddToCart(productId: string, quantity: number): void{
          const userId = "C1E15921-E8F6-4CBC-EACD-08DD67BB3796";
          this.cartService.addToCart(userId, productId, quantity).pipe(takeUntil(this.destroy$)).subscribe({
            next: (response) => {
              console.log('Product add successfully', response);
            },
            error: (err) => {
              console.error('Error when adding product to cart.', err);
            }
          });
    }
}
