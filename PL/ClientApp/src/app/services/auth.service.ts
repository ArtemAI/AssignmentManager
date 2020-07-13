import { Injectable, EventEmitter, Output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import * as jwt_decode from "jwt-decode";
import * as moment from "moment";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  @Output()
  getLoggedInName: EventEmitter<any> = new EventEmitter();

  constructor(private http: HttpClient) { }

  public login(userData: any) {
    const formData = this.getFormDataValues(userData);
    return this.http.post<any>('/api/login', formData)
      .pipe(map(result => {
        this.saveUserData(result.token, userData.username)
        this.getLoggedInName.emit(userData.username);
      }));
  }

  public register(userData: any) {
    const formData = this.getFormDataValues(userData);
    return this.http.post<any>('/api/register', formData)
      .pipe(map(result => {
        this.saveUserData(result.token, userData.username);
        this.getLoggedInName.emit(userData.username);
      }));
  }

  public logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("username");
    localStorage.removeItem("expires_at");
  }

  public isLoggedIn() {
    if (moment().isBefore(this.getExpiration())) {
      const username = localStorage.getItem("username");
      this.getLoggedInName.emit(username);
      return true;
    }
    return false;
  }

  private saveUserData(token: string, username: string) {
    const expiresAt = moment().add(this.getDecodedAccessToken(token).exp, 'second');
    localStorage.setItem('token', token);
    localStorage.setItem("username", username);
    localStorage.setItem('expires_at', JSON.stringify(expiresAt.valueOf()));
  }

  private getExpiration() {
    const expiration = localStorage.getItem("expires_at");
    const expiresAt = JSON.parse(expiration);
    return moment(expiresAt);
  }

  private getFormDataValues(userData: any) {
    let formData = new FormData();
    for (let key in userData) {
      formData.append(key, userData[key]);
    }
    return formData;
  }

  private getDecodedAccessToken(token: string): any {
    try {
      return jwt_decode(token);
    }
    catch (Error) {
      return null;
    }
  }
}