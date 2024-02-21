import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dateFormatting',
  standalone: true
})
export class DateFormattingPipe implements PipeTransform {

  transform(value: any, ...args: unknown[]): unknown {
    const date: Date = typeof value === 'string' ? new Date(value) : value

    const options: Intl.DateTimeFormatOptions = {
      year: 'numeric',
      month: 'numeric',
      day: 'numeric',
    };

    return date.toLocaleDateString(undefined, options);
  }

}
