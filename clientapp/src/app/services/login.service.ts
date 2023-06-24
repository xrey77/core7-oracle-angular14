import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import {  Observable, throwError } from 'rxjs';
import { retry, catchError} from 'rxjs/operators';
import { User } from '../interface/user';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(
    private httpclient: HttpClient
  ) { }

  handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      return throwError(() => new Error( error.error));
    } else {
        return throwError(() => new Error( error.error));
    }
  }  

  public sendLoginRequest(userdtls: User): Observable<any> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
      })
    };

    return this.httpclient.post<User>("http://localhost:5099/signin",userdtls, options)
    .pipe(catchError(this.handleError));
 }    
}
