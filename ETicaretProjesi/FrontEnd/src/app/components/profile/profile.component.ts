import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule, SlicePipe } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { AddressService } from '../../services/address.service';
import { OrderService } from '../../services/order.service';
import { Address, Order } from '../../models/models';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [FormsModule, CommonModule],
  template: `
    <div class="page">
      <div class="container">
        <h1 class="page-title">👤 Profilim</h1>

        <!-- Tabs -->
        <div class="tabs">
          <button class="tab" [class.active]="activeTab === 'profile'" (click)="activeTab = 'profile'">Profil</button>
          <button class="tab" [class.active]="activeTab === 'addresses'" (click)="activeTab = 'addresses'; loadAddresses()">Adreslerim</button>
          <button class="tab" [class.active]="activeTab === 'orders'" (click)="activeTab = 'orders'; loadOrders()">Siparişlerim</button>
        </div>

        <!-- Profile Tab -->
        @if (activeTab === 'profile') {
          <div class="tab-content">
            <div class="profile-card">
              <div class="avatar">{{ authService.getUsername().charAt(0).toUpperCase() }}</div>
              <h2>{{ authService.getUsername() }}</h2>
              <span class="role-badge">{{ authService.getUserRole() }}</span>
            </div>

            <!-- Üyelik Bilgileri -->
            <div class="section-title">
              <span class="section-icon">📋</span>
              Üyelik Bilgileri
            </div>
            <div class="info-grid">
              <div class="info-card">
                <div class="info-icon">👤</div>
                <div class="info-content">
                  <span class="info-label">Kullanıcı Adı</span>
                  <span class="info-value">{{ authService.getUsername() }}</span>
                </div>
              </div>
              <div class="info-card">
                <div class="info-icon">🛡️</div>
                <div class="info-content">
                  <span class="info-label">Hesap Rolü</span>
                  <span class="info-value">{{ authService.getUserRole() === 'Admin' ? 'Yönetici' : 'Müşteri' }}</span>
                </div>
              </div>
              <div class="info-card">
                <div class="info-icon">🆔</div>
                <div class="info-content">
                  <span class="info-label">Üyelik No</span>
                  <span class="info-value">#{{ authService.getUserId() }}</span>
                </div>
              </div>
              <div class="info-card">
                <div class="info-icon">✅</div>
                <div class="info-content">
                  <span class="info-label">Hesap Durumu</span>
                  <span class="info-value status-active">Aktif</span>
                </div>
              </div>
            </div>

            <!-- Şifre Güncelleme -->
            <div class="section-title">
              <span class="section-icon">🔐</span>
              Şifre Güncelle
            </div>
            <div class="password-form-card">
              @if (pwMessage) {
                <div class="alert" [class.success]="pwMsgType === 'success'" [class.error]="pwMsgType === 'error'">{{ pwMessage }}</div>
              }
              <div class="form-group">
                <label>Mevcut Şifre</label>
                <div class="input-wrapper">
                  <span class="input-icon">🔑</span>
                  <input [type]="showCurrentPw ? 'text' : 'password'" [(ngModel)]="passwordData.currentPassword" class="form-input icon-input" placeholder="Mevcut şifrenizi girin" />
                  <button class="toggle-pw" (click)="showCurrentPw = !showCurrentPw">{{ showCurrentPw ? '🙈' : '👁️' }}</button>
                </div>
              </div>
              <div class="form-group">
                <label>Yeni Şifre</label>
                <div class="input-wrapper">
                  <span class="input-icon">🔒</span>
                  <input [type]="showNewPw ? 'text' : 'password'" [(ngModel)]="passwordData.newPassword" class="form-input icon-input" placeholder="Yeni şifrenizi girin (min 6 karakter)" />
                  <button class="toggle-pw" (click)="showNewPw = !showNewPw">{{ showNewPw ? '🙈' : '👁️' }}</button>
                </div>
              </div>
              <div class="form-group">
                <label>Yeni Şifre (Tekrar)</label>
                <div class="input-wrapper">
                  <span class="input-icon">🔒</span>
                  <input [type]="showConfirmPw ? 'text' : 'password'" [(ngModel)]="passwordData.confirmNewPassword" class="form-input icon-input" placeholder="Yeni şifrenizi tekrar girin" />
                  <button class="toggle-pw" (click)="showConfirmPw = !showConfirmPw">{{ showConfirmPw ? '🙈' : '👁️' }}</button>
                </div>
              </div>
              <button class="btn-primary" (click)="changePassword()" [disabled]="pwLoading">
                {{ pwLoading ? '⏳ Güncelleniyor...' : '🔄 Şifreyi Güncelle' }}
              </button>
            </div>
          </div>
        }

        <!-- Addresses Tab -->
        @if (activeTab === 'addresses') {
          <div class="tab-content">
            @if (message) {
              <div class="alert" [class.success]="msgType === 'success'" [class.error]="msgType === 'error'">{{ message }}</div>
            }
            <div class="address-grid">
              <div class="address-list">
                @if (addresses.length > 0) {
                  @for (addr of addresses; track addr.id) {
                    <div class="address-card">
                      <h4>{{ addr.title }}</h4>
                      <p>{{ addr.fullAddress }}</p>
                      <p class="address-meta">{{ addr.district }} / {{ addr.city }}</p>
                    </div>
                  }
                } @else {
                  <div class="empty-state"><p>📍 Henüz adres eklenmemiş.</p></div>
                }
              </div>
              <div class="address-form-card">
                <h3>➕ Yeni Adres Ekle</h3>
                <div class="form-group">
                  <label>Adres Başlığı</label>
                  <input type="text" [(ngModel)]="newAddress.title" class="form-input" placeholder="Ev, İş vb." />
                </div>
                <div class="form-group">
                  <label>Tam Adres</label>
                  <textarea [(ngModel)]="newAddress.fullAddress" class="form-input" rows="3" placeholder="Sokak, mahalle, bina no..."></textarea>
                </div>
                <div class="form-row">
                  <div class="form-group">
                    <label>İlçe</label>
                    <input type="text" [(ngModel)]="newAddress.district" class="form-input" placeholder="İlçe" />
                  </div>
                  <div class="form-group">
                    <label>Şehir</label>
                    <input type="text" [(ngModel)]="newAddress.city" class="form-input" placeholder="Şehir" />
                  </div>
                </div>
                <button class="btn-primary" (click)="addAddress()">Adresi Kaydet</button>
              </div>
            </div>
          </div>
        }

        <!-- Orders Tab -->
        @if (activeTab === 'orders') {
          <div class="tab-content">
            @if (orders.length > 0) {
              @for (order of orders; track order.id) {
                <div class="order-card">
                  <div class="order-header">
                    <div>
                      <span class="order-id">Sipariş #{{ order.id }}</span>
                      <span class="order-date">{{ order.orderDate | slice:0:10 }}</span>
                    </div>
                    <span class="order-status" [class]="getStatusClass(order.status)">{{ order.status }}</span>
                  </div>
                  <div class="order-items">
                    @for (item of order.items; track item.productName) {
                      <div class="order-item">
                        <span class="item-name">{{ item.productName }}</span>
                        <span class="item-qty">x{{ item.quantity }}</span>
                        <span class="item-price">₺{{ (item.price * item.quantity).toFixed(2) }}</span>
                      </div>
                    }
                  </div>
                  <div class="order-footer">
                    <span class="total-label">Toplam:</span>
                    <span class="total-price">₺{{ order.totalPrice.toFixed(2) }}</span>
                  </div>
                </div>
              }
            } @else {
              <div class="empty-state"><p>📭 Henüz siparişiniz yok.</p></div>
            }
          </div>
        }
      </div>
    </div>
  `,
  styles: [`
    .page { padding: 2rem; }
    .container { max-width: 1000px; margin: 0 auto; }
    .page-title { color: #fff; font-size: 2rem; font-weight: 700; margin-bottom: 1.5rem; }

    .tabs { display: flex; gap: 4px; margin-bottom: 2rem; background: rgba(255,255,255,0.03); border-radius: 12px; padding: 4px; }
    .tab {
      flex: 1; padding: 12px; border: none; border-radius: 10px;
      background: transparent; color: #a8b2d1; font-weight: 600;
      cursor: pointer; transition: all 0.3s; font-size: 0.95rem;
    }
    .tab.active { background: rgba(233,69,96,0.2); color: #e94560; }
    .tab:hover:not(.active) { background: rgba(255,255,255,0.05); }

    .tab-content { animation: fadeIn 0.3s ease; }
    @keyframes fadeIn { from { opacity: 0; transform: translateY(10px); } to { opacity: 1; transform: translateY(0); } }

    /* Profile */
    .profile-card {
      text-align: center; padding: 3rem;
      background: rgba(255,255,255,0.05); border: 1px solid rgba(255,255,255,0.1);
      border-radius: 20px;
    }
    .avatar {
      width: 80px; height: 80px; border-radius: 50%; margin: 0 auto 1rem;
      background: linear-gradient(135deg, #e94560, #c23152);
      display: flex; align-items: center; justify-content: center;
      font-size: 2rem; font-weight: 700; color: #fff;
    }
    .profile-card h2 { color: #fff; margin: 0; }
    .role-badge {
      display: inline-block; margin-top: 8px; padding: 4px 16px;
      border-radius: 20px; font-size: 0.8rem; font-weight: 600;
      background: rgba(74,222,128,0.15); color: #4ade80;
    }

    /* Section Title */
    .section-title {
      display: flex; align-items: center; gap: 10px;
      color: #fff; font-size: 1.2rem; font-weight: 700;
      margin: 2rem 0 1rem; padding-bottom: 0.5rem;
      border-bottom: 2px solid rgba(233,69,96,0.3);
    }
    .section-icon { font-size: 1.3rem; }

    /* Info Grid */
    .info-grid {
      display: grid; grid-template-columns: 1fr 1fr; gap: 1rem;
    }
    .info-card {
      display: flex; align-items: center; gap: 14px;
      padding: 1.2rem 1.4rem;
      background: rgba(255,255,255,0.04);
      border: 1px solid rgba(255,255,255,0.08);
      border-radius: 16px;
      transition: all 0.3s ease;
    }
    .info-card:hover {
      background: rgba(255,255,255,0.07);
      border-color: rgba(233,69,96,0.3);
      transform: translateY(-2px);
    }
    .info-icon {
      width: 48px; height: 48px; border-radius: 14px;
      background: rgba(233,69,96,0.12);
      display: flex; align-items: center; justify-content: center;
      font-size: 1.4rem; flex-shrink: 0;
    }
    .info-content { display: flex; flex-direction: column; gap: 4px; }
    .info-label { color: #6b7a99; font-size: 0.8rem; font-weight: 500; text-transform: uppercase; letter-spacing: 0.5px; }
    .info-value { color: #fff; font-size: 1rem; font-weight: 600; }
    .status-active { color: #4ade80 !important; }

    /* Password Form */
    .password-form-card {
      padding: 2rem;
      background: rgba(255,255,255,0.04);
      border: 1px solid rgba(255,255,255,0.08);
      border-radius: 20px;
    }
    .input-wrapper {
      position: relative; display: flex; align-items: center;
    }
    .input-icon {
      position: absolute; left: 14px; font-size: 1rem; z-index: 1;
      pointer-events: none;
    }
    .icon-input { padding-left: 42px !important; padding-right: 42px !important; }
    .toggle-pw {
      position: absolute; right: 10px;
      background: none; border: none; cursor: pointer;
      font-size: 1rem; padding: 4px;
      opacity: 0.6; transition: opacity 0.2s;
    }
    .toggle-pw:hover { opacity: 1; }

    /* Addresses */
    .address-grid { display: grid; grid-template-columns: 1fr 380px; gap: 2rem; }
    .address-card {
      padding: 1rem; background: rgba(255,255,255,0.05); border: 1px solid rgba(255,255,255,0.1);
      border-radius: 12px; margin-bottom: 1rem;
    }
    .address-card h4 { color: #e94560; margin: 0 0 6px; font-size: 1rem; }
    .address-card p { color: #a8b2d1; margin: 0; font-size: 0.9rem; }
    .address-meta { margin-top: 4px !important; font-size: 0.8rem !important; color: #6b7280 !important; }
    .address-form-card {
      padding: 1.5rem; background: rgba(255,255,255,0.05); border: 1px solid rgba(255,255,255,0.1);
      border-radius: 16px; height: fit-content; position: sticky; top: 80px;
    }
    .address-form-card h3 { color: #fff; font-size: 1.1rem; margin: 0 0 1rem; }
    .form-group { margin-bottom: 1rem; }
    .form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 1rem; }
    label { display: block; color: #a8b2d1; font-size: 0.85rem; margin-bottom: 4px; }
    .form-input {
      width: 100%; padding: 10px 14px; border: 1px solid rgba(255,255,255,0.1);
      border-radius: 10px; background: rgba(255,255,255,0.05); color: #fff;
      font-size: 0.9rem; outline: none; box-sizing: border-box; font-family: inherit;
    }
    .form-input:focus { border-color: #e94560; }
    textarea.form-input { resize: vertical; }
    .btn-primary {
      width: 100%; padding: 12px; border: none; border-radius: 10px;
      background: linear-gradient(135deg, #e94560, #c23152); color: #fff;
      font-weight: 600; cursor: pointer; transition: all 0.3s; font-size: 0.95rem;
    }
    .btn-primary:hover:not(:disabled) { transform: translateY(-2px); box-shadow: 0 8px 25px rgba(233,69,96,0.4); }
    .btn-primary:disabled { opacity: 0.6; cursor: not-allowed; }

    /* Orders */
    .order-card {
      background: rgba(255,255,255,0.05); border: 1px solid rgba(255,255,255,0.1);
      border-radius: 16px; padding: 1.5rem; margin-bottom: 1rem;
    }
    .order-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1rem; }
    .order-id { color: #fff; font-weight: 700; margin-right: 12px; }
    .order-date { color: #a8b2d1; font-size: 0.85rem; }
    .order-status { padding: 6px 14px; border-radius: 20px; font-size: 0.8rem; font-weight: 600; }
    .status-beklemede { background: rgba(251,191,36,0.15); color: #fbbf24; }
    .status-hazirlaniyor { background: rgba(96,165,250,0.15); color: #60a5fa; }
    .status-kargoda { background: rgba(168,130,255,0.15); color: #a882ff; }
    .status-teslim { background: rgba(74,222,128,0.15); color: #4ade80; }
    .status-iptal { background: rgba(239,68,68,0.15); color: #ef4444; }
    .order-items { border-top: 1px solid rgba(255,255,255,0.05); padding-top: 1rem; }
    .order-item { display: flex; justify-content: space-between; padding: 6px 0; color: #a8b2d1; }
    .item-name { flex: 1; } .item-qty { width: 60px; text-align: center; } .item-price { width: 100px; text-align: right; color: #fff; }
    .order-footer { display: flex; justify-content: flex-end; gap: 8px; margin-top: 1rem; padding-top: 1rem; border-top: 1px solid rgba(255,255,255,0.05); }
    .total-label { color: #a8b2d1; } .total-price { color: #4ade80; font-weight: 700; font-size: 1.1rem; }

    .empty-state { text-align: center; padding: 3rem; color: #a8b2d1; font-size: 1.1rem; }
    .alert { padding: 10px 16px; border-radius: 10px; margin-bottom: 1rem; text-align: center; font-size: 0.9rem; }
    .alert.success { background: rgba(74,222,128,0.15); color: #4ade80; }
    .alert.error { background: rgba(239,68,68,0.15); color: #ef4444; }

    @media (max-width: 768px) {
      .address-grid { grid-template-columns: 1fr; }
      .info-grid { grid-template-columns: 1fr; }
      .tabs { flex-direction: column; }
    }
  `]
})
export class ProfileComponent implements OnInit {
  authService = inject(AuthService);
  private addressService = inject(AddressService);
  private orderService = inject(OrderService);

