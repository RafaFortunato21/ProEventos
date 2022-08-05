/* tslint:disable:no-unused-variable */

import localeBrasil from '@angular/common/locales/br';
import { DateTimeFormatPipe } from './DateTimeFormat.pipe';

describe('Pipe: DateTimeFormatPipe', () => {
  it('create an instance', () => {
    let pipe = new DateTimeFormatPipe('pt-BR');
    expect(pipe).toBeTruthy();
    
  });
});
