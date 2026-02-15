import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { loginGuard } from './guards/login.guard';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./modules/product/home/home.component').then(m => m.HomeComponent)
    },
    {
        path: 'login',
        canActivate: [loginGuard],
        loadComponent: () => import('./components/login/login.component').then(m => m.LoginComponent)
    },
    {
        path: 'register',
        canActivate: [loginGuard],
        loadComponent: () => import('./components/register/register.component').then(m => m.RegisterComponent)
    },
    {
        path: 'products',
        loadComponent: () => import('./modules/product/product-list/product-list.component').then(m => m.ProductListComponent)
    },
    {
        path: 'products/:id',
        loadComponent: () => import('./modules/product/product-detail/product-detail.component').then(m => m.ProductDetailComponent)
    },
    {
        path: 'basket',
        canActivate: [authGuard],
        loadComponent: () => import('./components/basket/basket.component').then(m => m.BasketComponent)
    },
    {
        path: 'checkout',
        canActivate: [authGuard],
        loadComponent: () => import('./components/checkout/checkout.component').then(m => m.CheckoutComponent)
    },
    {
        path: 'profile',
        canActivate: [authGuard],
        loadComponent: () => import('./components/profile/profile.component').then(m => m.ProfileComponent)
    },
    {
        path: 'orders',
        canActivate: [authGuard],
        loadComponent: () => import('./components/orders/orders.component').then(m => m.OrdersComponent)
    },
    {
        path: '404',
        loadComponent: () => import('./components/not-found/not-found.component').then(m => m.NotFoundComponent)
    },
    {
        path: '500',
        loadComponent: () => import('./components/server-error/server-error.component').then(m => m.ServerErrorComponent)
    },
    {
        path: '**',
        loadComponent: () => import('./components/not-found/not-found.component').then(m => m.NotFoundComponent)
    }
];
