import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationComponent } from "../navigation/navigation.component";
import { FooterComponent } from "../footer/footer.component";
import { CategoryList } from '../../UI-Admin/Category/Model/Category.Model';
import { Subject, takeUntil } from 'rxjs';
import { CategoryService } from '../../UI-Admin/Category/category.service';
import { CommonModule } from '@angular/common';
import { BaseProductDto } from '../../UI-Admin/Product/Model/Figure';
import { PagedResult } from '../Model/PageResult';
import { FigureService } from '../../UI-Admin/Product/services/figure.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [NavigationComponent, FooterComponent, CommonModule, RouterLink],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css'
})
export class HomePageComponent implements OnInit, OnDestroy {
  figure?: PagedResult<BaseProductDto>;
  categories: CategoryList[] = [];
  private destroy$ = new Subject<void>();

  constructor(private categoryService: CategoryService,
    private figureService: FigureService
  ) {
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  ngOnInit(): void {
    this.loadCategories();
    this.filter();
  }

  loadCategories() {
    this.categoryService.getCategories().pipe(takeUntil(this.destroy$)).subscribe({
      next: data => {
        this.categories = data;
      },
      error: err => { // Sửa lại `err` thành `error`
        console.error('Error loading categories:', err);
      }
    });
  }

  filter(): void {
    this.figureService.filterProduct('', '', '', '', 0, Number.MAX_VALUE, 1, 10)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: data => {
          if (data?.items?.length > 0) {
            this.figure = {
              ...data,
              items: data.items.sort(() => Math.random() - 0.5).slice(0, 4)
            };
          } else {
            this.figure = data; // Tránh lỗi nếu danh sách rỗng
          }
          console.log(this.figure);
        },
        error: err => {
          console.log(err);
        }
      });
  }


}
