import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class LoadingService {
  // Spinner'ın durumunu tutan "yayına açık" bir değişken
  private loadingCount = 0;
  isLoading = new BehaviorSubject<boolean>(false);

  show() {
    this.loadingCount++;
    this.isLoading.next(true);
  }

  hide() {
    this.loadingCount--;
    if (this.loadingCount <= 0) {
      this.loadingCount = 0;
      this.isLoading.next(false);
    }
  }
}