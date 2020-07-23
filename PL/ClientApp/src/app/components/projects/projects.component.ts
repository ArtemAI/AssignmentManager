import { FormGroup, FormBuilder, Validators } from '@angular/forms';
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

  @ViewChild('createModal') createModal: ModalDirective;
  @ViewChild('infoModal') infoModal: ModalDirective;
  @ViewChild('deleteModal') deleteModal: ModalDirective;

  projectList: Project[];
  currentSelectedProject: Project;
  projectForm: FormGroup;
  errorMessage: string;
  infoIcon = faInfo;
  pencilIcon = faPencilAlt;
  trashIcon = faTrash;

  constructor(private projectService: ProjectService,
    private userService: UserService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.loadProjects();
    this.projectForm = this.formBuilder.group({
      'id': [null],
      'name': ['', [Validators.required, Validators.maxLength(100)]],
      'description': ['', Validators.maxLength(1000)]
    });
  }

  loadProjects() {
    const combinedProjects$ = combineLatest(
      this.projectService.getAll(),
      this.userService.getAll(),
      (projects: Project[], users: UserProfile[]) => {
        projects = this.transformToArray(projects);
        return projects.map(project => {
          project.manager = users.find(user => user.id === project.managerId);
          return project;
        });
      }
    );

    combinedProjects$.subscribe(result => {
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
          this.errorMessage = submitError.error.message;
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
          this.errorMessage = submitError.error.message;
        }
      );
    }
  }

  onInfoButtonClicked(selectedProject: Project) {
    this.currentSelectedProject = selectedProject;
    this.infoModal.show();
  }

  onDeleteButtonClicked(selectedProject: Project) {
    this.projectService.delete(selectedProject.id).subscribe(response => {
      this.projectList = this.projectList.filter(({ id }) => id !== selectedProject.id);
    });
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
}
