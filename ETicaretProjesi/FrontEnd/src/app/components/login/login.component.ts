import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  template: `
    <div class="auth-page">
      <div class="auth-card">
        <h2>🔐 Giriş Yap</h2>
        <p class="subtitle">Hesabınıza giriş yapın</p>
        <div class="form-group">
          <label>Kullanıcı Adı</label>
          <input type="text" [(ngModel)]="username" placeholder="Kullanıcı adınız" class="form-input" />
        </div>
        <div class="form-group">
          <label>Şifre</label>
          <input type="password" [(ngModel)]="password" placeholder="Şifreniz" class="form-input" (keyup.enter)="login()" />
        </div>
        <button class="btn-submit" (click)="login()" [disabled]="loading">
          {{ loading ? 'Giriş yapılıyor...' : 'Giriş Yap' }}
        </button>
        @if (message) {
          <div class="alert" [class.success]="success" [class.error]="!success">{{ message }}</div>
        }
        <p class="form-footer">Hesabınız yok mu? <a routerLink="/register">Kayıt Ol</a></p>
      </div>
    </div>
  `,
  styles: [`
    .auth-page {
      display: flex; justify-content: center; align-items: center; min-height: calc(100vh - 200px); padding: 2rem;
    }
    .auth-card {
      background: rgba(255,255,255,0.05); border: 1px solid rgba(255,255,255,0.1);
      border-radius: 20px; padding: 2.5rem; width: 100%; max-width: 420px;
    }
    h2 { color: #fff; text-align: center; font-size: 1.5rem; margin-bottom: 4px; }
    .subtitle { color: #a8b2d1; text-align: center; font-size: 0.9rem; margin-bottom: 1.5rem; }
    .form-group { margin-bottom: 1rem; }
    label { display: block; color: #a8b2d1; font-size: 0.85rem; margin-bottom: 4px; font-weight: 500; }
    .form-input {
      width: 100%; padding: 12px 16px; border: 1px solid rgba(255,255,255,0.1);
      border-radius: 10px; background: rgba(255,255,255,0.05); color: #fff;
      font-size: 0.95rem; outline: none; box-sizing: border-box;
    }
    .form-input:focus { border-color: #e94560; }
    .btn-submit {
      width: 100%; padding: 14px; border: none; border-radius: 12px;
      background: linear-gradient(135deg, #e94560, #c23152); color: #fff;
      font-size: 1rem; font-weight: 600; cursor: pointer; margin-top: 1rem; transition: all 0.3s;
    }
    .btn-submit:hover { transform: translateY(-2px); box-shadow: 0 8px 25px rgba(233,69,96,0.4); }
    .btn-submit:disabled { opacity: 0.6; cursor: not-allowed; }
    .alert { margin-top: 1rem; padding: 10px; border-radius: 8px; text-align: center; font-size: 0.9rem; }
    .alert.success { background: rgba(74,222,128,0.15); color: #4ade80; }
    .alert.error { background: rgba(239,68,68,0.15); color: #ef4444; }
    .form-footer { text-align: center; color: #a8b2d1; font-size: 0.9rem; margin-top: 1rem; }
    .form-footer a { color: #e94560; text-decoration: none; font-weight: 600; }
  `]
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  username = '';
  password = '';
  message = '';
  success = false;
  loading = false;

  login() {
    if (!this.username || !this.password) {
      this.message = 'Lütfen tüm alanları doldurun.';
      this.success = false;
      return;
    }
    this.loading = true;
    this.authService.login(this.username, this.password).subscribe({
      next: res => {
        this.loading = false;
        if (res.success) {
          this.message = 'Giriş başarılı! Yönlendiriliyorsunuz...';
          this.success = true;
          setTimeout(() => this.router.navigate(['/']), 1000);
        } else {
          this.message = res.message;
          this.success = false;
        }
      },
      error: () => { this.loading = false; this.message = 'Giriş başarısız.'; this.success = false; }
    });
  }
}
