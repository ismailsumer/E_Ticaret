import { isPlatformBrowser } from '@angular/common';
import { HttpInterceptorFn } from '@angular/common/http';
import { inject, PLATFORM_ID } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';


export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const platformId = inject(PLATFORM_ID);

  return next(req).pipe(
    catchError((error) => {
      // SSR Sorununu önlemek için sadece tarayıcıda çalıştır
      if (isPlatformBrowser(platformId)) {
        import('sweetalert2').then((module) => {
          const Swal = module.default;
          // 401: Unauthorized -> Login'e yönlendir
          if (error.status === 401) {
            Swal.fire({
              icon: 'warning',
              title: 'Oturum Süresi Doldu',
              text: 'Lütfen tekrar giriş yapınız.',
              confirmButtonText: 'Giriş Yap'
            }).then(() => {
              localStorage.removeItem('token'); // Token'ı temizle
              router.navigate(['/login']);
            });
          }
          // 403: Forbidden -> Yetki Yok
          else if (error.status === 403) {
            Swal.fire({
              icon: 'error',
              title: 'Yetkisiz Erişim',
              text: 'Bu işlemi yapmaya yetkiniz yok.',
              confirmButtonText: 'Tamam'
            });
          }
          // 400: Bad Request -> Validation Hataları
          else if (error.status === 400) {
            // Backend'den { validationErrors: string[] } veya "Hata mesajı string" dönebilir
            const message = error.error?.message || error.error?.validationErrors?.join('<br>') || 'Geçersiz istek.';
            Swal.fire({
              icon: 'warning',
              title: 'Hata',
              html: message,
              confirmButtonText: 'Tamam'
            });
          }
          // 500: Server Error
          else if (error.status === 500) {
            Swal.fire({
              icon: 'error',
              title: 'Sunucu Hatası',
              text: 'Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.',
              confirmButtonText: 'Tamam'
            });
          }
          // Diğer hatalar
          else {
            Swal.fire({
              icon: 'error',
              title: 'Hata',
              text: error.message || 'Bilinmeyen bir hata.',
              confirmButtonText: 'Tamam'
            });
          }
        });
      }

      return throwError(() => error);
    })
  );
};