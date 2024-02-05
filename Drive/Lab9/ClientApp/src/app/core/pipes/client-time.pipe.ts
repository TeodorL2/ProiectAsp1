import { Pipe, PipeTransform, Inject, LOCALE_ID } from '@angular/core';

@Pipe({
  name: 'clientTime',
})
export class ClientTimePipe implements PipeTransform {
  constructor(@Inject(LOCALE_ID) private locale: string) { }

  transform(value: Date | string): string {
    const date = typeof value === 'string' ? new Date(value) : value;

    if (isNaN(date.getTime())) {
      return 'Invalid Date';
    }

    const timeString = new Intl.DateTimeFormat(this.locale, {
      hour: 'numeric',
      minute: 'numeric',
      second: 'numeric',
      timeZoneName: 'short',
    }).format(date);

    return timeString;
  }
}
