import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { SideBarComponent } from "../../core/side-bar/side-bar.component";
import { NavigationComponent } from "../../../core/navigation/navigation.component";
import { BaseProductDto } from '../Model/Figure';
import { FigureService } from '../services/figure.service';
import { Subject, takeUntil } from 'rxjs';
import { PagedResult } from '../../../core/Model/PageResult';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, SideBarComponent, RouterLink],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit, OnDestroy {
  products?: PagedResult<BaseProductDto>;
  isCollapsed = false;
  page?: number = 1;
  pageSize: number = 6;
  private destroy$ = new Subject<void>();

  constructor(private figure: FigureService) {
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngOnInit() {
    this.getProducts();
  }
  toggleSidebar() {
    this.isCollapsed = !this.isCollapsed;
  }

  getProducts() {
    this.figure.filterProduct('', '', '', '', 0, Number.MAX_VALUE, this.page, this.pageSize).pipe(takeUntil(this.destroy$)).subscribe({
      next: (data) => {
        this.products = data;
        console.log(this.products)
      },
      error: (error) => {
        console.log(error);
      }
    })
  }
  goToPreviousPage() {
    if (this.products?.pageNumber && this.products.pageNumber > 1) {
      this.page = this.products.pageNumber - 1;
      this.getProducts();
    }
  }
  goToPage(pageIn?: number) {
    this.page = pageIn;
    this.getProducts();
  }
  goToNext() {
    if (this.products?.pageNumber && this.products.pageNumber < this.products.totalPages) {
      this.page = this.products.pageNumber + 1;
      this.getProducts();
    }
  }

  numberPage(): number[] {
    if (this.products == null || this.products.totalPages <= 0) {
      return [];
    }

    const totalPage = this.products.totalPages;
    const current = this.products.pageNumber;

    const pageStart = Math.max(1, current - 1);
    const pageEnd = Math.min(totalPage, current + 1);

    const result: number[] = [];

    for (let page = pageStart; page <= pageEnd; page++) {
      result.push(page);
    }
    return result;
  }
}
