import { Resource } from "./resource.model";
import { UserProfile } from "./user.profile.model";

export interface Project extends Resource {
  name: string;
  description: string;
  managerId: string;

  manager: UserProfile
}