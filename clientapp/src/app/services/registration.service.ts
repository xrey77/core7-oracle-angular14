import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpResponse } from '@angular/common/http';
import {  Observable, throwError } from 'rxjs';
import { retry, catchError} from 'rxjs/operators';
import { User } from '../interface/user';

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {

  constructor(
    private httpclient: HttpClient
  ) { }

  handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      return throwError(() => new Error(error.error));     
    } else {
      return throwError(() => new Error(error.error));      
    }
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }  
  
  public sendRegistrationRequest(userdtls: User): Observable<any> {
    const option = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        // 'Authorization': 'jwt-token'
      })
    };

    return this.httpclient.post<User>("http://localhost:5099/signup",userdtls, option)
    .pipe(catchError(this.handleError));      
 }    

}
