import { DOCUMENT } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit, ViewChild } from '@angular/core';
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
  formTitle: string
  displayStyle = "none";
  @ViewChild('bookingForm') bookingForm: NgForm
  bookingDetails: Booking=new Booking();

  constructor(private bookingService: BookingService,
              private changeDetectorRef: ChangeDetectorRef,
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
      }, (error:HttpError) => console.log(error.message));
  }

  getDisabledDates(actualDates:Date[]=null) {
    let allDates = this.dateRange(this.minDateValue, this.maxDateValue);
    this.bookingService.getAvailableDates().subscribe((result: Date[]) => {
      this.availableDates = result.map(x => new Date(x));
      if(actualDates){
        this.availableDates.push(...actualDates)
      }
      this.disabledDates = allDates.filter(x => !this.availableDates.map(x => x.toLocaleDateString("en-US")).includes(x)).map(x => new Date(x));
    }, (error: HttpError) => console.log((error.message)))
  }

 

  onSaveBooking(){
      this.bookingDetails.checkInDate= this.bookingDetails.dateRange[0]
      this.bookingDetails.checkOutDate= this.bookingDetails.dateRange[1]
      if(this.bookingDetails.id){
        this.bookingService.updateBooking(this.bookingDetails.id, this.bookingDetails)
        .subscribe(_=> {this.refresh()},  (error:HttpError) => alert(`Date range error.`));
      }
      else{
        this.bookingService.addBooking(this.bookingDetails)
        .subscribe(_=> {this.refresh()},  (error:HttpError) => alert(`Date range error`));
        this.bookingForm.reset();
      }
      //this.refresh()
  }

  onAddBooking(){
    this.formTitle="Create Booking";
    this.bookingForm.reset();
    this.getDisabledDates();
    this.displayStyle = "block";
  }

  closeModal() {
    this.displayStyle = "none";
  }

  refresh(){
    console.log("refresh")
    window.location.reload();
    this.changeDetectorRef.detectChanges();
  }
  

  onUpdateItem(item: any){
    this.formTitle="Update Booking"
    this.bookingDetails=item;
    this.bookingDetails.dateRange = [new Date(this.bookingDetails.checkInDate), new Date(this.bookingDetails.checkOutDate)];
    let actualDates=this.dateRange(this.bookingDetails.checkInDate, this.bookingDetails.checkOutDate).map(x => new Date(x));
    this.getDisabledDates(actualDates);
    this.displayStyle = "block";
  }

 

  dateRange(startDate, endDate) {
    const dateArray = [];
    let currentDate = new Date(startDate);

    while (currentDate <= new Date(endDate)) {
      dateArray.push(new Date(currentDate));
      currentDate.setUTCDate(currentDate.getUTCDate() + 1);
    }
    return dateArray.map(x => x.toLocaleDateString("en-US"));
  }

}