  activeTab = 'profile';
  addresses: Address[] = [];
  orders: Order[] = [];
  message = '';
  msgType: 'success' | 'error' = 'success';

  newAddress = { title: '', fullAddress: '', city: '', district: '' };

  // Password change
  passwordData = { currentPassword: '', newPassword: '', confirmNewPassword: '' };
  pwMessage = '';
  pwMsgType: 'success' | 'error' = 'success';
  pwLoading = false;
  showCurrentPw = false;
  showNewPw = false;
  showConfirmPw = false;

  ngOnInit() { }

  loadAddresses() {
    this.addressService.getAddresses().subscribe(res => {
      if (res.success) this.addresses = res.data;
    });
  }

  addAddress() {
    if (!this.newAddress.title || !this.newAddress.fullAddress || !this.newAddress.city) {
      this.message = 'Lütfen zorunlu alanları doldurun.'; this.msgType = 'error'; return;
    }
    this.addressService.addAddress(this.newAddress).subscribe({
      next: () => {
        this.message = 'Adres eklendi! 🎉'; this.msgType = 'success';
        this.newAddress = { title: '', fullAddress: '', city: '', district: '' };
        this.loadAddresses();
      },
      error: () => { this.message = 'Adres eklenemedi.'; this.msgType = 'error'; }
    });
  }

