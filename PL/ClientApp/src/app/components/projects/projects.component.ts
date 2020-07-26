import { AuthService } from 'src/app/services/auth.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UserService } from './../../services/user.service';
import { combineLatest } from 'rxjs';
import { Component, ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ProjectService } from '../../services/project.service'
import { Project } from '../../models/project.model';
import { faInfo, faPencilAlt, faTrash, faCross } from '@fortawesome/free-solid-svg-icons';
import { UserProfile } from 'src/app/models/user.profile.model';
import { faTimesCircle } from '@fortawesome/free-regular-svg-icons';

@Component({
  templateUrl: 'Projects.component.html'
})
export class ProjectComponent {

  @ViewChild('createModal') createModal: ModalDirective;
  @ViewChild('infoModal') infoModal: ModalDirective;
  @ViewChild('deleteModal') deleteModal: ModalDirective;

  projectList: Project[];
  currentSelectedProject: Project;
  projectForm: FormGroup;
  currentUserRoleName: string;
  errorMessage: string;
  infoIcon = faInfo;
  pencilIcon = faPencilAlt;
  trashIcon = faTrash;
  leaveIcon = faTimesCircle;

  constructor(private projectService: ProjectService,
    private userService: UserService,
    private authService: AuthService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.loadProjects();
    this.currentUserRoleName = this.authService.getUserRoleName();
    this.projectForm = this.formBuilder.group({
      'id': [null],
      'name': ['', [Validators.required, Validators.maxLength(100)]],
      'description': ['', Validators.maxLength(1000)]
    });
  }

  loadProjects() {
    combineLatest(this.projectService.getAll(), this.userService.getAll(),
      (projects: Project[], users: UserProfile[]) => {
        return projects.map(project => {
          project.manager = users.find(user => user.id === project.managerId);
          return project;
        });
      }
    ).subscribe(result => {
      this.projectList = result;
    });
  }

  onCreateButtonClicked() {
    this.createModal.show();
  }

  onEditButtonClicked(selectedProject: Project) {
    this.createModal.show();
    this.projectForm.setValue({
      id: selectedProject.id,
      name: selectedProject.name,
      description: selectedProject.description,
    })
  }

  onSubmit(submittedProject: Project) {
    if (submittedProject.id == null) {
      this.projectService.create(submittedProject).subscribe(
        () => {
          this.loadProjects();
          this.projectForm.reset();
          this.createModal.hide();
        },
        submitError => {
          this.errorMessage = submitError.error;
        }
      );
    }
    else {
      this.projectService.update(submittedProject).subscribe(
        () => {
          this.loadProjects();
          this.projectForm.reset();
          this.createModal.hide();
        },
        submitError => {
          this.errorMessage = submitError.error;
        }
      );
    }
  }

  onInfoButtonClicked(selectedProject: Project) {
    this.currentSelectedProject = selectedProject;
    this.infoModal.show();
  }

  onLeaveButtonClicked(selectedProject: Project) {
    let currentUserId = this.authService.getUserId();
    this.currentSelectedProject = selectedProject;
    this.userService.removeFromProject(currentUserId).subscribe(() => {
      this.projectList = this.projectList.filter(({ id }) => id !== this.currentSelectedProject.id);
    });
  }

  onDeleteButtonClicked(selectedProject: Project) {
    this.currentSelectedProject = selectedProject;
    this.deleteModal.show();
  }

  onDeletionConfirm() {
    this.projectService.delete(this.currentSelectedProject.id).subscribe(() => {
      this.projectList = this.projectList.filter(({ id }) => id !== this.currentSelectedProject.id);
      this.deleteModal.hide();
    });
  }

  canCurrentUserChangeProject(): boolean {
    return this.currentUserRoleName == 'Manager' || this.currentUserRoleName == 'Administrator';
  }
}
