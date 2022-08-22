import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Booking } from '../booking-component/booking.model';
import { BookingService } from '../booking-component/booking.service';

@Component({
  selector: 'app-booking-item',
  templateUrl: './booking-item.component.html',
  styleUrls: ['./booking-item.component.css'],
  changeDetection:ChangeDetectionStrategy.OnPush
})
export class BookingItemComponent implements OnInit {

  @Input() bookingItem :Booking;
  @Output() updateEvent= new EventEmitter<any>();
  @Output() refreshParent= new EventEmitter();
  defaultValue="<Empty>"

  constructor(private bookingService: BookingService)
   { }

  ngOnInit(): void {
  }

  update(){
    this.updateEvent.emit(this.bookingItem);
  }

  delete(){
    this.bookingService.delete(this.bookingItem.id)
    this.refreshParent.emit();
  }

}
