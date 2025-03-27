import { Component, OnDestroy, OnInit } from '@angular/core';
import { CategoryList } from '../Model/Category.Model';
import { CategoryService } from '../category.service';
import { Subject } from 'rxjs/internal/Subject';
import { takeUntil } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SideBarComponent } from "../../core/side-bar/side-bar.component";

@Component({
  selector: 'app-list-category',
  standalone: true,
  imports: [CommonModule, FormsModule, SideBarComponent],
  templateUrl: './list-category.component.html',
  styleUrl: './list-category.component.css'
})
export class ListCategoryComponent implements OnInit, OnDestroy {

  categories: CategoryList[] = [];
  newCategory: { name: string; description: string } = { name: '', description: '' };
  private destroy$ = new Subject<void>();

  constructor(private categoryService: CategoryService) { }
  ngOnDestroy(): void {
    this.destroy$.next;
    this.destroy$.complete;
  }
  ngOnInit(): void {
    this.loadCategories();
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

  addCategory() {
    if (this.newCategory.name.trim() && this.newCategory.description.trim()) {
      this.categoryService.createCategory(this.newCategory).subscribe(() => {
        this.loadCategories();
        this.newCategory = { name: '', description: '' }; // Reset form
      });
    }
  }
  deleteCategory(id: string) {
    if (confirm('Bạn có chắc muốn xóa?')) {
      this.categoryService.deleteCategory(id).subscribe(() => {
        this.loadCategories();
      });
    }
  }


}
