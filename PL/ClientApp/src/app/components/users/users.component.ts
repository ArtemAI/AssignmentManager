import { Component, ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { UserService } from '../../services/user.service'
import { UserProfile } from '../../models/user.profile.model';
import { faPencilAlt } from '@fortawesome/free-solid-svg-icons';

@Component({
  templateUrl: 'Users.component.html'
})
export class UserComponent {

  @ViewChild('infoModal', { static: false }) public infoModal: ModalDirective;

  public userList: UserProfile[];
  public currentSelectedItem: UserProfile;
  public pencilIcon = faPencilAlt;

  constructor(public userService: UserService) { }

  ngOnInit(): void {
    this.userService.getAll().subscribe(result => {
      this.userList = result;
    }, error => console.error(error));
  }

  onManageButtonClicked(selectedItem: UserProfile) { }
}