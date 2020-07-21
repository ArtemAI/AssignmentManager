import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";
import { Resource } from "../models/resource.model";

@Injectable({
  providedIn: 'root'
})
export abstract class ResourceService<T extends Resource> {
  constructor(
    protected httpClient: HttpClient,
    protected endpointUrl: string) { }

  public create(item: T): Observable<T> {
    return this.httpClient
      .post<T>(`${this.endpointUrl}`, item);
  }

  public update(item: T): Observable<T> {
    return this.httpClient
      .put<T>(`${this.endpointUrl}/${item.id}`, item);
  }

  public getById(id: string): Observable<T> {
    return this.httpClient
      .get<T>(`${this.endpointUrl}/${id}`);
  }

  public getAll(): Observable<T[]> {
    return this.httpClient
      .get<T[]>(`${this.endpointUrl}`);
  }

  public delete(id: string): Observable<any> {
    return this.httpClient
      .delete(`${this.endpointUrl}/${id}`);
  }
}