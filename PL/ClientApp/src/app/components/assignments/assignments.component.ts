import { Component, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { combineLatest } from 'rxjs';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AssignmentService } from '../../services/assignment.service';
import { UserService } from '../../services/user.service';
import { ProjectService } from '../../services/project.service';
import { Assignment } from '../../models/assignment.model';
import { faInfo, faPencilAlt, faTrash } from '@fortawesome/free-solid-svg-icons';

@Component({
  templateUrl: 'assignments.component.html'
})
export class AssignmentComponent {

  @ViewChild('createModal') public createModal: ModalDirective;
  @ViewChild('infoModal') public infoModal: ModalDirective;
  @ViewChild('deleteModal') public deleteModal: ModalDirective;

  public assignmentList: Assignment[];
  public currentSelectedAssignment: Assignment;
  public assignmentForm: FormGroup;
  public errorMessage: string;
  public infoIcon = faInfo;
  public pencilIcon = faPencilAlt;
  public trashIcon = faTrash;

  constructor(public assignmentService: AssignmentService,
    public projectService: ProjectService,
    public userService: UserService,
    public formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.loadAssignmentList();
    this.assignmentForm = this.formBuilder.group({
      'id': [null],
      'name': ['', [Validators.required, Validators.maxLength(100)]],
      'description': ['', Validators.maxLength(1000)],
      'priority': [1, Validators.required],
      'deadline': [''],
      'status': [0],
      'completionPercent': [0, [Validators.min(0), Validators.max(100)]],
      'projectId': ['', Validators.required],
      'assigneeId': ['', Validators.required],
    });
  }

  loadAssignmentList() {
    const combinedAssignments$ = combineLatest(
      this.assignmentService.getAll(),
      this.projectService.getAll(),
      this.userService.getAll(),
      (assignments, projects, users) => {
        assignments = this.transformToArray(assignments);
        projects = this.transformToArray(projects);
        users = this.transformToArray(users);
        return assignments.map(assignment => {
          assignment.project = projects.find(project => project.id === assignment.projectId);
          assignment.assignee = users.find(user => user.id === assignment.assigneeId);
          return assignment;
        })
      }
    );

    combinedAssignments$.subscribe(result => {
      this.assignmentList = result;
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

  onInfoButtonClicked(selectedAssignment: Assignment) {
    this.currentSelectedAssignment = selectedAssignment;
    this.infoModal.show()
  }

  onDeleteButtonClicked(selectedAssignment: Assignment) {
    this.assignmentService.delete(selectedAssignment.id).subscribe(response => {
      this.assignmentList = this.assignmentList.filter(({ id }) => id !== selectedAssignment.id);
    });
  }

  onCreateButtonClicked() {
    this.createModal.show()
  }

  onEditButtonClicked(selectedAssignment: Assignment) {
    this.createModal.show();
    this.assignmentForm.setValue({
      id: selectedAssignment.id,
      name: selectedAssignment.name,
      description: selectedAssignment.description,
      priority: selectedAssignment.priority,
      deadline: selectedAssignment.deadline,
      status: selectedAssignment.status,
      completionPercent: selectedAssignment.completionPercent,
      projectId: selectedAssignment.projectId,
      assigneeId: selectedAssignment.assigneeId
    })
  }

  onSubmit(submittedAssignment: Assignment) {
    submittedAssignment.deadline.toLocaleDateString();
    if (submittedAssignment.id == null) {
      this.assignmentService.create(submittedAssignment).subscribe(
        () => {
          this.assignmentList.push(submittedAssignment);
          this.loadAssignmentList();
          this.assignmentForm.reset();
          this.createModal.hide();
        },
        submitError => {
          this.errorMessage = submitError.error.message;
          console.log(submitError)
        }
      );
    }
    else {
      this.assignmentService.update(submittedAssignment).subscribe(
        () => {
          var index = this.assignmentList.indexOf(this.currentSelectedAssignment);
          this.assignmentList[index] = submittedAssignment;
          this.loadAssignmentList();
          this.assignmentForm.reset();
          this.createModal.hide();
        },
        submitError => {
          this.errorMessage = submitError.error.message;
        }
      );
    }
  }
}