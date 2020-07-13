import { HttpClient } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { Project } from '../models/project.model';
import { ResourceService } from "./resource.service";

@Injectable({
  providedIn: 'root'
})
export class ProjectService extends ResourceService<Project> {
  constructor(httpClient: HttpClient) {
    super(httpClient, 'api/projects');
  }
}