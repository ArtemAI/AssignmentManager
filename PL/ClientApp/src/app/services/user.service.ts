import { HttpClient } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { UserProfile } from '../models/user.profile.model';
import { ResourceService } from "./resource.service";
import { Observable } from "rxjs/internal/Observable";

@Injectable({
  providedIn: 'root'
})
export class UserService extends ResourceService<UserProfile> {
  constructor(httpClient: HttpClient) {
    super(httpClient, 'api/users');
  }

  public getRoles(): Observable<string[]> {
    return this.httpClient
      .get<string[]>(`${this.endpointUrl}/roles`);
  }

  public setRole(userId: string, role: any): Observable<void> {
    return this.httpClient
      .put<void>(`${this.endpointUrl}/${userId}/role`, role);
  }

  public removeFromProject(id: string): Observable<any> {
    return this.httpClient
      .delete(`${this.endpointUrl}/${id}/project`);
  }
}
