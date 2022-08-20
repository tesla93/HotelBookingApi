import { Component, OnInit } from '@angular/core';
import { CalendarModule } from 'primeng/calendar';
import { Observable } from 'rxjs/internal/Observable';
import { HttpError } from '../shared/models/httpErrorModel';
import { Booking } from './booking.model';
import { BookingService } from './booking.service';

@Component({
  selector: 'app-booking-component',
  templateUrl: './booking-component.component.html',
  styleUrls: ['./booking-component.component.css']
})
export class BookingComponent implements OnInit {

  dateBooking: Date = new Date();
  rangeDates: Date[];
  minDateValue: Date = new Date();
  maxDateValue: Date = new Date();
  disabledDates: Date[];
  availableDates: Date[];

  constructor(private bookingService: BookingService) { }

  async ngOnInit() {
    this.minDateValue = new Date(this.minDateValue.setDate(this.minDateValue.getDate() + 1));
    this.maxDateValue = new Date(this.maxDateValue.setDate(this.maxDateValue.getDate() + 30));
    let allDates = this.dateRange(this.minDateValue, this.maxDateValue).map(x => x.toLocaleDateString("en-US"));
    this.bookingService.getAvailableDates().subscribe((result: Date[]) => {
      this.availableDates=result.map(x=>new Date(x));
      this.disabledDates = allDates.filter(x => !this.availableDates.map(x => x.toLocaleDateString("en-US")).includes(x)).map(x=> new Date(x));     
    }, (error:HttpError) => console.log((error.message)))

    
  }

  dateRange(startDate, endDate) {
    const dateArray = [];
    let currentDate = new Date(startDate);

    while (currentDate <= new Date(endDate)) {
      dateArray.push(new Date(currentDate));
      currentDate.setUTCDate(currentDate.getUTCDate() + 1);
    }
    return dateArray;
  }

}
