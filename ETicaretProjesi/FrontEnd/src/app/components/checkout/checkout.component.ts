import { Component, inject, OnInit, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BasketService } from '../../services/basket.service';
import { OrderService } from '../../services/order.service';
import { AddressService } from '../../services/address.service';
import { BasketItem, Address } from '../../models/models';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [RouterLink, FormsModule],
  template: `
    <div class="page">
      <div class="container">
        <h1 class="page-title">📋 Siparişi Tamamla</h1>

        <div class="checkout-layout">
          <!-- Basket Summary -->
          <div class="section">
            <h2 class="section-title">Sepet Özeti</h2>
            @if (items.length > 0) {
              @for (item of items; track item.id) {
                <div class="checkout-item">
                  <span class="ci-name">{{ item.productName }}</span>
                  <span class="ci-qty">x{{ item.quantity }}</span>
                  <span class="ci-price">₺{{ (item.price * item.quantity).toFixed(2) }}</span>
                </div>
              }
              <div class="checkout-total">
                <span>Toplam:</span>
                <span class="total-amount">₺{{ totalPrice.toFixed(2) }}</span>
              </div>
            } @else {
              <p class="empty-msg">Sepetiniz boş. <a routerLink="/products">Ürünlere göz atın →</a></p>
            }
          </div>

          <!-- Address Selection -->
          <div class="section">
            <h2 class="section-title">Teslimat Adresi</h2>
            <div class="address-options">
              @for (addr of addresses; track addr.id) {
                <label class="address-option" [class.selected]="selectedAddressId === addr.id">
                  <input type="radio" name="address" [value]="addr.id" [(ngModel)]="selectedAddressId" />
                  <div class="ao-content">
                    <strong>{{ addr.title }}</strong>
                    <p>{{ addr.fullAddress }}</p>
                    <small>{{ addr.district }} / {{ addr.city }}</small>
                  </div>
                </label>
              }
            </div>

            @if (addresses.length === 0) {
              <p class="empty-msg">Henüz adres eklenmemiş.</p>
            }

            <!-- Add new address inline -->
            <div class="new-address-toggle">
              <button class="btn-outline" (click)="showNewAddress = !showNewAddress">
                {{ showNewAddress ? '✕ İptal' : '➕ Yeni Adres Ekle' }}
              </button>
            </div>

            @if (showNewAddress) {
              <div class="new-address-form">
                <div class="form-group">
                  <input type="text" [(ngModel)]="newAddr.title" placeholder="Adres Başlığı (Ev, İş)" class="form-input" />
                </div>
                <div class="form-group">
                  <textarea [(ngModel)]="newAddr.fullAddress" placeholder="Tam Adres" class="form-input" rows="2"></textarea>
                </div>
                <div class="form-row">
                  <input type="text" [(ngModel)]="newAddr.district" placeholder="İlçe" class="form-input" />
                  <input type="text" [(ngModel)]="newAddr.city" placeholder="Şehir" class="form-input" />
                </div>
                <button class="btn-secondary" (click)="addNewAddress()">Adresi Kaydet</button>
              </div>
            }
          </div>
        </div>

        <!-- Place Order -->
        <div class="order-action">
          @if (message) {
            <div class="alert" [class.success]="msgType === 'success'" [class.error]="msgType === 'error'">{{ message }}</div>
          }
          <button class="btn-checkout" (click)="placeOrder()" [disabled]="items.length === 0 || processing">
            {{ processing ? 'İşleniyor...' : '🛒 Siparişi Oluştur' }}
          </button>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .page { padding: 2rem; }
    .container { max-width: 800px; margin: 0 auto; }
    .page-title { color: #fff; font-size: 2rem; font-weight: 700; margin-bottom: 2rem; }

    .checkout-layout { display: flex; flex-direction: column; gap: 2rem; }
    .section {
      background: rgba(255,255,255,0.05); border: 1px solid rgba(255,255,255,0.1);
      border-radius: 16px; padding: 1.5rem;
    }
    .section-title { color: #fff; font-size: 1.2rem; margin: 0 0 1rem; }

    .checkout-item { display: flex; justify-content: space-between; padding: 8px 0; color: #a8b2d1; border-bottom: 1px solid rgba(255,255,255,0.05); }
    .ci-name { flex: 1; } .ci-qty { width: 60px; text-align: center; } .ci-price { width: 120px; text-align: right; color: #fff; font-weight: 600; }
    .checkout-total { display: flex; justify-content: flex-end; gap: 12px; padding-top: 1rem; margin-top: 1rem; border-top: 1px solid rgba(255,255,255,0.1); color: #fff; font-size: 1.1rem; font-weight: 700; }
    .total-amount { color: #4ade80; }

    .address-options { display: flex; flex-direction: column; gap: 8px; }
    .address-option {
      display: flex; align-items: flex-start; gap: 12px; padding: 1rem;
      background: rgba(255,255,255,0.03); border: 1px solid rgba(255,255,255,0.08);
      border-radius: 12px; cursor: pointer; transition: all 0.3s;
    }
    .address-option.selected { border-color: #e94560; background: rgba(233,69,96,0.08); }
    .address-option input[type="radio"] { margin-top: 4px; accent-color: #e94560; }
    .ao-content strong { color: #e94560; font-size: 0.95rem; }
    .ao-content p { color: #a8b2d1; margin: 4px 0 0; font-size: 0.9rem; }
    .ao-content small { color: #6b7280; }

    .new-address-toggle { margin-top: 1rem; }
    .btn-outline {
      padding: 10px 20px; border: 1px solid rgba(255,255,255,0.2); border-radius: 10px;
      background: transparent; color: #a8b2d1; cursor: pointer; transition: all 0.3s;
    }
    .btn-outline:hover { border-color: #e94560; color: #e94560; }
    .new-address-form { margin-top: 1rem; display: flex; flex-direction: column; gap: 10px; }
    .form-group { display: flex; flex-direction: column; }
    .form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 10px; }
    .form-input {
      padding: 10px 14px; border: 1px solid rgba(255,255,255,0.1); border-radius: 10px;
      background: rgba(255,255,255,0.05); color: #fff; font-size: 0.9rem; outline: none;
      box-sizing: border-box; font-family: inherit;
    }
    .form-input:focus { border-color: #e94560; }
    .btn-secondary {
      padding: 10px; border: none; border-radius: 10px;
      background: rgba(233,69,96,0.2); color: #e94560; font-weight: 600;
      cursor: pointer; transition: all 0.3s;
    }
    .btn-secondary:hover { background: rgba(233,69,96,0.3); }

    .order-action { margin-top: 2rem; text-align: center; }
    .btn-checkout {
      padding: 16px 48px; border: none; border-radius: 14px;
      background: linear-gradient(135deg, #e94560, #c23152); color: #fff;
      font-size: 1.15rem; font-weight: 700; cursor: pointer; transition: all 0.3s;
    }
    .btn-checkout:hover:not(:disabled) { transform: translateY(-2px); box-shadow: 0 8px 30px rgba(233,69,96,0.4); }
    .btn-checkout:disabled { opacity: 0.5; cursor: not-allowed; }

    .empty-msg { color: #a8b2d1; } .empty-msg a { color: #e94560; text-decoration: none; }
    .alert { padding: 10px; border-radius: 10px; margin-bottom: 1rem; text-align: center; font-size: 0.9rem; }
    .alert.success { background: rgba(74,222,128,0.15); color: #4ade80; }
    .alert.error { background: rgba(239,68,68,0.15); color: #ef4444; }

    @media (max-width: 768px) { .form-row { grid-template-columns: 1fr; } }
  `]
})
export class CheckoutComponent implements OnInit {
  private basketService = inject(BasketService);
  private orderService = inject(OrderService);
  private addressService = inject(AddressService);
  private router = inject(Router);
  private platformId = inject(PLATFORM_ID);

