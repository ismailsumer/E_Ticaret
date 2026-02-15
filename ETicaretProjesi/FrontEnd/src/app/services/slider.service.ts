import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ServiceResponse, Slider } from '../models/models';

@Injectable({ providedIn: 'root' })
export class SliderService {
    private apiUrl = 'http://localhost:5292/api/Sliders';
    private http = inject(HttpClient);

    getSliders(): Observable<ServiceResponse<Slider[]>> {
        return this.http.get<ServiceResponse<Slider[]>>(this.apiUrl);
    }
}
