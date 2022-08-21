import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from "rxjs/internal/Observable";
import { Booking } from "./booking.model";
import { environment } from "src/environments/environment";
import { catchError, throwError } from "rxjs";
import { HttpError } from "../shared/models/httpErrorModel";

@Injectable({
    providedIn: 'root',
  })
  export class BookingService{
    constructor(private httpClient: HttpClient) {}


    getAvailableDates(): Observable<Date[] | HttpError>{
        console.log(environment.baseUrl)
        return this.httpClient.get<Date[]>(`${environment.baseUrl}booking/available`)
        .pipe(catchError((err) => this.handleHttpError(err)));;
    }

    getAllBookings():Observable<Booking[] | HttpError> {
      return this.httpClient.get<Booking[]>(`${environment.baseUrl}booking`)
      .pipe(catchError((err) => this.handleHttpError(err)));
    }

    saveBooking(booking: Booking){
      return this.httpClient.post<any>(`${environment.baseUrl}booking/`, booking)
      .subscribe();
    }

 private handleHttpError(
    error:HttpErrorResponse
  ):Observable<HttpError>{
    return throwError(()=>new HttpError(
      error.status,
      error.statusText
    ))};
}