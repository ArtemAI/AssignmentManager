import { AuthService } from 'src/app/services/auth.service';
import { UserProfile } from './../../models/user.profile.model';
import { Component, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { combineLatest } from 'rxjs';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AssignmentService } from '../../services/assignment.service';
import { UserService } from '../../services/user.service';
import { ProjectService } from '../../services/project.service';
import { Assignment } from '../../models/assignment.model';
import { faInfo, faPencilAlt, faTrash } from '@fortawesome/free-solid-svg-icons';
import { Project } from 'src/app/models/project.model';

@Component({
  templateUrl: 'assignments.component.html'
})
export class AssignmentComponent {

  @ViewChild('createModal') createModal: ModalDirective;
  @ViewChild('infoModal') infoModal: ModalDirective;
  @ViewChild('deleteModal') deleteModal: ModalDirective;

  assignmentList: Assignment[];
  userList: UserProfile[];
  userNames: string[];
  currentUserProject: Project;
  currentUserRoleName: string;
  currentSelectedAssignment: Assignment;
  assignmentForm: FormGroup;
  selectedUser: string;
  errorMessage: string;
  infoIcon = faInfo;
  pencilIcon = faPencilAlt;
  trashIcon = faTrash;

  constructor(private assignmentService: AssignmentService,
    private projectService: ProjectService,
    private userService: UserService,
    private authService: AuthService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.loadAssignmentList();
    this.getCurrentProject();
    this.loadUserList();
    this.currentUserRoleName = this.authService.getUserRoleName();
    this.assignmentForm = this.formBuilder.group({
      'id': [null],
      'name': ['', [Validators.required, Validators.maxLength(100)]],
      'description': ['', Validators.maxLength(1000)],
      'priority': [1, Validators.required],
      'deadline': [],
      'status': [0],
      'completionPercent': [0, [Validators.min(0), Validators.max(100)]],
      'assigneeId': ['', Validators.required],
      'projectId': [{ value: '', disabled: true }],
    });
  }

  loadAssignmentList() {
    combineLatest(this.assignmentService.getAll(), this.projectService.getAll(),
      this.userService.getAll(),
      (assignments, projects, users) => {
        return assignments.map(assignment => {
          assignment.project = projects.find(project => project.id === assignment.projectId);
          assignment.assignee = users.find(user => user.id === assignment.assigneeId);
          return assignment;
        });
      }
    ).subscribe(result => {
      this.assignmentList = result;
    });
  }

  getCurrentProject() {
    this.projectService.getAll().subscribe(result => {
      this.currentUserProject = result.pop();
      if (this.currentUserProject != undefined) {
        this.assignmentForm.patchValue({ projectId: this.currentUserProject.name });
      }
    });
  }

  loadUserList() {
    this.userService.getAll().subscribe(result => {
      this.userList = result;
      this.userNames = new Array();
      this.userList.forEach(element => {
        this.userNames.push(this.getUserFullName(element));
      });
    });
  }

  onCreateButtonClicked() {
    this.createModal.show();
  }

  onEditButtonClicked(selectedAssignment: Assignment) {
    let deadlineDate = new Date(selectedAssignment.deadline.toString());
    this.createModal.show();
    this.assignmentForm.setValue({
      id: selectedAssignment.id,
      name: selectedAssignment.name,
      description: selectedAssignment.description,
      priority: selectedAssignment.priority,
      deadline: deadlineDate.toLocaleDateString(),
      status: selectedAssignment.status,
      completionPercent: selectedAssignment.completionPercent,
      assigneeId: this.getUserFullName(selectedAssignment.assignee),
      projectId: selectedAssignment.project.name
    });
  }

  onSubmit(submittedAssignment: Assignment) {
    if (submittedAssignment.deadline != null) {
      submittedAssignment.deadline.toLocaleDateString();
    }
    submittedAssignment.assigneeId = this.getUserIdByName(this.selectedUser);
    submittedAssignment.projectId = this.currentUserProject.id;
    if (submittedAssignment.id == null) {
      this.assignmentService.create(submittedAssignment).subscribe(
        () => {
          this.loadAssignmentList();
          this.assignmentForm.reset();
          this.createModal.hide();
        },
        submitError => {
          this.errorMessage = submitError.error;
        }
      );
    }
    else {
      this.assignmentService.update(submittedAssignment).subscribe(
        () => {
          this.loadAssignmentList();
          this.assignmentForm.reset();
          this.createModal.hide();
        },
        submitError => {
          this.errorMessage = submitError.error;
        }
      );
    }
  }

  onInfoButtonClicked(selectedAssignment: Assignment) {
    this.currentSelectedAssignment = selectedAssignment;
    this.infoModal.show();
  }

  onDeleteButtonClicked(selectedAssignment: Assignment) {
    this.currentSelectedAssignment = selectedAssignment;
    this.deleteModal.show();
  }

  onDeletionConfirm() {
    this.assignmentService.delete(this.currentSelectedAssignment.id).subscribe(() => {
      this.assignmentList = this.assignmentList.filter(({ id }) => id !== this.currentSelectedAssignment.id);
      this.deleteModal.hide();
    });
  }

  getUserIdByName(userName: string): string {
    let result: string;
    this.userList.forEach(element => {
      if (this.getUserFullName(element) === userName) {
        result = element.id;
      }
    });
    return result;
  }

  getUserFullName(user: UserProfile): string {
    return user.firstName + ' ' + user.lastName;
  }
}
