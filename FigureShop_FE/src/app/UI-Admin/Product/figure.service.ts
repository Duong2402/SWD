import { Injectable } from '@angular/core';
import { URL_Base } from '../../app.config';
import { HttpClient } from '@angular/common/http';
import { PagedResult } from '../../core/Model/PageResult';
import { BaseProductDto } from './Model/Figure';
import { Observable } from 'rxjs/internal/Observable';

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
}