  items: BasketItem[] = [];
  addresses: Address[] = [];
  totalPrice = 0;
  selectedAddressId = 0;
  showNewAddress = false;
  newAddr = { title: '', fullAddress: '', city: '', district: '' };
  message = '';
  msgType: 'success' | 'error' = 'success';
  processing = false;

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.basketService.getBasket().subscribe(res => {
        if (res.success) {
          this.items = res.data;
          this.totalPrice = this.items.reduce((sum, i) => sum + i.price * i.quantity, 0);
        }
      });
      this.addressService.getAddresses().subscribe(res => {
        if (res.success) {
          this.addresses = res.data;
          if (this.addresses.length > 0) this.selectedAddressId = this.addresses[0].id;
        }
      });
    }
  }

  addNewAddress() {
    if (!this.newAddr.title || !this.newAddr.fullAddress || !this.newAddr.city) {
      this.message = 'Lütfen adres bilgilerini doldurun.'; this.msgType = 'error'; return;
    }
    this.addressService.addAddress(this.newAddr).subscribe({
      next: () => {
        this.showNewAddress = false;
        this.newAddr = { title: '', fullAddress: '', city: '', district: '' };
        this.addressService.getAddresses().subscribe(res => {
          if (res.success) {
            this.addresses = res.data;
            this.selectedAddressId = this.addresses[this.addresses.length - 1].id;
          }
        });
      },
      error: () => { this.message = 'Adres eklenemedi.'; this.msgType = 'error'; }
    });
  }

  placeOrder() {
    if (this.items.length === 0) return;
    this.processing = true;
    this.orderService.checkout().subscribe({
      next: res => {
        this.processing = false;
        if (res.success) {
          this.message = 'Siparişiniz başarıyla oluşturuldu! 🎉'; this.msgType = 'success';
          this.items = []; this.totalPrice = 0;
          setTimeout(() => this.router.navigate(['/profile']), 2000);
        } else {
          this.message = res.message || 'Sipariş oluşturulamadı.'; this.msgType = 'error';
        }
      },
      error: () => { this.processing = false; this.message = 'Bir hata oluştu.'; this.msgType = 'error'; }
    });
  }
}
