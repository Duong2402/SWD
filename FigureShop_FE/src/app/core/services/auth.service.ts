import { Injectable } from '@angular/core';
import { URL_Base } from '../../app.config';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { LoginRequest, LoginResponse } from '../Model/LoginRequest';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private api = `${URL_Base}/Auth`;
  constructor(private httpClient: HttpClient, private cookieService: CookieService) { }

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.httpClient.post<LoginResponse>(`${this.api}/Login`, request);
  }

  logout(): Observable<any> {
    const token = this.cookieService.get('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.httpClient.post(`${this.api}/Logout`, {}, { headers }).pipe(
      catchError((error) => {
        if (error.status === 401) {
          console.warn('Token đã hết hạn, xóa cookie...');
          this.cookieService.delete('token'); // Xóa token nếu hết hạn
        }
        return throwError(() => error);
      })
    );
  }


  getUserId(): string | null {
    const token = this.cookieService.get('token');
    if (!token) {
      return null;
    }
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.sub || payload.userId || null;
    } catch (e) {
      console.error(e);
      return null;
    }
  }
}
