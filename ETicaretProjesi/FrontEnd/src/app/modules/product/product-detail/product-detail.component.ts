import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../../services/product.service';
import { BasketService } from '../../../services/basket.service';
import { CommentService } from '../../../services/comment.service';
import { AuthService } from '../../../services/auth.service';
import { Product, Comment } from '../../../models/models';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [RouterLink, FormsModule, CommonModule],
  template: `
    <div class="page">
      <div class="container">
        @if (product()) {
          <div class="product-detail">
            <div class="product-image-section">
              @if (product()!.imageUrl) {
                <img [src]="'http://localhost:5292' + product()!.imageUrl" [alt]="product()!.name" class="main-image" />
              } @else {
                <div class="placeholder-image">📷</div>
              }
            </div>
            <div class="product-info-section">
              <span class="breadcrumb">
                <a routerLink="/products">Ürünler</a> → {{ product()!.categoryName }}
              </span>
              <h1>{{ product()!.name }}</h1>
              <div class="meta-badges">
                <span class="badge brand">{{ product()!.brandName }}</span>
                <span class="badge stock" [class.out]="product()!.stock === 0">
                  {{ product()!.stock > 0 ? product()!.stock + ' adet stokta' : 'Tükendi' }}
                </span>
              </div>
              <p class="description">{{ product()!.description }}</p>
              <div class="price-section">
                <span class="price">₺{{ product()!.price.toFixed(2) }}</span>
              </div>
              <div class="actions">
                @if (product()!.stock > 0) {
                  <button class="btn-add-cart" (click)="addToBasket()">🛒 Sepete Ekle</button>
                } @else {
                  <button class="btn-add-cart disabled" disabled>Stokta Yok</button>
                }
                <a routerLink="/products" class="btn-back">← Geri Dön</a>
              </div>
              @if (message()) {
                <div class="alert" [class.success]="messageType() === 'success'" [class.error]="messageType() === 'error'">
                  {{ message() }}
                </div>
              }
            </div>
          </div>

          <!-- Comments Section -->
          <div class="comments-section">
            <h2 class="comments-title">💬 Yorumlar ({{ comments().length }})</h2>

            <!-- Add Comment Form -->
            @if (authService.getToken()) {
              <div class="comment-form">
                <div class="rating-input">
                  <span class="rating-label">Puanınız:</span>
                  @for (star of [1,2,3,4,5]; track star) {
                    <button class="star-btn" [class.filled]="star <= newRating()" (click)="newRating.set(star)">★</button>
                  }
                </div>
                <div class="comment-input-row">
                  <input type="text" [ngModel]="newComment()" (ngModelChange)="newComment.set($event)" placeholder="Yorumunuzu yazın..." class="comment-input" (keyup.enter)="submitComment()" />
                  <button class="btn-comment" (click)="submitComment()" [disabled]="!newComment().trim()">Gönder</button>
                </div>
              </div>
            } @else {
              <p class="login-hint">Yorum yapmak için <a routerLink="/login">giriş yapın</a>.</p>
            }

            <!-- Comments List -->
            @if (comments().length > 0) {
              <div class="comments-list">
                @for (comment of comments(); track comment.id) {
                  <div class="comment-card">
                    <div class="comment-header">
                      <div class="comment-user">
                        <span class="user-avatar">{{ comment.username.charAt(0).toUpperCase() }}</span>
                        <span class="user-name">{{ comment.username }}</span>
                      </div>
                      <div class="comment-meta">
                        <span class="comment-stars">
                          @for (s of [1,2,3,4,5]; track s) {
                            <span [class.filled]="s <= comment.rating">★</span>
                          }
                        </span>
                        <span class="comment-date">{{ comment.createdDate | slice:0:10 }}</span>
                      </div>
                    </div>
                    <p class="comment-text">{{ comment.content }}</p>
                  </div>
                }
              </div>
            } @else {
              <p class="no-comments">Henüz yorum yapılmamış. İlk yorumu siz yapın!</p>
            }
          </div>
        } @else {
          <div class="loading"><p>Yükleniyor...</p></div>
        }
      </div>
    </div>
  `,
  styles: [`
    .page { padding: 2rem; }
    .container { max-width: 1280px; margin: 0 auto; }
    .product-detail { display: grid; grid-template-columns: 1fr 1fr; gap: 3rem; align-items: start; }
    .product-image-section { position: sticky; top: 80px; }
    .main-image {
      width: 100%; border-radius: 20px; object-fit: cover;
      border: 1px solid rgba(255,255,255,0.1); max-height: 500px;
    }
    .placeholder-image {
      width: 100%; height: 400px; border-radius: 20px;
      background: #1a1a2e; display: flex; align-items: center; justify-content: center;
      font-size: 5rem; border: 1px solid rgba(255,255,255,0.1);
    }
    .breadcrumb { color: #a8b2d1; font-size: 0.85rem; }
    .breadcrumb a { color: #e94560; text-decoration: none; }
    h1 { color: #fff; font-size: 2rem; font-weight: 700; margin: 12px 0; }
    .meta-badges { display: flex; gap: 8px; margin-bottom: 1rem; flex-wrap: wrap; }
    .badge { padding: 6px 14px; border-radius: 20px; font-size: 0.8rem; font-weight: 600; }
    .badge.brand { background: rgba(15,52,96,0.3); color: #60a5fa; border: 1px solid rgba(96,165,250,0.2); }
    .badge.stock { background: rgba(74,222,128,0.15); color: #4ade80; }
    .badge.stock.out { background: rgba(239,68,68,0.15); color: #ef4444; }
    .description { color: #a8b2d1; line-height: 1.8; font-size: 1rem; margin-bottom: 1.5rem; }
    .price-section { margin-bottom: 1.5rem; }
    .price { color: #4ade80; font-size: 2.2rem; font-weight: 800; }
    .actions { display: flex; gap: 12px; flex-wrap: wrap; }
    .btn-add-cart {
      flex: 1; padding: 14px 28px; border: none; border-radius: 12px;
      background: linear-gradient(135deg, #e94560, #c23152); color: #fff;
      font-size: 1.1rem; font-weight: 600; cursor: pointer; transition: all 0.3s;
    }
    .btn-add-cart:hover { transform: translateY(-2px); box-shadow: 0 8px 25px rgba(233,69,96,0.4); }
    .btn-add-cart.disabled { opacity: 0.5; cursor: not-allowed; background: #555; }
    .btn-back {
      padding: 14px 28px; border: 1px solid rgba(255,255,255,0.2); border-radius: 12px;
      color: #a8b2d1; text-decoration: none; transition: all 0.3s; text-align: center;
    }
    .btn-back:hover { border-color: #e94560; color: #e94560; }
    .alert { margin-top: 1rem; padding: 12px 16px; border-radius: 10px; font-size: 0.9rem; }
    .alert.success { background: rgba(74,222,128,0.15); color: #4ade80; border: 1px solid rgba(74,222,128,0.2); }
    .alert.error { background: rgba(239,68,68,0.15); color: #ef4444; border: 1px solid rgba(239,68,68,0.2); }
    .loading { text-align: center; padding: 4rem; color: #a8b2d1; }

    /* Comments */
    .comments-section { margin-top: 3rem; padding-top: 2rem; border-top: 1px solid rgba(255,255,255,0.1); }
    .comments-title { color: #fff; font-size: 1.4rem; font-weight: 700; margin-bottom: 1.5rem; }

    .comment-form {
      background: rgba(255,255,255,0.05); border: 1px solid rgba(255,255,255,0.1);
      border-radius: 16px; padding: 1.5rem; margin-bottom: 2rem;
    }
    .rating-input { display: flex; align-items: center; gap: 4px; margin-bottom: 1rem; }
    .rating-label { color: #a8b2d1; font-size: 0.9rem; margin-right: 8px; }
    .star-btn {
      background: none; border: none; font-size: 1.5rem; color: #555;
      cursor: pointer; transition: color 0.2s; padding: 0 2px;
    }
    .star-btn.filled { color: #fbbf24; }
    .comment-input-row { display: flex; gap: 10px; }
    .comment-input {
      flex: 1; padding: 12px 16px; border: 1px solid rgba(255,255,255,0.1);
      border-radius: 10px; background: rgba(255,255,255,0.05); color: #fff;
      font-size: 0.95rem; outline: none;
    }
    .comment-input:focus { border-color: #e94560; }
    .btn-comment {
      padding: 12px 24px; border: none; border-radius: 10px;
      background: #e94560; color: #fff; font-weight: 600; cursor: pointer;
      transition: all 0.3s;
    }
    .btn-comment:hover:not(:disabled) { background: #c23152; }
    .btn-comment:disabled { opacity: 0.5; cursor: not-allowed; }
    .login-hint { color: #a8b2d1; margin-bottom: 1.5rem; }
    .login-hint a { color: #e94560; text-decoration: none; font-weight: 600; }

    .comments-list { display: flex; flex-direction: column; gap: 1rem; }
    .comment-card {
      background: rgba(255,255,255,0.03); border: 1px solid rgba(255,255,255,0.06);
      border-radius: 12px; padding: 1rem 1.25rem;
    }
    .comment-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 8px; flex-wrap: wrap; gap: 8px; }
    .comment-user { display: flex; align-items: center; gap: 8px; }
    .user-avatar {
      width: 32px; height: 32px; border-radius: 50%;
      background: linear-gradient(135deg, #e94560, #0f3460);
      display: flex; align-items: center; justify-content: center;
      color: #fff; font-size: 0.8rem; font-weight: 700;
    }
    .user-name { color: #fff; font-weight: 600; font-size: 0.9rem; }
    .comment-meta { display: flex; align-items: center; gap: 12px; }
    .comment-stars { color: #555; font-size: 0.9rem; }
    .comment-stars .filled { color: #fbbf24; }
    .comment-date { color: #6b7280; font-size: 0.8rem; }
    .comment-text { color: #a8b2d1; font-size: 0.9rem; margin: 0; line-height: 1.6; }
    .no-comments { color: #6b7280; text-align: center; padding: 2rem; }

    @media (max-width: 768px) {
      .product-detail { grid-template-columns: 1fr; }
      .product-image-section { position: static; }
      .comment-input-row { flex-direction: column; }
    }
  `]
})
export class ProductDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private productService = inject(ProductService);
  private basketService = inject(BasketService);
  private commentService = inject(CommentService);
  authService = inject(AuthService);

  product = signal<Product | null>(null);
  comments = signal<Comment[]>([]);
  message = signal('');
  messageType = signal<'success' | 'error'>('success');
  newComment = signal('');
  newRating = signal(5);

  ngOnInit() {
    const id = +this.route.snapshot.paramMap.get('id')!;
    this.productService.getProductById(id).subscribe(res => {
      if (res.success) this.product.set(res.data);
    });
    this.loadComments(id);
  }

  loadComments(productId: number) {
    this.commentService.getProductComments(productId).subscribe(res => {
      if (res.success) this.comments.set(res.data);
    });
  }

  addToBasket() {
    if (!this.authService.getToken()) {
      this.message.set('Sepete eklemek için giriş yapmalısınız.');
      this.messageType.set('error');
      return;
    }
    this.basketService.addToBasket(this.product()!.id, 1).subscribe({
      next: () => { this.message.set('Ürün sepete eklendi! 🎉'); this.messageType.set('success'); },
      error: () => { this.message.set('Bir hata oluştu.'); this.messageType.set('error'); }
    });
  }

  submitComment() {
    if (!this.newComment().trim()) return;
    this.commentService.addComment(this.product()!.id, this.newComment(), this.newRating()).subscribe({
      next: () => {
        this.newComment.set('');
        this.newRating.set(5);
        this.loadComments(this.product()!.id);
      },
      error: () => { this.message.set('Yorum eklenemedi.'); this.messageType.set('error'); }
    });
  }
}
