import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-not-found',
    standalone: true,
    imports: [RouterLink],
    template: `
    <div class="error-page">
      <div class="error-content">
        <div class="error-icon">🔍</div>
        <h1>404</h1>
        <h2>Sayfa Bulunamadı</h2>
        <p>Aradığınız sayfa mevcut değil veya taşınmış olabilir.</p>
        <a routerLink="/" class="btn-home">🏠 Ana Sayfaya Dön</a>
      </div>
    </div>
  `,
    styles: [`
    .error-page {
      display: flex; justify-content: center; align-items: center;
      min-height: calc(100vh - 200px); padding: 2rem;
    }
    .error-content { text-align: center; max-width: 500px; }
    .error-icon { font-size: 5rem; margin-bottom: 1rem; }
    h1 {
      font-size: 6rem; font-weight: 800; margin: 0;
      background: linear-gradient(135deg, #e94560, #c23152);
      -webkit-background-clip: text; -webkit-text-fill-color: transparent; background-clip: text;
    }
    h2 { color: #fff; font-size: 1.5rem; margin: 0.5rem 0 1rem; }
    p { color: #a8b2d1; font-size: 1rem; margin-bottom: 2rem; line-height: 1.6; }
    .btn-home {
      display: inline-block; background: linear-gradient(135deg, #e94560, #c23152);
      color: #fff; padding: 14px 32px; border-radius: 12px;
      text-decoration: none; font-weight: 600; font-size: 1rem;
      transition: all 0.3s;
    }
    .btn-home:hover { transform: translateY(-2px); box-shadow: 0 8px 25px rgba(233,69,96,0.4); }
  `]
})
export class NotFoundComponent { }
