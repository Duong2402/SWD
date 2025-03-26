import { Injectable } from '@angular/core';
import { URL_Base } from '../../../app.config';
import { HttpClient } from '@angular/common/http';
import { PagedResult } from '../../../core/Model/PageResult';
import { BaseProductDto, ProductCreate, ProductDetail } from '../Model/Figure';
import { Observable } from 'rxjs/internal/Observable';
import { CategoryList } from '../../Category/Model/Category.Model';

@Injectable({
  providedIn: 'root'
})
export class FigureService {

  private api = `${URL_Base}/Figure`;
  constructor(private http: HttpClient) { }

  filterProduct(
    name?: string, type?: string, vendor?: string, category?: string, min?: number, max?: number,
    page: number = 1, pageSize: number = 10
  ): Observable<PagedResult<BaseProductDto>> {
    const params = new URLSearchParams();
    if (name) params.append('name', name);
    if (type) params.append('type', type);
    if (vendor) params.append('vendor', vendor);
    if (category) params.append('category', category);
    if (min) params.append('min', min.toString());
    if (max) params.append('max', max.toString());
    params.append('page', page.toString());
    params.append('pageSize', pageSize.toString());
    return this.http.get<PagedResult<BaseProductDto>>(`${this.api}/FilterProduct/filter?${params.toString()}`);
  }

  getDetail(id: string): Observable<ProductDetail> {
    return this.http.get<ProductDetail>(`${this.api}/GetDetailById/${id}`);
  }

  createProduct(product: FormData): Observable<string> {
    return this.http.post<string>(`${this.api}/Create`, product);
  }

  getCategories(): Observable<CategoryList[]> {
    return this.http.get<CategoryList[]>(`${this.api}/CategoryList`);
  }

  updateProduct(productId: string, product: FormData): Observable<string> {
    return this.http.put<string>(`${this.api}/Update/${productId}`, product);
  }

  deleteProduct(productId: string): Observable<string> {
    return this.http.delete<string>(`${this.api}/Delete/${productId}`);
  }

}
