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

    // getAvailableDates():Observable<Date[] | HttpError>{
    //     return this.httpClient.get<Date[]>(`${environment.baseUrl}booking`)
    //     .pipe(catchError((err) => this.handleHttpError(err)));
    // }

    getAvailableDates(): Observable<Date[] | HttpError>{
        console.log(environment.baseUrl)
        return this.httpClient.get<Date[]>(`${environment.baseUrl}booking/available`)
        .pipe(catchError((err) => this.handleHttpError(err)));;
    }


 private handleHttpError(
    error:HttpErrorResponse
  ):Observable<HttpError>{
    return throwError(()=>new HttpError(
      error.status,
      error.statusText
    ))};
}