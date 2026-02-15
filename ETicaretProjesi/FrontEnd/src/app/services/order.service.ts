import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ServiceResponse, Order } from '../models/models';

@Injectable({ providedIn: 'root' })
export class OrderService {
    private apiUrl = 'http://localhost:5292/api/Orders';
    private http = inject(HttpClient);

    checkout(): Observable<ServiceResponse<number>> {
        return this.http.post<ServiceResponse<number>>(`${this.apiUrl}/checkout`, {});
    }

    getMyOrders(): Observable<ServiceResponse<Order[]>> {
        return this.http.get<ServiceResponse<Order[]>>(this.apiUrl);
    }
}
