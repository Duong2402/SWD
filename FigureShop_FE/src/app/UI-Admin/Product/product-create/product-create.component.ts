import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ProductCreate } from '../Model/Figure';
import { FigureService } from '../services/figure.service';
import { Subject, takeUntil } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CategoryList } from '../../Category/Model/Category.Model';

@Component({
  selector: 'app-product-create',
  standalone: true,
  imports: [CommonModule, FormsModule, CommonModule],
  templateUrl: './product-create.component.html',
  styleUrl: './product-create.component.css'
})
export class ProductCreateComponent implements OnDestroy, OnInit {
  imagePreviews: string[] = [];
  category: CategoryList[] = [];
  selectedFiles: File[] = [];
  private destroy$ = new Subject<void>();
  productCreate: ProductCreate = {
    name: '',
    description: '',
    price: 0,
    stockQuantity: 0,
    categoryId: '',
    media: []
  };

  constructor(private figureService: FigureService) { }
  ngOnInit(): void {
    this.loadCategory();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }


  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;

    if (input.files && input.files.length > 0) {
      this.selectedFiles = []; // Reset danh sách file
      this.imagePreviews = []; // Reset danh sách preview

      for (let i = 0; i < input.files.length; i++) {
        const file = input.files[i];
        this.selectedFiles.push(file); // Lưu file vào selectedFiles

        const reader = new FileReader();
        reader.onload = (e: ProgressEvent<FileReader>) => {
          if (e.target?.result) {
            this.imagePreviews.push(e.target.result as string);
          }
        };
        reader.readAsDataURL(file);
      }

      console.log('Selected files:', this.selectedFiles.map(file => file.name)); // Kiểm tra file name
    }
  }

  loadCategory(): void {
    this.figureService.getCategories().pipe(takeUntil(this.destroy$)).subscribe({
      next: response => {
        this.category = response;
      },
      error: err => {
        console.error('Failed to get categories:', err);
        alert('Failed to get categories!');
      }
    });
  }

  createProduct(): void {
    const formData = new FormData();
    formData.append('name', this.productCreate?.name || '');
    formData.append('description', this.productCreate?.description || '');
    formData.append('price', this.productCreate?.price.toString() || '');
    formData.append('stockQuantity', this.productCreate?.stockQuantity.toString() || '');
    formData.append('categoryId', this.productCreate?.categoryId || '');

    this.selectedFiles.forEach((file, index) => {
      formData.append(`media`, file);
    });

    console.log('🔹 FormData contents:');
    formData.forEach((value, key) => {
      console.log(`${key}:`, value);
    });

    this.figureService.createProduct(formData).pipe(takeUntil(this.destroy$)).subscribe({
      next: response => {
        console.log('Product created:', response);
        alert('Product created successfully!');
      },
      error: err => {
        console.error('Failed to create product:', err);
        alert('Failed to create product!');
      }
    });
  }



}
