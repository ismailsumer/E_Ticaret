import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ProductService } from '../../../services/product.service';
import { CategoryService } from '../../../services/category.service';
import { Product, Category } from '../../../models/models';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [RouterLink, FormsModule, CommonModule],
  template: `
    <div class="page">
      <div class="container">
        <h1 class="page-title">Ürünler</h1>

        <!-- Filters -->
        <div class="filters">
          <input type="text" [(ngModel)]="search" placeholder="🔍 Ürün ara..." class="search-input" (keyup.enter)="loadProducts()" />
          <select [(ngModel)]="selectedCategory" (change)="loadProducts()" class="filter-select">
            <option [value]="0">Tüm Kategoriler</option>
            @for (cat of categories; track cat.id) {
              <option [value]="cat.id">{{ cat.name }}</option>
            }
          </select>
          <button class="filter-btn" (click)="loadProducts()">Filtrele</button>
        </div>

        <!-- Products Grid -->
        <div class="products-grid">
          @for (product of products; track product.id) {
            <a [routerLink]="['/products', product.id]" class="product-card">
              <div class="product-image">
                @if (product.imageUrl) {
                  <img [src]="'http://localhost:5292' + product.imageUrl" [alt]="product.name" />
                } @else {
                  <div class="placeholder-img">📷</div>
                }
              </div>
              <div class="product-info">
                <span class="product-category">{{ product.categoryName }}</span>
                <h3>{{ product.name }}</h3>
                <p class="product-desc">{{ product.description | slice:0:60 }}...</p>
                <div class="product-footer">
                  <span class="product-price">₺{{ product.price.toFixed(2) }}</span>
                  <span class="stock-badge" [class.out-of-stock]="product.stock === 0">
                    {{ product.stock > 0 ? 'Stokta' : 'Tükendi' }}
                  </span>
                </div>
              </div>
            </a>
          }
        </div>

        @if (products.length === 0) {
          <div class="empty-state">
            <p>📦 Ürün bulunamadı.</p>
          </div>
        }

        <!-- Pagination -->
        @if (totalPages > 1) {
          <div class="pagination">
            <button (click)="changePage(currentPage - 1)" [disabled]="currentPage <= 1" class="page-btn">← Önceki</button>
            <span class="page-info">Sayfa {{ currentPage }} / {{ totalPages }}</span>
            <button (click)="changePage(currentPage + 1)" [disabled]="currentPage >= totalPages" class="page-btn">Sonraki →</button>
          </div>
        }
      </div>
    </div>
  `,
  styles: [`
    .page { padding: 2rem; }
    .container { max-width: 1280px; margin: 0 auto; }
    .page-title { color: #fff; font-size: 2rem; font-weight: 700; margin-bottom: 1.5rem; }

    .filters {
      display: flex; gap: 1rem; margin-bottom: 2rem; flex-wrap: wrap;
    }
    .search-input {
      flex: 1; min-width: 200px; padding: 12px 16px; border-radius: 12px;
      border: 1px solid rgba(255,255,255,0.1); background: rgba(255,255,255,0.05);
      color: #fff; font-size: 0.95rem; outline: none;
    }
    .search-input:focus { border-color: #e94560; }
    .filter-select {
      padding: 12px 16px; border-radius: 12px;
      border: 1px solid rgba(255,255,255,0.1); background: #1a1a2e;
      color: #fff; font-size: 0.95rem; outline: none;
    }
    .filter-btn {
      padding: 12px 24px; border-radius: 12px; border: none;
      background: #e94560; color: #fff; font-weight: 600; cursor: pointer;
      transition: all 0.3s;
    }
    .filter-btn:hover { background: #c23152; }

    .products-grid {
      display: grid; grid-template-columns: repeat(auto-fill, minmax(260px, 1fr)); gap: 1.5rem;
    }
    .product-card {
      background: rgba(255,255,255,0.05); border: 1px solid rgba(255,255,255,0.1);
      border-radius: 16px; overflow: hidden; text-decoration: none;
      transition: all 0.3s;
    }
    .product-card:hover { transform: translateY(-6px); box-shadow: 0 12px 40px rgba(0,0,0,0.3); border-color: #e94560; }
    .product-image { height: 200px; background: #1a1a2e; display: flex; align-items: center; justify-content: center; overflow: hidden; }
    .product-image img { width: 100%; height: 100%; object-fit: cover; }
    .placeholder-img { font-size: 3rem; }
    .product-info { padding: 1rem; }
    .product-category { color: #e94560; font-size: 0.8rem; font-weight: 600; text-transform: uppercase; }
    .product-info h3 { color: #fff; font-size: 1.05rem; font-weight: 600; margin: 6px 0; }
    .product-desc { color: #a8b2d1; font-size: 0.85rem; margin: 4px 0 8px; }
    .product-footer { display: flex; justify-content: space-between; align-items: center; }
    .product-price { color: #4ade80; font-size: 1.15rem; font-weight: 700; }
    .stock-badge { font-size: 0.75rem; padding: 4px 10px; border-radius: 20px; background: rgba(74,222,128,0.15); color: #4ade80; font-weight: 600; }
    .stock-badge.out-of-stock { background: rgba(239,68,68,0.15); color: #ef4444; }

    .empty-state { text-align: center; padding: 4rem; color: #a8b2d1; font-size: 1.2rem; }

    .pagination { display: flex; justify-content: center; align-items: center; gap: 1rem; margin-top: 2rem; }
    .page-btn {
      padding: 10px 20px; border-radius: 10px; border: 1px solid rgba(255,255,255,0.1);
      background: transparent; color: #fff; cursor: pointer; transition: all 0.3s;
    }
    .page-btn:hover:not(:disabled) { background: #e94560; border-color: #e94560; }
    .page-btn:disabled { opacity: 0.4; cursor: not-allowed; }
    .page-info { color: #a8b2d1; }

    @media (max-width: 768px) {
      .products-grid { grid-template-columns: repeat(auto-fill, minmax(180px, 1fr)); }
      .filters { flex-direction: column; }
    }
  `]
})
export class ProductListComponent implements OnInit {
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  private route = inject(ActivatedRoute);

  products: Product[] = [];
  categories: Category[] = [];
  search = '';
  selectedCategory = 0;
  currentPage = 1;
  totalPages = 1;

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      if (params['category']) this.selectedCategory = +params['category'];
      this.loadProducts();
    });
    this.categoryService.getCategories().subscribe(res => {
      if (res.success) this.categories = res.data;
    });
  }

  loadProducts() {
    const catId = this.selectedCategory > 0 ? this.selectedCategory : undefined;
    const searchTerm = this.search.trim() || undefined;
    this.productService.getProducts(this.currentPage, 12, searchTerm, catId).subscribe(res => {
      if (res.success) {
        this.products = res.data;
        this.totalPages = res.totalPages;
      }
    });
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadProducts();
  }
}