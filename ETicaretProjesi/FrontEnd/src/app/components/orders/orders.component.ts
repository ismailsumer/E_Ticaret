import { Component, inject, OnInit } from '@angular/core';
import { OrderService } from '../../services/order.service';
import { Order } from '../../models/models';

import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="page">
      <div class="container">
        <h1 class="page-title">📦 Siparişlerim</h1>
        @if (orders.length > 0) {
          @for (order of orders; track order.id) {
            <div class="order-card">
              <div class="order-header">
                <div>
                  <span class="order-id">Sipariş #{{ order.id }}</span>
                  <span class="order-date">{{ order.orderDate | slice:0:10 }}</span>
                </div>
                <div class="order-status" [class]="getStatusClass(order.status)">
                  {{ order.status }}
                </div>
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
          <div class="empty-state">
            <p>📭 Henüz siparişiniz yok.</p>
          </div>
        }
      </div>
    </div>
  `,
  styles: [`
    .page { padding: 2rem; }
    .container { max-width: 900px; margin: 0 auto; }
    .page-title { color: #fff; font-size: 2rem; font-weight: 700; margin-bottom: 2rem; }
    .order-card {
      background: rgba(255,255,255,0.05); border: 1px solid rgba(255,255,255,0.1);
      border-radius: 16px; padding: 1.5rem; margin-bottom: 1rem;
    }
    .order-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1rem; }
    .order-id { color: #fff; font-weight: 700; font-size: 1.05rem; margin-right: 12px; }
    .order-date { color: #a8b2d1; font-size: 0.85rem; }
    .order-status {
      padding: 6px 14px; border-radius: 20px; font-size: 0.8rem; font-weight: 600;
    }
    .status-beklemede { background: rgba(251,191,36,0.15); color: #fbbf24; }
    .status-hazirlaniyor { background: rgba(96,165,250,0.15); color: #60a5fa; }
    .status-kargoda { background: rgba(168,130,255,0.15); color: #a882ff; }
    .status-teslim { background: rgba(74,222,128,0.15); color: #4ade80; }
    .status-iptal { background: rgba(239,68,68,0.15); color: #ef4444; }

    .order-items { border-top: 1px solid rgba(255,255,255,0.05); padding-top: 1rem; }
    .order-item {
      display: flex; justify-content: space-between; padding: 6px 0; color: #a8b2d1;
    }
    .item-name { flex: 1; }
    .item-qty { width: 60px; text-align: center; }
    .item-price { width: 100px; text-align: right; color: #fff; }
    .order-footer {
      display: flex; justify-content: flex-end; gap: 8px; margin-top: 1rem;
      padding-top: 1rem; border-top: 1px solid rgba(255,255,255,0.05);
    }
    .total-label { color: #a8b2d1; }
    .total-price { color: #4ade80; font-weight: 700; font-size: 1.1rem; }

    .empty-state { text-align: center; padding: 4rem; color: #a8b2d1; font-size: 1.2rem; }
  `]
})
export class OrdersComponent implements OnInit {
  private orderService = inject(OrderService);
  orders: Order[] = [];

  ngOnInit() {
    this.orderService.getMyOrders().subscribe(res => {
      if (res.success) this.orders = res.data;
    });
  }

  getStatusClass(status: string): string {
    const map: Record<string, string> = {
      'Beklemede': 'status-beklemede',
      'Hazırlanıyor': 'status-hazirlaniyor',
      'Kargoda': 'status-kargoda',
      'Teslim Edildi': 'status-teslim',
      'İptal Edildi': 'status-iptal'
    };
    return map[status] || 'status-beklemede';
  }
}
