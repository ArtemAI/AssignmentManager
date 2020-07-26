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
        this.saveUserData(result.token)
        this.getLoggedInName.emit(userData.username);
      }));
  }

  public register(userData: any) {
    const formData = this.getFormDataValues(userData);
    return this.http.post<any>('/api/register', formData)
      .pipe(map(result => {
        this.saveUserData(result.token);
        this.getLoggedInName.emit(userData.username);
      }));
  }

  public logout() {
    localStorage.removeItem("token");
  }

  public isLoggedIn(): boolean {
    if (moment().isBefore(this.getExpiration())) {
      this.getLoggedInName.emit(this.getUserName());
      return true;
    }
    return false;
  }

  public getUserId(): string {
    const userToken = localStorage.getItem("token");
    let decodedToken = this.getDecodedJwt(userToken);
    return decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
  }

  public getUserRoleName(): string {
    const userToken = localStorage.getItem("token");
    let decodedToken = this.getDecodedJwt(userToken);
    return decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
  }

  private saveUserData(token: string) {
    localStorage.setItem("token", token);
  }

  private getUserName() {
    const userToken = localStorage.getItem("token");
    let decodedToken = this.getDecodedJwt(userToken);
    return decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
  }

  private getExpiration() {
    const userToken = localStorage.getItem("token");
    let decodedToken = this.getDecodedJwt(userToken);
    if (decodedToken == null) {
      return null;
    }
    let expiresAt = moment().add(decodedToken.exp, 'second');
    return moment(expiresAt);
  }

  private getFormDataValues(userData: any) {
    let formData = new FormData();
    for (let key in userData) {
      formData.append(key, userData[key]);
    }
    return formData;
  }

  private getDecodedJwt(token: string): any {
    try {
      return jwt_decode(token);
    }
    catch (Error) {
      return null;
    }
  }
}