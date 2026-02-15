import { Component, inject, OnInit, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BasketService } from '../../services/basket.service';
import { BasketItem } from '../../models/models';

@Component({
  selector: 'app-basket',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div class="page">
      <div class="container">
        <h1 class="page-title">🛍️ Sepetim</h1>
        @if (items.length > 0) {
          <div class="basket-layout">
            <div class="basket-items">
              @for (item of items; track item.id) {
                <div class="basket-item">
                  <div class="item-image">
                    @if (item.imageUrl) {
                      <img [src]="'http://localhost:5292' + item.imageUrl" [alt]="item.productName" />
                    } @else {
                      <div class="placeholder">📷</div>
                    }
                  </div>
                  <div class="item-info">
                    <h3>{{ item.productName }}</h3>
                    <p class="item-unit-price">₺{{ (item.price || 0).toFixed(2) }} / adet</p>
                  </div>
                  <div class="quantity-controls">
                    <button class="qty-btn" (click)="decreaseQty(item)" [disabled]="item.quantity <= 1">−</button>
                    <span class="qty-value">{{ item.quantity }}</span>
                    <button class="qty-btn" (click)="increaseQty(item)">+</button>
                  </div>
                  <div class="item-price">₺{{ ((item.price || 0) * (item.quantity || 0)).toFixed(2) }}</div>
                </div>
              }
            </div>
            <div class="basket-summary">
              <h3>Sipariş Özeti</h3>
              <div class="summary-row">
                <span>Ürünler ({{ totalItemCount }})</span>
                <span>₺{{ totalPrice.toFixed(2) }}</span>
              </div>
              <div class="summary-row">
                <span>Kargo</span>
                <span class="free">Ücretsiz</span>
              </div>
              <hr />
              <div class="summary-row total">
                <span>Toplam</span>
                <span>₺{{ totalPrice.toFixed(2) }}</span>
              </div>
              <a routerLink="/checkout" class="btn-checkout">Siparişi Tamamla →</a>
              <button class="btn-clear" (click)="clearBasket()">Sepeti Temizle</button>
            </div>
          </div>
        } @else {
          <div class="empty-state">
            <p>🛒 Sepetiniz boş</p>
            <a routerLink="/products" class="shop-btn">Alışverişe Başla</a>
          </div>
        }
        @if (message) {
          <div class="alert" [class.success]="messageType === 'success'" [class.error]="messageType === 'error'">
            {{ message }}
          </div>
        }
      </div>
    </div>
  `,
  styles: [`
    .page { padding: 2rem; }
    .container { max-width: 1280px; margin: 0 auto; }
    .page-title { color: #fff; font-size: 2rem; font-weight: 700; margin-bottom: 2rem; }
    .basket-layout { display: grid; grid-template-columns: 1fr 380px; gap: 2rem; }
    .basket-item {
      display: flex; gap: 1rem; align-items: center; padding: 1rem;
      background: rgba(255,255,255,0.05); border: 1px solid rgba(255,255,255,0.1);
      border-radius: 12px; margin-bottom: 1rem;
    }
    .item-image { width: 80px; height: 80px; border-radius: 10px; overflow: hidden; background: #1a1a2e; flex-shrink: 0; }
    .item-image img { width: 100%; height: 100%; object-fit: cover; }
    .placeholder { width: 100%; height: 100%; display: flex; align-items: center; justify-content: center; font-size: 1.5rem; }
    .item-info { flex: 1; }
    .item-info h3 { color: #fff; font-size: 1rem; margin: 0 0 4px; }
    .item-unit-price { color: #6b7280; font-size: 0.8rem; margin: 0; }
    .item-price { color: #4ade80; font-weight: 700; font-size: 1.1rem; min-width: 100px; text-align: right; }

    .quantity-controls { display: flex; align-items: center; gap: 8px; }
    .qty-btn {
      width: 32px; height: 32px; border-radius: 8px; border: 1px solid rgba(255,255,255,0.15);
      background: rgba(255,255,255,0.05); color: #fff; font-size: 1.1rem;
      cursor: pointer; transition: all 0.2s; display: flex; align-items: center; justify-content: center;
    }
    .qty-btn:hover:not(:disabled) { background: rgba(233,69,96,0.3); border-color: #e94560; }
    .qty-btn:disabled { opacity: 0.3; cursor: not-allowed; }
    .qty-value { color: #fff; font-weight: 700; min-width: 24px; text-align: center; }

    .basket-summary {
      background: rgba(255,255,255,0.05); border: 1px solid rgba(255,255,255,0.1);
      border-radius: 16px; padding: 1.5rem; position: sticky; top: 80px; height: fit-content;
    }
    .basket-summary h3 { color: #fff; font-size: 1.2rem; margin-bottom: 1rem; }
    .summary-row { display: flex; justify-content: space-between; color: #a8b2d1; margin-bottom: 8px; }
    .summary-row .free { color: #4ade80; }
    hr { border: none; border-top: 1px solid rgba(255,255,255,0.1); margin: 1rem 0; }
    .summary-row.total { color: #fff; font-size: 1.2rem; font-weight: 700; }
    .btn-checkout {
      display: block; width: 100%; padding: 14px; border: none; border-radius: 12px;
      background: linear-gradient(135deg, #e94560, #c23152); color: #fff;
      font-size: 1.05rem; font-weight: 600; cursor: pointer; margin-top: 1rem; transition: all 0.3s;
      text-decoration: none; text-align: center; box-sizing: border-box;
    }
    .btn-checkout:hover { transform: translateY(-2px); box-shadow: 0 8px 25px rgba(233,69,96,0.4); }
    .btn-clear {
      width: 100%; padding: 10px; border: 1px solid rgba(255,255,255,0.2);
      border-radius: 12px; background: transparent; color: #a8b2d1;
      cursor: pointer; margin-top: 8px; transition: all 0.3s;
    }
    .btn-clear:hover { border-color: #ef4444; color: #ef4444; }

    .empty-state { text-align: center; padding: 4rem; }
    .empty-state p { color: #a8b2d1; font-size: 1.3rem; margin-bottom: 1.5rem; }
    .shop-btn {
      display: inline-block; background: #e94560; color: #fff; padding: 12px 32px;
      border-radius: 12px; text-decoration: none; font-weight: 600;
    }

    .alert { margin-top: 1rem; padding: 12px; border-radius: 10px; text-align: center; }
    .alert.success { background: rgba(74,222,128,0.15); color: #4ade80; }
    .alert.error { background: rgba(239,68,68,0.15); color: #ef4444; }

    @media (max-width: 768px) {
      .basket-layout { grid-template-columns: 1fr; }
      .basket-item { flex-wrap: wrap; }
    }
  `]
})
export class BasketComponent implements OnInit {
  private basketService = inject(BasketService);
  private platformId = inject(PLATFORM_ID);

  items: BasketItem[] = [];
  totalPrice = 0;
  totalItemCount = 0;
  message = '';
  messageType: 'success' | 'error' = 'success';

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.loadBasket();
    }
  }

  loadBasket() {
    this.basketService.getBasket().subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.items = res.data;
          this.calculateTotals();
        } else {
          this.items = [];
        }
      },
      error: () => {
        this.items = [];
      }
    });
  }

  calculateTotals() {
    this.totalPrice = this.items.reduce((sum, i) => sum + (i.price || 0) * (i.quantity || 0), 0);
    this.totalItemCount = this.items.reduce((sum, i) => sum + i.quantity, 0);
  }

  increaseQty(item: BasketItem) {
    this.basketService.addToBasket(item.productId, 1).subscribe({
      next: () => this.loadBasket(),
      error: () => { this.message = 'Adet artırılamadı.'; this.messageType = 'error'; }
    });
  }

  decreaseQty(item: BasketItem) {
    if (item.quantity <= 1) return;
    this.basketService.addToBasket(item.productId, -1).subscribe({
      next: () => this.loadBasket(),
      error: () => { this.message = 'Adet azaltılamadı.'; this.messageType = 'error'; }
    });
  }

  clearBasket() {
    this.basketService.clearBasket().subscribe(() => {
      this.items = [];
      this.totalPrice = 0;
      this.totalItemCount = 0;
      this.message = 'Sepet temizlendi.';
      this.messageType = 'success';
    });
  }
}