  loadOrders() {
    this.orderService.getMyOrders().subscribe(res => {
      if (res.success) this.orders = res.data;
    });
  }

  changePassword() {
    const { currentPassword, newPassword, confirmNewPassword } = this.passwordData;

    if (!currentPassword || !newPassword || !confirmNewPassword) {
      this.pwMessage = 'Lütfen tüm alanları doldurun.'; this.pwMsgType = 'error'; return;
    }
    if (newPassword !== confirmNewPassword) {
      this.pwMessage = 'Yeni şifreler eşleşmiyor.'; this.pwMsgType = 'error'; return;
    }
    if (newPassword.length < 6) {
      this.pwMessage = 'Yeni şifre en az 6 karakter olmalıdır.'; this.pwMsgType = 'error'; return;
    }

    this.pwLoading = true;
    this.pwMessage = '';
    this.authService.changePassword(currentPassword, newPassword, confirmNewPassword).subscribe({
      next: (res) => {
        this.pwLoading = false;
        if (res.success) {
          this.pwMessage = res.message || 'Şifre başarıyla güncellendi! 🎉';
          this.pwMsgType = 'success';
          this.passwordData = { currentPassword: '', newPassword: '', confirmNewPassword: '' };
        } else {
          this.pwMessage = res.message || 'Şifre güncellenemedi.';
          this.pwMsgType = 'error';
        }
      },
      error: (err) => {
        this.pwLoading = false;
        this.pwMessage = err.error?.message || 'Şifre güncellenirken bir hata oluştu.';
        this.pwMsgType = 'error';
      }
    });
  }

  getStatusClass(status: string): string {
    const map: Record<string, string> = {
      'Beklemede': 'status-beklemede', 'Hazırlanıyor': 'status-hazirlaniyor',
      'Kargoda': 'status-kargoda', 'Teslim Edildi': 'status-teslim', 'İptal Edildi': 'status-iptal'
    };
    return map[status] || 'status-beklemede';
  }
}
