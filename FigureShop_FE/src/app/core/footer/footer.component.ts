import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.css'
})
export class FooterComponent {
  scrollTop() {
    const container = document.querySelector('.container-fluid'); // Lấy phần tử cha
    if (container) {
      console.log("get Top");
      container.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }
}
