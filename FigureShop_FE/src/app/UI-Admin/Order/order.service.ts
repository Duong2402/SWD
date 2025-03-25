import { Injectable } from '@angular/core';
import { URL_Base } from '../../app.config';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Order } from './Model/Order';
import { PagedResult } from '../../core/Model/PageResult';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private api = `${URL_Base}/Order`;

  constructor(private http: HttpClient) { }

  getAllOrder(userId?: string, page: number = 1, pageSize: number = 10): Observable<PagedResult<Order>> {
    let params = new HttpParams();
    if (userId) params = params.set('userId', userId);
    params = params.set('page', page.toString());
    params = params.set('pageSize', pageSize.toString());
    return this.http.get<PagedResult<Order>>(`${this.api}/filter`, { params });
  }

  updateOrderStatus(orderId: string, status: number): Observable<string> {
    return this.http.put<string>(`${this.api}/update-status?orderId=${orderId}`, status);
  }

  getOrderDetail(orderId: string): Observable<Order> {
    return this.http.get<Order>(`${this.api}/Detail/${orderId}`);
  }

  createOrder(order: any): Observable<string> {
    return this.http.post<string>(`${this.api}/create`, order);
  }

}
