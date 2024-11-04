import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MessageService } from './message.service';
import { Observable, tap, catchError, of, map } from 'rxjs';
import { Product } from './product';
import { GetProducts } from './GetProducts';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private ProductsUrl = 'https://localhost:7285/api/Products';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  constructor(
    private http: HttpClient,
    private messageService:MessageService
  ) { }

  getProducts(): Observable<GetProducts[]> {
    return this.http.get<GetProducts[]>(this.ProductsUrl)
      .pipe(
        tap(_ => this.log('fetched Products')),
        catchError(this.handleError<GetProducts[]>('getProducts', []))
      );
  }
  getProductNo404<Data>(id: number): Observable<GetProducts> {
    const url = `${this.ProductsUrl}/?id=${id}`;
    return this.http.get<GetProducts[]>(url)
      .pipe(
        map(Products => Products[0]), // returns a {0|1} element array
        tap(h => {
          const outcome = h ? 'fetched' : 'did not find';
          this.log(`${outcome} Product id=${id}`);
        }),
        catchError(this.handleError<GetProducts>(`getProduct id=${id}`))
      );
  }
  getProduct(id: number): Observable<GetProducts> {
    const url = `${this.ProductsUrl}/${id}`;
    return this.http.get<GetProducts>(url).pipe(
      tap(_ => this.log(`fetched Product id=${id}`)),
      catchError(this.handleError<GetProducts>(`getProduct id=${id}`))
    );
  }
  searchProducts(term: string): Observable<Product[]> {
    if (!term.trim()) {
      // if not search term, return empty Product array.
      return of([]);
    }
    return this.http.get<Product[]>(`${this.ProductsUrl}/?name=${term}`).pipe(
      tap(x => x.length ?
        this.log(`found Products matching "${term}"`) :
        this.log(`no Products matching "${term}"`)),
      catchError(this.handleError<Product[]>('searchProducts', []))
    );
  }
  addProduct(Product: Product): Observable<Product> {
    return this.http.post<Product>(this.ProductsUrl, Product, this.httpOptions).pipe(
      tap(_ => this.log('Added Products')),
      catchError(this.handleError<Product>('addProduct'))
    );
  }
  deleteProduct(id: number): Observable<Product> {
    const url = `${this.ProductsUrl}/${id}`;

    return this.http.delete<Product>(url, this.httpOptions).pipe(
      tap(_ => this.log(`deleted Product id=${id}`)),
      catchError(this.handleError<Product>('deleteProduct'))
    );
  }

  updateProduct(Product: GetProducts): Observable<Product> {
    const url = `${this.ProductsUrl}/${Product.id}`;
    return this.http.put<Product>(url, Product, this.httpOptions).pipe(
      tap(updatedProduct => this.log(`Updated Product id`)),
      catchError(this.handleError<Product>('updateProduct'))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
  private log(message: string) {
    this.messageService.add(`ProductService: ${message}`);
  }
  
}