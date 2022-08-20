export class HttpError{
    errorNumber: number;
    message:string
  
    constructor(errorNumber: number, message: string){
      this.errorNumber = errorNumber;
      this.message=message;
    }
  }