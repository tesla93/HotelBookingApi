import { DOCUMENT } from '@angular/common';
import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, NgForm } from '@angular/forms';
import { CalendarModule } from 'primeng/calendar';
import { Observable } from 'rxjs/internal/Observable';
import { take } from 'rxjs/operators';
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
  display=false;
  dialogText:string;
  bookings: Booking[];
  displayStyle = "none";
  @ViewChild('bookingForm') bookingForm: NgForm
  bookingDetails: Booking=new Booking();

  constructor(private bookingService: BookingService,
              @Inject(DOCUMENT) private document:Document,
    ) { }

  async ngOnInit() {
    this.minDateValue = new Date(this.minDateValue.setDate(this.minDateValue.getDate() + 1));
    this.maxDateValue = new Date(this.maxDateValue.setDate(this.maxDateValue.getDate() + 30));
    this.getDisabledDates();
    this.getBookings();
  }


  getBookings(){
    this.bookingService
      .getAllBookings()
      .pipe(take(1))
      .subscribe((res: any) => {
        if (res?.length) {
          this.bookings = [...res];
        } else {
          this.bookings = [];
        }
      }, (error:HttpError) => console.log((error.message)));
  }

  getDisabledDates() {
    let allDates = this.dateRange(this.minDateValue, this.maxDateValue).map(x => x.toLocaleDateString("en-US"));
    this.bookingService.getAvailableDates().subscribe((result: Date[]) => {
      this.availableDates = result.map(x => new Date(x));
      this.disabledDates = allDates.filter(x => !this.availableDates.map(x => x.toLocaleDateString("en-US")).includes(x)).map(x => new Date(x));
    }, (error: HttpError) => console.log((error.message)))
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

  onSaveBooking(){
      this.bookingDetails.checkInDate= this.bookingDetails.dateRange[0]
      this.bookingDetails.checkOutDate= this.bookingDetails.dateRange[1]
      this.bookingService.saveBooking(this.bookingDetails);
      window.location.reload();
      this.bookingForm.reset();   
   
  }

  onScrollTop():void{
    this.document.body.scrollTop = 0;
    this.document.documentElement.scrollTop = 0;
  }

  onAddBooking(){
    this.displayStyle = "block";
  }

  closePopup() {
    this.displayStyle = "none";
  }
  

}
