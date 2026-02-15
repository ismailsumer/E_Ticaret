import { inject, PLATFORM_ID } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';

export const authGuard: CanActivateFn = (route, state) => {
    const router = inject(Router);
    const platformId = inject(PLATFORM_ID);

    if (isPlatformBrowser(platformId)) {
        const token = localStorage.getItem('token');
        if (token) {
            return true;
        }
        router.navigate(['/login']);
        return false;
    }

    // Server-side: token is inaccessible, so treat as unauthenticated or allow shell?
    // Since we rely on localStorage, we can't auth on server.
    // We should probably redirect to login or just return false to prevent rendering protected content.
    // However, ensuring client takes over is key.
    // Logic: Server doesn't have token -> invalid -> false.
    // But if we want to allow hydration to handle it?

    // Let's redirect to login on server too (if we can), or just fallback to false.
    // Since this is a simple port, let's just skip usage of localStorage.
    return false;
};
