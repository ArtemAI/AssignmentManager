import { ProjectService } from './../../services/project.service';
import { AuthService } from 'src/app/services/auth.service';
import { Component, ViewChild } from '@angular/core';
import { UserService } from '../../services/user.service'
import { UserProfile } from '../../models/user.profile.model';
import { faPencilAlt } from '@fortawesome/free-solid-svg-icons';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { combineLatest } from 'rxjs';
import { Project } from 'src/app/models/project.model';

@Component({
  templateUrl: 'Users.component.html'
})
export class UserComponent {

  @ViewChild('manageModal') manageModal: ModalDirective;

  userList: UserProfile[];
  roleList: string[];
  currentUserRoleName: string;
  errorMessage: string;
  currentSelectedUser: UserProfile;
  userManageForm: FormGroup;
  pencilIcon = faPencilAlt;

  constructor(private userService: UserService,
    private projectService: ProjectService,
    private formBuilder: FormBuilder,
    private authService: AuthService) { }

  ngOnInit(): void {
    this.currentUserRoleName = this.authService.getUserRoleName();
    this.userManageForm = this.formBuilder.group({
      'username': [{ value: '', disabled: true }],
      'role': ['Employee']
    });
    combineLatest(this.projectService.getAll(), this.userService.getAll(),
      (projects: Project[], users: UserProfile[]) => {
        return users.map(user => {
          user.project = projects.find(project => project.id === user.projectId);
          return user;
        });
      }
    ).subscribe(result => {
      this.userList = result;
    });
    this.userService.getRoles().subscribe(roles => {
      this.roleList = roles;
    });
  }

  onManageButtonClicked(selectedUser: UserProfile) {
    this.currentSelectedUser = selectedUser;
    this.userManageForm.patchValue({ username: this.getUserFullName(selectedUser) });
    this.manageModal.show();
  }

  onSubmit() {
    let role = this.userManageForm.value.role;
    this.userService.setRole(this.currentSelectedUser.id, { 'role': role }).subscribe(
      () => { this.manageModal.hide(); },
      submitError => { this.errorMessage = submitError.error }
    );
  }

  canCurrentUserManageUsers(): boolean {
    return this.currentUserRoleName == 'Manager' || this.currentUserRoleName == 'Administrator';
  }

  getUserFullName(user: UserProfile): string {
    return user.firstName + ' ' + user.lastName;
  }
}
