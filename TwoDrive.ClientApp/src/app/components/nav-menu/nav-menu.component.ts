import { LogoutService } from 'src/app/services/logout.service';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmDialogModel, ConfirmDialogComponent } from 'src/app/components/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material';
import { Writer } from 'src/app/components/interfaces/interfaces.model';
 
@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  isAdmin : boolean;
  username : string;
  public basicLoggedInWriter : Writer = {
    id: 0,
    role: '',
    userName: '',
    password: '',
    friends: null,
    claims: null,
    isFriendsWithUserLoggedIn: false
  };

  constructor(private logoutService: LogoutService,
    private router: Router,
    public dialog: MatDialog) {
      this.isAdmin = (localStorage.getItem('isAdmin') == 'true');
      this.username = localStorage.getItem('username');
    }

  collapse() {
    this.isExpanded = false;
  }
    toggle() {
    this.isExpanded = !this.isExpanded;
  }
 
  confirmDialog(): void {
    const message = `Are you sure you want to log out?`;
 
    const dialogData = new ConfirmDialogModel("Confirm Logout", message);
 
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
 
    dialogRef.afterClosed().subscribe(dialogResult => {
      if(dialogResult){
        this.logoutService.Logout().subscribe();
        localStorage.removeItem('token');
        localStorage.removeItem('writerId');
        localStorage.removeItem('isAdmin');
        localStorage.removeItem('username');
        window.location.href = '/login';
        window.location.reload();
      }
    });
  }

  editLoggedInWriter(){
    this.basicLoggedInWriter.id = +localStorage.getItem('writerId');
    this.basicLoggedInWriter.userName = this.username
    this.basicLoggedInWriter.role = this.isAdmin
      ? 'Administrator'
      : 'Writer';
    this.router.navigateByUrl('/', {skipLocationChange: true})
    .then(()=> this.router.navigate(['/edit-writer'], {state: this.basicLoggedInWriter}));
  }

  createWriter(){
    this.router.navigateByUrl('/', {skipLocationChange: true})
    .then(()=> this.router.navigate(['/create-writer']));
  }

  homePage(){
    this.router.navigate(['/home-page'])
  }
}