import { Component, OnDestroy, OnInit } from '@angular/core';
import { ProductDetail } from '../Model/Figure';
import { FigureService } from '../services/figure.service';
import { CategoryList } from '../../Category/Model/Category.Model';
import { Subject } from 'rxjs/internal/Subject';
import { ActivatedRoute } from '@angular/router';
import { takeUntil } from 'rxjs/internal/operators/takeUntil';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.css'
})
export class ProductDetailAdminComponent implements OnInit, OnDestroy {

  idParam?: string;
  product?: ProductDetail;
  category: CategoryList[] = [];
  previewImages: string[] = [];
  selectedFiles: File[] = [];
  private destroy$ = new Subject<void>();

  constructor(private figure: FigureService, private router: ActivatedRoute) {
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  ngOnInit(): void {
    this.router.params.subscribe(params => {
      this.idParam = params['id'];
      this.getProductDetail();
      this.loadCategory();
    });
  }

  getProductDetail() {
    console.log("ID: ", this.idParam);
    if (this.idParam) {
      this.figure.getDetail(this.idParam).pipe(takeUntil(this.destroy$)).subscribe({
        next: (product) => {
          this.product = product;
          console.log("Product: ", this.product);
        },
        error: (error) => {
          console.log("Error: ", error);
        }
      });
    }
  }

  loadCategory(): void {
    this.figure.getCategories().pipe(takeUntil(this.destroy$)).subscribe({
      next: response => {
        this.category = response;
      },
      error: err => {
        console.error('Failed to get categories:', err);
        alert('Failed to get categories!');
      }
    });
  }

  onFilesSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFiles = Array.from(input.files);
      this.previewImages = [];

      // Hiển thị preview cho từng ảnh
      this.selectedFiles.forEach(file => {
        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.previewImages.push(e.target.result);
        };
        reader.readAsDataURL(file);
      });
    }
  }

  updateProduct() {
    if (!this.product) return;
    const formData = new FormData();
    formData.append('id', this.product.id);
    formData.append('name', this.product.name);
    formData.append('description', this.product.description ? this.product.description : '');
    formData.append('price', this.product.price.toString());
    formData.append('stockQuantity', this.product.stockQuantity.toString());
    formData.append('categoryId', this.product.categoryId ? this.product.categoryId.toString() : this.category[0].id);

    if (this.product.imageUrl && this.product.imageUrl.length > 0) {
      this.product.imageUrl.forEach(media => {
        formData.append('media', JSON.stringify({ id: media.id, name: media.url }));
      });
    }

    if (this.selectedFiles.length > 0) {
      this.selectedFiles.forEach(file => {
        formData.append('Media', file);
      });
    }

    this.figure.updateProduct(this.product.id, formData).pipe(takeUntil(this.destroy$)).subscribe({
      next: response => {
        console.log('Update product successfully:', response);
        alert('Update product successfully!');
      },
      error: err => {
        console.error('Failed to update product:', err);
        alert('Failed to update product!');
      }
    });
  }

}
