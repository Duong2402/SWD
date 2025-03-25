import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { SideBarComponent } from "../../core/side-bar/side-bar.component";
import { NavigationComponent } from "../../../core/navigation/navigation.component";

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, SideBarComponent, NavigationComponent],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent {
  products = [
    { name: "iPhone 14", category: "Electronics", price: 999, publishedOn: new Date("2024-01-15") },
    { name: "MacBook Pro", category: "Laptops", price: 1999, publishedOn: new Date("2024-02-01") },
    { name: "Samsung Galaxy S23", category: "Electronics", price: 899, publishedOn: new Date("2024-01-10") },
    { name: "Nike Air Max", category: "Fashion", price: 150, publishedOn: new Date("2024-03-05") },
    { name: "Sony Headphones", category: "Audio", price: 250, publishedOn: new Date("2024-02-25") }
  ];
}
