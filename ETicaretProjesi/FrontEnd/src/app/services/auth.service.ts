import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { ServiceResponse, LoginResponse } from '../models/models';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private apiUrl = 'http://localhost:5292/api/Auth';
    private http = inject(HttpClient);
    private router = inject(Router);
    private platformId = inject(PLATFORM_ID);

    private isLoggedInSubject = new BehaviorSubject<boolean>(false);
    isLoggedIn$ = this.isLoggedInSubject.asObservable();

    constructor() {
        this.isLoggedInSubject.next(this.hasToken());
    }

    private hasToken(): boolean {
        if (isPlatformBrowser(this.platformId)) {
            return !!localStorage.getItem('token');
        }
        return false;
    }

    login(username: string, password: string): Observable<ServiceResponse<LoginResponse>> {
        return this.http.post<ServiceResponse<LoginResponse>>(`${this.apiUrl}/login`, { username, password })
            .pipe(tap(res => {
                if (res.success && isPlatformBrowser(this.platformId)) {
                    localStorage.setItem('token', res.data.token);
                    this.isLoggedInSubject.next(true);
                }
            }));
    }

    register(username: string, password: string, phoneNumber: string): Observable<ServiceResponse<string>> {
        return this.http.post<ServiceResponse<string>>(`${this.apiUrl}/register`, { username, password, phoneNumber });
    }

    logout(): void {
        if (isPlatformBrowser(this.platformId)) {
            localStorage.removeItem('token');
        }
        this.isLoggedInSubject.next(false);
        this.router.navigate(['/login']);
    }

    getToken(): string | null {
        if (isPlatformBrowser(this.platformId)) {
            return localStorage.getItem('token');
        }
        return null;
    }

    getUserRole(): string {
        const token = this.getToken();
        if (!token) return '';
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || '';
        } catch { return ''; }
    }

    getUsername(): string {
        const token = this.getToken();
        if (!token) return '';
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            return payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || '';
        } catch { return ''; }
    }

    isAdmin(): boolean {
        return this.getUserRole() === 'Admin';
    }

    getUserId(): number {
        const token = this.getToken();
        if (!token) return 0;
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            return parseInt(payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || '0', 10);
        } catch { return 0; }
    }

    changePassword(currentPassword: string, newPassword: string, confirmNewPassword: string): Observable<ServiceResponse<string>> {
        return this.http.put<ServiceResponse<string>>(`${this.apiUrl}/change-password`, {
            currentPassword,
            newPassword,
            confirmNewPassword
        });
    }
}
