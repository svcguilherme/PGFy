import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'horas', standalone: true })
export class HorasPipe implements PipeTransform {
  transform(horas: number): string {
    const h = Math.floor(horas);
    const min = Math.round((horas - h) * 60);
    if (min === 0) return `${h}h`;
    if (h === 0) return `${min}min`;
    return `${h}h ${min}min`;
  }
}
