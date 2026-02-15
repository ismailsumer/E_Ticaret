import { Component, inject, OnInit, PLATFORM_ID } from '@angular/core';
import { RouterLink } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { ProductService } from '../../../services/product.service';
import { CategoryService } from '../../../services/category.service';
import { SliderService } from '../../../services/slider.service';
import { Product, Category, Slider } from '../../../models/models';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink],
  template: `
    <!-- Slider/Carousel -->
    @if (sliders.length > 0) {
      <section class="slider-section">
        <div class="slider-container">
          <div class="slide" [style.background-image]="'url(http://localhost:5292' + sliders[currentSlide].imageUrl + ')'">
            <div class="slide-overlay">
              <div class="slide-content">
                <h1>{{ sliders[currentSlide].title }}</h1>
                <p>{{ sliders[currentSlide].description }}</p>
                @if (sliders[currentSlide].linkUrl) {
                  <a [href]="sliders[currentSlide].linkUrl" class="slide-btn">Keşfet →</a>
                }
              </div>
            </div>
          </div>
          @if (sliders.length > 1) {
            <button class="slider-nav prev" (click)="prevSlide()">❮</button>
            <button class="slider-nav next" (click)="nextSlide()">❯</button>
            <div class="slider-dots">
              @for (s of sliders; track s.id; let i = $index) {
                <span class="dot" [class.active]="i === currentSlide" (click)="currentSlide = i"></span>
              }
            </div>
          }
        </div>
      </section>
    } @else {
      <!-- Fallback Hero -->
      <section class="hero">
        <div class="hero-content">
          <h1>Kaliteli Ürünler, <span class="highlight">Uygun Fiyatlar</span></h1>
          <p>Binlerce ürün arasından ihtiyacınız olanı bulun. Hızlı ve güvenilir teslimat.</p>
          <a routerLink="/products" class="hero-btn">Alışverişe Başla →</a>
        </div>
      </section>
    }

    <!-- Categories Section -->
    <section class="section">
      <div class="container">
        <h2 class="section-title">Kategoriler</h2>
        <div class="categories-grid">
          @for (category of categories; track category.id) {
            <a [routerLink]="['/products']" [queryParams]="{category: category.id}" class="category-card">
              <span class="category-icon">📦</span>
              <span class="category-name">{{ category.name }}</span>
            </a>
          }
        </div>
      </div>
    </section>

    <!-- Featured Products -->
    <section class="section">
      <div class="container">
        <h2 class="section-title">Öne Çıkan Ürünler</h2>
        <div class="products-grid">
          @for (product of featuredProducts; track product.id) {
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
                <p class="product-price">₺{{ product.price.toFixed(2) }}</p>
              </div>
            </a>
          }
        </div>
        <div class="center">
          <a routerLink="/products" class="see-all-btn">Tüm Ürünleri Gör →</a>
        </div>
      </div>
    </section>
  `,
  styles: [`
    /* Slider */
    .slider-section { position: relative; overflow: hidden; }
    .slider-container { position: relative; height: 420px; }
    .slide {
      width: 100%; height: 100%; background-size: cover; background-position: center;
      transition: background-image 0.5s ease;
    }
    .slide-overlay {
      width: 100%; height: 100%;
      background: linear-gradient(135deg, rgba(15,52,96,0.85) 0%, rgba(26,26,46,0.9) 50%, rgba(233,69,96,0.4) 100%);
      display: flex; align-items: center; justify-content: center;
    }
    .slide-content { text-align: center; max-width: 700px; padding: 2rem; }
    .slide-content h1 { color: #fff; font-size: 2.8rem; font-weight: 800; margin-bottom: 1rem; line-height: 1.2; }
    .slide-content p { color: #a8b2d1; font-size: 1.1rem; margin-bottom: 1.5rem; }
    .slide-btn {
      display: inline-block; background: #e94560; color: #fff;
      padding: 12px 32px; border-radius: 12px; text-decoration: none;
      font-weight: 600; transition: all 0.3s;
    }
    .slide-btn:hover { background: #c23152; transform: translateY(-2px); box-shadow: 0 8px 25px rgba(233,69,96,0.4); }
    .slider-nav {
      position: absolute; top: 50%; transform: translateY(-50%);
      background: rgba(255,255,255,0.1); backdrop-filter: blur(10px);
      border: 1px solid rgba(255,255,255,0.2); color: #fff;
      width: 44px; height: 44px; border-radius: 50%; cursor: pointer;
      font-size: 1.2rem; transition: all 0.3s; display: flex; align-items: center; justify-content: center;
    }
    .slider-nav:hover { background: rgba(233,69,96,0.5); }
    .slider-nav.prev { left: 16px; }
    .slider-nav.next { right: 16px; }
    .slider-dots { position: absolute; bottom: 16px; left: 50%; transform: translateX(-50%); display: flex; gap: 8px; }
    .dot {
      width: 10px; height: 10px; border-radius: 50%; background: rgba(255,255,255,0.3);
      cursor: pointer; transition: all 0.3s;
    }
    .dot.active { background: #e94560; transform: scale(1.3); }

    /* Hero fallback */
    .hero {
      background: linear-gradient(135deg, #0f3460 0%, #1a1a2e 50%, #e94560 100%);
      padding: 6rem 2rem; text-align: center; color: #fff;
    }
    .hero-content { max-width: 700px; margin: 0 auto; }
    .hero h1 { font-size: 3rem; font-weight: 800; margin-bottom: 1rem; line-height: 1.2; }
    .highlight { color: #e94560; }
    .hero p { font-size: 1.2rem; color: #a8b2d1; margin-bottom: 2rem; }
    .hero-btn {
      display: inline-block; background: #e94560; color: #fff;
      padding: 14px 36px; border-radius: 12px; text-decoration: none;
      font-weight: 600; font-size: 1.1rem; transition: all 0.3s;
    }
    .hero-btn:hover { background: #c23152; transform: translateY(-2px); box-shadow: 0 8px 25px rgba(233,69,96,0.4); }

    .section { padding: 4rem 2rem; }
    .container { max-width: 1280px; margin: 0 auto; }
    .section-title { color: #fff; font-size: 1.8rem; font-weight: 700; margin-bottom: 2rem; text-align: center; }

    .categories-grid {
      display: grid; grid-template-columns: repeat(auto-fill, minmax(160px, 1fr)); gap: 1rem;
    }
    .category-card {
      background: rgba(255,255,255,0.05); border: 1px solid rgba(255,255,255,0.1);
      border-radius: 12px; padding: 1.5rem; text-align: center; text-decoration: none;
      transition: all 0.3s; display: flex; flex-direction: column; align-items: center; gap: 8px;
    }
    .category-card:hover { background: rgba(233,69,96,0.1); border-color: #e94560; transform: translateY(-4px); }
    .category-icon { font-size: 2rem; }
    .category-name { color: #fff; font-weight: 500; font-size: 0.95rem; }

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
    .product-price { color: #4ade80; font-size: 1.2rem; font-weight: 700; margin: 0; }

    .center { text-align: center; margin-top: 2rem; }
    .see-all-btn {
      display: inline-block; background: transparent; border: 2px solid #e94560;
      color: #e94560; padding: 12px 32px; border-radius: 12px;
      text-decoration: none; font-weight: 600; transition: all 0.3s;
    }
    .see-all-btn:hover { background: #e94560; color: #fff; }

    @media (max-width: 768px) {
      .hero h1, .slide-content h1 { font-size: 2rem; }
      .slider-container { height: 320px; }
      .products-grid { grid-template-columns: repeat(auto-fill, minmax(180px, 1fr)); }
    }
  `]
})
export class HomeComponent implements OnInit {
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  private sliderService = inject(SliderService);
  private platformId = inject(PLATFORM_ID);

  featuredProducts: Product[] = [];
  categories: Category[] = [];
  sliders: Slider[] = [];
  currentSlide = 0;
  private slideInterval: any;

  ngOnInit() {
    this.productService.getProducts(1, 8).subscribe(res => {
      if (res.success) this.featuredProducts = res.data;
    });
    this.categoryService.getCategories().subscribe(res => {
      if (res.success) this.categories = res.data;
    });
    this.sliderService.getSliders().subscribe(res => {
      if (res.success && res.data.length > 0) {
        this.sliders = res.data.filter(s => s.isActive).sort((a, b) => a.displayOrder - b.displayOrder);
        this.startAutoSlide();
      }
    });
  }

  nextSlide() {
    this.currentSlide = (this.currentSlide + 1) % this.sliders.length;
  }

  prevSlide() {
    this.currentSlide = (this.currentSlide - 1 + this.sliders.length) % this.sliders.length;
  }

  private startAutoSlide() {
    if (isPlatformBrowser(this.platformId)) {
      this.slideInterval = setInterval(() => this.nextSlide(), 5000);
    }
  }
}
