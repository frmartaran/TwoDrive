import { LogoutService } from './../services/logout.service';
import { Component } from '@angular/core';
import { ConfirmDialogModel, ConfirmDialogComponent } from 'src/app/components/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material';
 
@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(private logoutService: LogoutService,
    public dialog: MatDialog) {}

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
        localStorage.removeItem('userId');
        localStorage.removeItem('isAdmin');
        window.location.href = '/login';
        window.location.reload();
      }
    });
  }
}