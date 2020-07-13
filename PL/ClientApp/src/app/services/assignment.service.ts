import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ResourceService } from "./resource.service";
import { Assignment } from "../models/assignment.model";

@Injectable({
  providedIn: 'root'
})
export class AssignmentService extends ResourceService<Assignment> {
  constructor(httpClient: HttpClient) {
    super(httpClient, 'api/assignments');
  }
}