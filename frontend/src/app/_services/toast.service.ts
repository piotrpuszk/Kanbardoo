import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  constructor(private toastrService: ToastrService) {}

  public error(message?: string, title?: string) {
    this.toastrService.error(message, title);
  }
}
