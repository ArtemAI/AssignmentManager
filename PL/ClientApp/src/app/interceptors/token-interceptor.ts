import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TokenInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const token = localStorage.getItem("token");
    if (token) {
      const request = req.clone({
        headers: req.headers.set("Authorization", "Bearer " + token)
      });

      return next.handle(request);
    }
    else {
      return next.handle(req);
    }
  }
}