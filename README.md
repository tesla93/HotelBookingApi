### Problem to solve

Post-Covid scenario:
People are now free to travel everywhere but because of the pandemic, a lot of hotels went bankrupt. Some former famous travel places are left with only one hotel.
You’ve been given the responsibility to develop a booking API for the very last hotel in Cancun.
The requirements are:
- API will be maintained by the hotel’s IT department.
- As it’s the very last hotel, the quality of service must be 99.99 to 100% => no downtime
- For the purpose of the test, we assume the hotel has only one room available
- To give a chance to everyone to book the room, the stay can’t be longer than 3 days and can’t be reserved more than 30 days in advance.
- All reservations start at least the next day of booking,
- To simplify the use case, a “DAY’ in the hotel room starts from 00:00 to 23:59:59.
- Every end-user can check the room availability, place a reservation, cancel it or modify it.
- To simplify the API is insecure.

### STACK USED

API:
- .Net 5
- AutoMapper
- Entity Framework
- Serilog

Unit Test for APIÑ
- Xunit
- FluentAssertions
- Moq

Frontend:
- Angular 14.1.3
- PrimeNG

## Endpoints
| Verb |  Path                  | Description                 |
| ---- | ---------------------- | --------------------------- |
| GET  | /api/booking           | Get all reservations |
| POST | /api/booking           | Create Reservation |
| PUT  | /api/booking/{id}      | Modify a reservation |
| DELETE | /api/booking/cancel/1 | Delete a reservation |
| GET  | api/booking/available  | Get available dates |

## Instructions

1. To make this project work, open a terminal in the HotelBookingAPI folder and type `dotnet restore`. 

2. Once you have finished inside the terminal, type `dotnet run` and this will cause an api to be raised at the address https://localhost:44357. Automatically a database will be created in SQL Server and the corresponding migrations will be applied

3. In https://localhost:44357/swagger/index.html the api is documented.

4. Then to get the client working `cd BookingFrontApp` and type `npm install` to restore the necessary packages. 

5. Once finished, write `ng serve` to launch the client application that will be available at address https://localhost:4200 This client application has a fairly basic UI since it is only for demonstration purposes..



