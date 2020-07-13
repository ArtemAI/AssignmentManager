import { Resource } from './resource.model';
import { Project } from './project.model';
import { UserProfile } from './user.profile.model';

export interface Assignment extends Resource {
  name: string;
  description: string;
  priority: number;
  completionPercent: number;
  status: number;
  deadline: Date;
  projectId: string;
  assigneeId: string;

  project: Project;
  assignee: UserProfile;
}
