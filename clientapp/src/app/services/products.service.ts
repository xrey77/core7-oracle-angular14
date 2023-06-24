import { Injectable } from '@angular/core';
import { Product } from '../interface/product';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import {  Observable, of, throwError } from 'rxjs';
import { retry, catchError} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {

  constructor(
    private httpclient: HttpClient

  ) { }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      this.log(`${operation} failed: ${error.message}`);
  
      return of(result as T);
    };
  }
  log(arg0: string) {
    throw new Error('Method not implemented.');
  }

  public sendSearchRequest(search: any): Observable<any>
  {
    const options = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
        // 'Authorization': 'jwt-token'
      })
    };
    return this.httpclient.post<any>("http://localhost:5099/searchproducts", search, options)
    .pipe(catchError(this.handleError()));
  }

  public sendProductRequest(page: any): Observable<any>
  {
    const options = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
        // 'Authorization': 'jwt-token'
      })
    };
    return this.httpclient.get("http://localhost:5099/listproducts/" + page, options)
    .pipe(catchError(this.handleError()));        
  }

}
