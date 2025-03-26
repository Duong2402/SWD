import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationComponent } from "../navigation/navigation.component";
import { FooterComponent } from "../footer/footer.component";
import { LoginRequest } from '../Model/LoginRequest';
import { Subject } from 'rxjs/internal/Subject';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [NavigationComponent, FooterComponent, FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnDestroy {
  model: LoginRequest = { login: '', password: '' };
  private destroy$ = new Subject<void>();
  constructor(private authService: AuthService,
    private router: Router, private cookieService: CookieService
  ) { this.model }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  login() {
    console.log(this.model);;

    if (!this.model || !this.model.login || !this.model.password) {
      alert("❌ Vui lòng nhập đầy đủ thông tin đăng nhập!");
      return;
    }

    this.authService.login(this.model).subscribe(
      {
        next: (res) => {
          if (res) {
            this.cookieService.set("token", res.token);
            console.log("res", res);
            alert("🎉 Đăng nhập thành công!");
            this.router.navigate(['/']);
          }
        },
        error: (err) => {
          console.log("err", err);
          alert("❌ Đăng nhập thất bại!");
        }
      }
    );
  }



}
