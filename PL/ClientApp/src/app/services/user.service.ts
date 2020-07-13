import { HttpClient } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { UserProfile } from '../models/user.profile.model';
import { ResourceService } from "./resource.service";

@Injectable({
  providedIn: 'root'
})
export class UserService extends ResourceService<UserProfile> {
  constructor(httpClient: HttpClient) {
    super(httpClient, 'api/users');
  }
}
