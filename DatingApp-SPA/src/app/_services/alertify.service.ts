import { Injectable } from '@angular/core';
import * as alertify from 'alertifyjs';

@Injectable({
  providedIn: 'root',
})

// We use alertify to create dynamic notifications alerting the user on succesful or unsuccesful requests
export class AlertifyService {
  constructor() {}

  confirm(message: string, okCallBack: () => any) {
    alertify.confirm(message, (e: any) => {
      if (e) {
        okCallBack();
      }
      else {}
    });
  }

  success(message: any) {
    alertify.success(message);
  }

  error(message: any) {
    alertify.error(message);
  }
  warning(message: any) {
    alertify.warning(message);
  }
  message(message: any) {
    alertify.message(message);
  }
}
