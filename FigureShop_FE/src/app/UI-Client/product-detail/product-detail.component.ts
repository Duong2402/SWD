import { Component } from '@angular/core';
import { NavigationComponent } from "../../core/navigation/navigation.component";
import { CartComponent } from "../cart/cart.component";
import { FooterComponent } from "../../core/footer/footer.component";

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [NavigationComponent, FooterComponent],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.css'
})
export class ProductDetailComponent {

}
