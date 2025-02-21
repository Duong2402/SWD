import { CommonModule } from '@angular/common';
import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { FigureService } from '../../UI-Admin/Product/figure.service';
import { BaseProductDto } from '../../UI-Admin/Product/Model/Figure';
import { PagedResult } from '../Model/PageResult';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-navigation',
  standalone: true,
  imports: [RouterLink, CommonModule, FormsModule],
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.css'
})
export class NavigationComponent implements OnInit, OnDestroy {

  isNavbarVisible = true;
  lastScrollPosition = 0;
  searchResults?: PagedResult<BaseProductDto>;
  page?: number = 1;
  pageSize: number = 4;
  selectResult: any;
  searchQuery?: string;
  showResults: boolean = false;
  private destroy$ = new Subject<void>();


  constructor(private figure: FigureService, private router: Router) {

  }
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
    const currentScroll = window.scrollY || document.documentElement.scrollTop;

    if (currentScroll > this.lastScrollPosition && currentScroll > 50) {
      this.isNavbarVisible = false;
    } else {
      this.isNavbarVisible = true;
    }

    this.lastScrollPosition = currentScroll;
  }

  hideResults() {
    setTimeout(() => {
      this.showResults = false;
    }, 200); // Delay để tránh mất focus trước khi click
  }

  SearchAll() {
    console.log(this.searchQuery);
    this.figure.filterProduct(this.searchQuery, "", "", "", 0, 100000, this.page, this.pageSize).
      pipe(takeUntil(this.destroy$)).subscribe({
        next: response => {
          this.searchResults = response;
          console.log("Response", response);
          console.log("Search", this.searchResults);

        },
        error: err => {
          console.log(err);
        }
      })
  }


}
