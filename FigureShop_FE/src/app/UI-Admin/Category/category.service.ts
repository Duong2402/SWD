import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { URL_Base } from '../../app.config';
import { CategoryCreate, CategoryList } from './Model/Category.Model';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  private api = `${URL_Base}/Category`;
  constructor(private http: HttpClient) { }

  getCategories(): Observable<CategoryList[]> {
    return this.http.get<CategoryList[]>(this.api);
  }

  getCategoryById(id: string): Observable<CategoryList> {
    return this.http.get<CategoryList>(`${this.api}/${id}`);
  }

  createCategory(category: { name: string; description: string }): Observable<string> {
    return this.http.post<string>(this.api, category);
  }

  updateCategory(id: string, category: CategoryCreate): Observable<void> {
    return this.http.put<void>(`${this.api}/${id}`, category);
  }

  deleteCategory(id: string): Observable<void> {
    return this.http.delete<void>(`${this.api}/${id}`);
  }
}
