import { Resource } from './resource.model';

export interface UserProfile extends Resource {
    firstName: string;
    lastName: string;
    projectId: string;
    email: string;
    allowEmailNotifications: boolean;
  }