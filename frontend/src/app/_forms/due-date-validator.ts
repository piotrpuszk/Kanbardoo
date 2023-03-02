import { Injectable } from "@angular/core";
import { FormControl } from "@angular/forms";

@Injectable()
export class DueDateValidator {
  constructor() {}

  static todayOrAfter(control: FormControl) {
    if(!control.value) return null;

    const selectedDate = new Date(control.value);
    selectedDate.setHours(0,0,0,0);

    const today = new Date();
    today.setHours(0,0,0,0);

    if(selectedDate.getTime() < today.getTime()) {
      return { todayOrAfter: true}
    }

    return null;
  }
}