import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { LoadingService } from '../services/loading.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);
  
  // İstek başladığında spinner'ı göster
  loadingService.show();

  return next(req).pipe(
    // İstek bittiğinde (başarılı veya başarısız) spinner'ı gizle
    finalize(() => loadingService.hide())
  );
};