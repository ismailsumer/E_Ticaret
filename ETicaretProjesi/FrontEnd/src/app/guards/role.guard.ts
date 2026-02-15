import { inject, PLATFORM_ID } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';

export const roleGuard: CanActivateFn = (route, state) => {
    const router = inject(Router);
    const platformId = inject(PLATFORM_ID);

    if (isPlatformBrowser(platformId)) {
        const token = localStorage.getItem('token');

        if (!token) {
            router.navigate(['/login']);
            return false;
        }

        // Token'ı decode et ve rolü kontrol et
        // Basitçe payload'dan okuyoruz. Gerçek senaryoda jwt-decode kütüphanesi veya AuthService kullanılabilir.
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            const userRole = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || payload['role'];
            const expectedRole = route.data['role'];

            if (userRole === expectedRole || userRole === 'Admin') { // Admin her yere girebilsin
                return true;
            }

            import('sweetalert2').then((module) => {
                const Swal = module.default;
                Swal.fire({
                    icon: 'error',
                    title: 'Yetkisiz Erişim',
                    text: 'Bu sayfaya erişim yetkiniz yok.'
                });
            });

            router.navigate(['/']); // Ana sayfaya at
            return false;
        } catch (error) {
            router.navigate(['/login']);
            return false;
        }
    }

    // Server-side
    return false;
};
