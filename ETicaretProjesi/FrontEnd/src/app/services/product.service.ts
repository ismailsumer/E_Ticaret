import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product, PagedResponse, ServiceResponse } from '../models/models';

@Injectable({ providedIn: 'root' })
export class ProductService {
    private apiUrl = 'http://localhost:5292/api/Products';
    private http = inject(HttpClient);

    getProducts(page = 1, size = 10, search?: string, categoryId?: number, brandId?: number): Observable<PagedResponse<Product[]>> {
        let params = new HttpParams()
            .set('page', page.toString())
            .set('size', size.toString());
        if (search) params = params.set('search', search);
        if (categoryId) params = params.set('categoryId', categoryId.toString());
        if (brandId) params = params.set('brandId', brandId.toString());
        return this.http.get<PagedResponse<Product[]>>(this.apiUrl, { params });
    }

    getProductById(id: number): Observable<ServiceResponse<Product>> {
        return this.http.get<ServiceResponse<Product>>(`${this.apiUrl}/${id}`);
    }
}