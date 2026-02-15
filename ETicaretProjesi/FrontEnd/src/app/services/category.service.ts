import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Category, ServiceResponse } from '../models/models';

@Injectable({ providedIn: 'root' })
export class CategoryService {
    private apiUrl = 'http://localhost:5292/api/Categories';
    private http = inject(HttpClient);

    getCategories(): Observable<ServiceResponse<Category[]>> {
        return this.http.get<ServiceResponse<Category[]>>(this.apiUrl);
    }
}
