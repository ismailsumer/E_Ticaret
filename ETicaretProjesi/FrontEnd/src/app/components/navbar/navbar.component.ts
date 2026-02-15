import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AsyncPipe } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, AsyncPipe],
  template: `
    <nav class="navbar">
      <div class="nav-container">
        <a routerLink="/" class="nav-brand">
          <span class="brand-icon">🛒</span>
          <span class="brand-text">E-Ticaret</span>
        </a>
        <button class="mobile-toggle" (click)="mobileOpen = !mobileOpen">☰</button>
        <div class="nav-links" [class.open]="mobileOpen">
          <a routerLink="/products" routerLinkActive="active" class="nav-link" (click)="mobileOpen = false">Ürünler</a>
          @if (authService.isLoggedIn$ | async) {
            <a routerLink="/basket" routerLinkActive="active" class="nav-link nav-cart" (click)="mobileOpen = false">
              <span>🛍️</span> Sepetim
            </a>
            <a routerLink="/profile" routerLinkActive="active" class="nav-link" (click)="mobileOpen = false">👤 Profilim</a>
            <div class="nav-user">
              <span class="username">{{ authService.getUsername() }}</span>
              <button class="btn-logout" (click)="authService.logout(); mobileOpen = false">Çıkış</button>
            </div>
          } @else {
            <a routerLink="/login" routerLinkActive="active" class="nav-link btn-login" (click)="mobileOpen = false">Giriş Yap</a>
            <a routerLink="/register" routerLinkActive="active" class="nav-link btn-register" (click)="mobileOpen = false">Kayıt Ol</a>
          }
        </div>
      </div>
    </nav>
  `,
  styles: [`
    .navbar {
      background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
      padding: 0 2rem;
      position: sticky; top: 0; z-index: 1000;
      box-shadow: 0 2px 20px rgba(0,0,0,0.3);
    }
    .nav-container {
      max-width: 1280px; margin: 0 auto;
      display: flex; align-items: center; justify-content: space-between;
      height: 64px;
    }
    .nav-brand {
      display: flex; align-items: center; gap: 8px;
      text-decoration: none; color: #fff; font-size: 1.4rem; font-weight: 700;
    }
    .brand-icon { font-size: 1.6rem; }
    .brand-text {
      background: linear-gradient(135deg, #e94560, #0f3460);
      -webkit-background-clip: text; -webkit-text-fill-color: transparent;
      background-clip: text;
    }
    .mobile-toggle {
      display: none; background: none; border: none; color: #fff;
      font-size: 1.5rem; cursor: pointer;
    }
    .nav-links {
      display: flex; align-items: center; gap: 8px;
    }
    .nav-link {
      color: #a8b2d1; text-decoration: none; padding: 8px 16px;
      border-radius: 8px; font-size: 0.9rem; font-weight: 500;
      transition: all 0.3s ease;
    }
    .nav-link:hover, .nav-link.active {
      color: #fff; background: rgba(233,69,96,0.15);
    }
    .nav-cart { position: relative; }
    .nav-user {
      display: flex; align-items: center; gap: 12px;
      margin-left: 8px; padding-left: 16px;
      border-left: 1px solid rgba(255,255,255,0.1);
    }
    .username { color: #e94560; font-weight: 600; font-size: 0.9rem; }
    .btn-logout {
      background: transparent; border: 1px solid #e94560; color: #e94560;
      padding: 6px 16px; border-radius: 8px; cursor: pointer;
      font-size: 0.85rem; transition: all 0.3s;
    }
    .btn-logout:hover { background: #e94560; color: #fff; }
    .btn-login { border: 1px solid #e94560; color: #e94560 !important; }
    .btn-register { background: linear-gradient(135deg, #e94560, #c23152) !important; color: #fff !important; }

    @media (max-width: 768px) {
      .mobile-toggle { display: block; }
      .nav-links {
        display: none; flex-direction: column; position: absolute;
        top: 64px; left: 0; right: 0; background: #16213e;
        padding: 1rem; gap: 4px; border-bottom: 1px solid rgba(255,255,255,0.1);
      }
      .nav-links.open { display: flex; }
      .nav-link { width: 100%; text-align: center; padding: 12px; }
      .nav-user { border-left: none; padding-left: 0; margin-left: 0; justify-content: center; padding-top: 8px; border-top: 1px solid rgba(255,255,255,0.1); }
    }
  `]
})
export class NavbarComponent {
  authService = inject(AuthService);
  mobileOpen = false;
}
