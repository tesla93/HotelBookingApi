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
        return this.httpClient.get<Date[]>(`${environment.baseUrl}booking/available`)
    }

    getAllBookings():Observable<Booking[] | HttpError> {
      return this.httpClient.get<Booking[]>(`${environment.baseUrl}booking`)
    }

    addBooking(booking: Booking){
      return this.httpClient.post<any>(`${environment.baseUrl}booking/`, booking)
      .pipe(catchError((err) => this.handleHttpError(err)));
    }

    updateBooking(id: number, booking: Booking){
      return this.httpClient.put<any>(`${environment.baseUrl}booking/${id}`, booking)
      .pipe(catchError((err) => this.handleHttpError(err)))
    }

    delete(id: number){
      this.httpClient.delete<any>(`${environment.baseUrl}booking/${id}`).subscribe();
    }

 private handleHttpError(
    error:HttpErrorResponse
  ):Observable<HttpError>{
    return throwError(()=>new HttpError(
      error.status,
      error.statusText
    ))};
}