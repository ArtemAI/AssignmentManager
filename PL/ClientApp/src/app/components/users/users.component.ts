import { Component, ViewChild } from '@angular/core';
import { UserService } from '../../services/user.service'
import { UserProfile } from '../../models/user.profile.model';
import { faPencilAlt } from '@fortawesome/free-solid-svg-icons';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  templateUrl: 'Users.component.html'
})
export class UserComponent {

  @ViewChild('manageModal') manageModal: ModalDirective;

  userList: UserProfile[];
  currentSelectedUser: UserProfile;
  userManageForm: FormGroup;
  pencilIcon = faPencilAlt;

  constructor(private userService: UserService, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.userManageForm = this.formBuilder.group({
      'username': [{ value: '', disabled: true }],
      'role': ['', Validators.required]
    });
    this.userService.getAll().subscribe(result => {
      this.userList = result;
    });
  }

  onManageButtonClicked(selectedUser: UserProfile) {
    this.currentSelectedUser = selectedUser;
    this.userManageForm.patchValue({ username: this.getUserFullName(selectedUser) });
    this.manageModal.show();
  }

  getUserFullName(user: UserProfile): string {
    return user.firstName + ' ' + user.lastName;
  }
}
