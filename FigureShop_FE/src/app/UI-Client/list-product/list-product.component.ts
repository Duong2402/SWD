import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationComponent } from "../../core/navigation/navigation.component";
import { FooterComponent } from "../../core/footer/footer.component";
import { PagedResult } from '../../core/Model/PageResult';
import { BaseProductDto } from '../../UI-Admin/Product/Model/Figure';
import { Subject } from 'rxjs/internal/Subject';
import { FigureService } from '../../UI-Admin/Product/services/figure.service';
import { Router, RouterLink } from '@angular/router';
import { takeUntil } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CartService } from '../../UI-Admin/Cart/cart.service';
@Component({
    selector: 'app-checkout',
    standalone: true,
    imports: [NavigationComponent, FooterComponent, FormsModule, CommonModule, RouterLink],
    templateUrl: './list-product.component.html',
    styleUrl: './list-product.component.css'
})
export class ListProductComponent implements OnInit, OnDestroy {

    figure?: PagedResult<BaseProductDto>;
    name?: string;
    type?: string;
    vendor?: string;
    category?: string;
    min?: number;
    max?: number;
    page?: number = 1;
    pageSize: number = 6;
    private destroy$ = new Subject<void>();

    priceRanges = [
        { id: 'price-1', label: '$0 - $100.000', min: 0, max: 100000 },
        { id: 'price-2', label: '$100.000 - $300.000', min: 100000, max: 300000 },
        { id: 'price-3', label: '$300.000 - $500.000', min: 300000, max: 500000 },
        { id: 'price-4', label: '$500.000 - $800.000', min: 500000, max: 800000 },
        { id: 'price-5', label: '$800.000 - $1.500.000', min: 800000, max: 1500000 },
    ];
    selectedPrice: { id: string; min: number; max: number } | null = null;



    constructor(private service: FigureService, private cartService: CartService, private router: Router) { }
    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
    ngOnInit(): void {
        this.filter();
    }

    formatCurrency(price: number | undefined): string {
        if (price === undefined) {
            return ''; // Or handle the undefined case as needed
        }
        const formatter = new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND',
        });
        return formatter.format(price);
    }

    filter(): void {
        this.service.filterProduct(this.name, this.type, this.vendor, this.category, this.min
            , this.max, this.page, this.pageSize).pipe(takeUntil(this.destroy$)).subscribe({
                next: response => {
                    this.figure = response;
                },
                error: error => {
                    console.error('API error:', error);
                }
            });
    }
    filterByPrice(price: { id: string; min: number; max: number }): void {
        this.page = 1;
        if (this.selectedPrice && this.selectedPrice.id === price.id) {
            this.selectedPrice = null;
            this.min = 0;
            this.max = Number.MAX_VALUE;
            this.updateUrlParam();
            this.filter();
        } else {
            this.selectedPrice = price;
            this.min = price.min;
            this.max = price.max;
            this.updateUrlParam();
            this.filter();
        }
    }

    isCheckedPrice(id: string): boolean {
        return this.selectedPrice?.id === id;
    }

    updateUrlParam(): void {
        this.router.navigate([], {
            queryParams: this.selectedPrice !== null ?
                { min: this.selectedPrice.min, max: this.selectedPrice.max } : { min: null, max: null },

            queryParamsHandling: 'merge',
        });
    }

    //pagination

    goToPreviousPage() {
        if (this.figure?.pageNumber && this.figure.pageNumber > 1) {
            this.page = this.figure.pageNumber - 1;
            this.filter();
        }
    }
    goToPage(pageIn?: number) {
        this.page = pageIn;
        this.filter();
    }
    goToNext() {
        if (this.figure?.pageNumber && this.figure.pageNumber < this.figure.totalPages) {
            this.page = this.figure.pageNumber + 1;
            this.filter();
        }
    }

    numberPage(): number[] {
        if (this.figure == null || this.figure.totalPages <= 0) {
            return [];
        }

        const totalPage = this.figure.totalPages;
        const current = this.figure.pageNumber;

        const pageStart = Math.max(1, current - 1);
        const pageEnd = Math.min(totalPage, current + 1);

        const result: number[] = [];

        for (let page = pageStart; page <= pageEnd; page++) {
            result.push(page);
        }
        return result;
    }
    onAddToCart(productId: string, quantity: number): void{
        const userId = "C1E15921-E8F6-4CBC-EACD-08DD67BB3796";
        this.cartService.addToCart(userId, productId, quantity).pipe(takeUntil(this.destroy$)).subscribe({
          next: (response) => {
            console.log('Product add successfully', response);
          },
          error: (err) => {
            console.error('Error when adding product to cart.', err);
          }
        });
    }

}
