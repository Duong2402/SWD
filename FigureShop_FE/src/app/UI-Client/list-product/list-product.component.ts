import { Component } from '@angular/core';
import { NavigationComponent } from "../../core/navigation/navigation.component";
import { FooterComponent } from "../../core/footer/footer.component";

@Component({
  selector: 'app-list-product',
  standalone: true,
  imports: [NavigationComponent, FooterComponent],
  templateUrl: './list-product.component.html',
  styleUrl: './list-product.component.css'
})
export class ListProductComponent {

}
