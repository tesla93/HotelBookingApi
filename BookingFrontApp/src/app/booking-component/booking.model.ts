export class Booking{
    id: number;
    checkInDate: Date;
    checkOutDate: Date;
    dateRange?: Date[]
    roomNumber?: number;
    guestName?: string;
    created?: Date;
    updated?: Date;
}