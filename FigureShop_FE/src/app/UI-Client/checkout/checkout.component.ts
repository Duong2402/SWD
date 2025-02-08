import { Component } from '@angular/core';
import { NavigationComponent } from "../../core/navigation/navigation.component";
import { FooterComponent } from "../../core/footer/footer.component";

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [NavigationComponent, FooterComponent],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})
export class CheckoutComponent {

}
