import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ServiceResponse, BasketItem } from '../models/models';

@Injectable({ providedIn: 'root' })
export class BasketService {
    private apiUrl = 'http://localhost:5292/api/Basket';
    private http = inject(HttpClient);

    getBasket(): Observable<ServiceResponse<BasketItem[]>> {
        return this.http.get<ServiceResponse<BasketItem[]>>(this.apiUrl);
    }

    addToBasket(productId: number, quantity: number): Observable<ServiceResponse<string>> {
        return this.http.post<ServiceResponse<string>>(this.apiUrl, { productId, quantity });
    }

    clearBasket(): Observable<ServiceResponse<string>> {
        return this.http.delete<ServiceResponse<string>>(this.apiUrl);
    }
}
