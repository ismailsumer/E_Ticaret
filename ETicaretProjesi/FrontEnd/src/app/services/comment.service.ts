import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ServiceResponse, Comment } from '../models/models';

@Injectable({ providedIn: 'root' })
export class CommentService {
    private apiUrl = 'http://localhost:5292/api/Comments';
    private http = inject(HttpClient);

    getProductComments(productId: number): Observable<ServiceResponse<Comment[]>> {
        return this.http.get<ServiceResponse<Comment[]>>(`${this.apiUrl}/product/${productId}`);
    }

    addComment(productId: number, content: string, rating: number): Observable<ServiceResponse<boolean>> {
        return this.http.post<ServiceResponse<boolean>>(this.apiUrl, { productId, content, rating });
    }
}
