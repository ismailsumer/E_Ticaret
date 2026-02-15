import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ServiceResponse, Address } from '../models/models';

@Injectable({ providedIn: 'root' })
export class AddressService {
    private apiUrl = 'http://localhost:5292/api/Addresses';
    private http = inject(HttpClient);

    getAddresses(): Observable<ServiceResponse<Address[]>> {
        return this.http.get<ServiceResponse<Address[]>>(this.apiUrl);
    }

    addAddress(address: { title: string; fullAddress: string; city: string; district: string }): Observable<ServiceResponse<string>> {
        return this.http.post<ServiceResponse<string>>(this.apiUrl, address);
    }
}
