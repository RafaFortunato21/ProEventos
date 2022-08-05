import { DatePipe, formatDate } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import { Constants } from '../util/constants';

@Pipe({
  name: 'DateTimeFormat'
})
export class DateTimeFormatPipe extends DatePipe implements PipeTransform {

  transform(value: any, args?: any): any {



    var dateCreated = super.transform (value,  'yyyy-MM-dd hh:mm:ss');


    return super.transform (dateCreated, Constants.DATE_TIME_FMT);



  }

}
