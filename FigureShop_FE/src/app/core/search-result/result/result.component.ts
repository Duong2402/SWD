import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationComponent } from "../../navigation/navigation.component";
import { BaseProductDto } from '../../../UI-Admin/Product/Model/Figure';
import { PagedResult } from '../../Model/PageResult';
import { ActivatedRoute, Router } from '@angular/router';
import { FigureService } from '../../../UI-Admin/Product/services/figure.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-result',
  standalone: true,
  imports: [NavigationComponent],
  templateUrl: './result.component.html',
  styleUrl: './result.component.css'
})
export class ResultComponent implements OnInit, OnDestroy {
  searchQuery: string = '';
  searchResults?: PagedResult<BaseProductDto>;
  private destroy$ = new Subject<void>();

  constructor(private route: ActivatedRoute, private router: Router
    , private figure: FigureService
  ) {
  }
  ngOnDestroy(): void {
    this.destroy$.next;
    this.destroy$.complete;
  }
  
  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.searchQuery = params['name'] || ''; // Lấy từ queryParams
      this.fetchResults();
    });
  }
  

  fetchResults() {
    this.figure.filterProduct(this.searchQuery, "", "", "", 0, Number.MAX_VALUE, 1, 100)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: response => {
          this.searchResults = response;
        },
        error: err => {
          console.log(err);
        }
      });
  }





}
