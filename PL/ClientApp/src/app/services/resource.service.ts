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
			.post<T>(`${this.endpointUrl}`, item)
			.pipe(catchError(this.handleError<T>()));
	}

	public update(item: T): Observable<T> {
		return this.httpClient
			.put<T>(`${this.endpointUrl}/${item.id}`, item)
			.pipe(catchError(this.handleError<T>()));
	}

	public getById(id: string): Observable<T> {
		return this.httpClient
			.get<T>(`${this.endpointUrl}/${id}`)
			.pipe(catchError(this.handleError<T>()));
	}

	public getAll(): Observable<T[]> {
		return this.httpClient
			.get<T[]>(`${this.endpointUrl}`)
			.pipe(catchError(this.handleError<T[]>()));
	}

	public delete(id: string): Observable<any> {
		return this.httpClient
			.delete(`${this.endpointUrl}/${id}`)
			.pipe(catchError(this.handleError<T[]>()));
	}

	private handleError<T>(result?: T) {
		return (error: any): Observable<T> => {
			const msg = `${error.status} ${error.statusText} -  ${error.url}`;
			console.error(error);
			return of(result as T);
		};
	}
}