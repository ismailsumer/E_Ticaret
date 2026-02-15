import { Component } from '@angular/core';

@Component({
    selector: 'app-footer',
    standalone: true,
    template: `
    <footer class="footer">
      <div class="footer-container">
        <div class="footer-grid">
          <div class="footer-col">
            <h3><span>🛒</span> E-Ticaret</h3>
            <p>Kaliteli ürünler, hızlı teslimat. Türkiye'nin güvenilir online alışveriş platformu.</p>
          </div>
          <div class="footer-col">
            <h4>Hızlı Linkler</h4>
            <ul>
              <li><a href="/products">Ürünler</a></li>
              <li><a href="/basket">Sepetim</a></li>
              <li><a href="/orders">Siparişlerim</a></li>
            </ul>
          </div>
          <div class="footer-col">
            <h4>İletişim</h4>
            <ul>
              <li>📧 info&#64;eticaret.com</li>
              <li>📞 +90 555 123 4567</li>
              <li>📍 İstanbul, Türkiye</li>
            </ul>
          </div>
        </div>
        <div class="footer-bottom">
          <p>© 2026 E-Ticaret. Tüm hakları saklıdır.</p>
        </div>
      </div>
    </footer>
  `,
    styles: [`
    .footer {
      background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
      color: #a8b2d1; padding: 3rem 2rem 1.5rem;
      margin-top: auto;
    }
    .footer-container { max-width: 1280px; margin: 0 auto; }
    .footer-grid { display: grid; grid-template-columns: 2fr 1fr 1fr; gap: 2rem; }
    .footer-col h3 { color: #fff; font-size: 1.3rem; margin-bottom: 1rem; }
    .footer-col h4 { color: #fff; font-size: 1rem; margin-bottom: 1rem; }
    .footer-col p { font-size: 0.9rem; line-height: 1.6; }
    .footer-col ul { list-style: none; padding: 0; }
    .footer-col li { margin-bottom: 8px; font-size: 0.9rem; }
    .footer-col a { color: #a8b2d1; text-decoration: none; transition: color 0.3s; }
    .footer-col a:hover { color: #e94560; }
    .footer-bottom {
      margin-top: 2rem; padding-top: 1.5rem;
      border-top: 1px solid rgba(255,255,255,0.1);
      text-align: center; font-size: 0.85rem;
    }
    @media (max-width: 768px) {
      .footer-grid { grid-template-columns: 1fr; }
    }
  `]
})
export class FooterComponent { }
