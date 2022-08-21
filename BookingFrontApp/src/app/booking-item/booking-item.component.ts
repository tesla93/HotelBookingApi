import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { Booking } from '../booking-component/booking.model';

@Component({
  selector: 'app-booking-item',
  templateUrl: './booking-item.component.html',
  styleUrls: ['./booking-item.component.css'],
  changeDetection:ChangeDetectionStrategy.OnPush
})
export class BookingItemComponent implements OnInit {

  @Input() bookingItem :Booking;
  defaultValue="<Empty>"

  ngOnInit(): void {
  }

}
