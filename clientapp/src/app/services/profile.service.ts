import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import {  Observable, throwError } from 'rxjs';
import { retry, catchError} from 'rxjs/operators';
import { Userprofile } from '../interface/userprofile';

interface ActivateMfa {
  Twofactorenabled: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(
    private httpclient: HttpClient
  ) { }

  handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      return throwError(() => new Error(error.error));
    } else {
      return throwError(() => new Error(error.error));
    }
  }  

  public getUserbyId(id: string, token: any): Observable<any> {

    const options = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        'Authorization': `Bearer ${token}`
      })
    };    

    //let params1 = this.httpParams.set("userId",id);
    return this.httpclient.get("http://localhost:5099/api/getuserbyid/" + id, options)
    .pipe(catchError(this.handleError));      
  }

  public ActivateMFA(id: string, enabled: ActivateMfa, token: any) {
    const options = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        'Authorization': `Bearer ${token}`
      })
    };    

    return this.httpclient.put("http://localhost:5099/api/enablemfa/" + id, enabled, options)
    .pipe(catchError(this.handleError));
  }

  public UploadPicture(profilepic: any, token: any): Observable<any> {
    const options = {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
    };    
    return this.httpclient.post<any>("http://localhost:5099/api/uploadpicture", profilepic, options)
    .pipe(catchError(this.handleError));
  }

  public sendProfileRequest(id: string, userdtls: Userprofile, token: any): Observable<any> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        'Authorization': `Bearer ${token}`
      })
    };    

    return this.httpclient.post<Userprofile>("http://localhost:5099/api/updateprofile/" + id,userdtls, options)
    .pipe(catchError(this.handleError));
  }
}
