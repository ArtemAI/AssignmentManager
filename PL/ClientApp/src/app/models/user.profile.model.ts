import { Resource } from './resource.model';
import { Project } from './project.model';

export interface UserProfile extends Resource {
  firstName: string;
  lastName: string;
  projectId: string;
  email: string;
  allowEmailNotifications: boolean;

  project: Project;
}
