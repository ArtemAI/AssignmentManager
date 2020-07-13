import { UserService } from './../../services/user.service';
import { combineLatest } from 'rxjs';
import { Component, ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ProjectService } from '../../services/project.service'
import { Project } from '../../models/project.model';
import { faInfo, faPencilAlt, faTrash } from '@fortawesome/free-solid-svg-icons';
import { UserProfile } from 'src/app/models/user.profile.model';

@Component({
  templateUrl: 'Projects.component.html'
})
export class ProjectComponent {

  @ViewChild('infoModal', { static: false }) public infoModal: ModalDirective;

  public projectList: Project[];
  public currentSelectedProject: Project;
  public infoIcon = faInfo;
  public pencilIcon = faPencilAlt;
  public trashIcon = faTrash;

  constructor(public projectService: ProjectService, public userService: UserService) { }

  ngOnInit(): void {
    const combinedProjects$ = combineLatest(
      this.projectService.getAll(),
      this.userService.getAll(),
      (projects: Project[], users: UserProfile[]) => {
        projects = this.transformToArray(projects);
        return projects.map(project => {
          project.manager = users.find(user => user.id === project.managerId);
          return project;
        })
      }
    )

    combinedProjects$.subscribe(result => {
      this.projectList = result;
    }, error => console.error(error));
  }

  transformToArray(value) {
    if (value instanceof Array) {
      return value;
    }
    else {
      var array = [];
      array.push(value);
      return array;
    }
  }

  onInfoButtonClicked(selectedProject: Project) {
    this.currentSelectedProject = selectedProject;
    this.infoModal.show()
  }

  onDeleteButtonClicked(selectedProject: Project) {
    this.projectService.delete(selectedProject.id).subscribe(response => {
      this.projectList = this.projectList.filter(({ id }) => id !== selectedProject.id);
    });
  }

  onEditButtonClicked(selectedProject: Project) { }
}