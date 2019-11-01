import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { Component, OnInit } from '@angular/core';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-create-writer',
  templateUrl: './create-writer.component.html',
  styleUrls: ['./create-writer.component.css']
})
export class CreateWriterComponent implements OnInit {
  private user: any = {
    username: '',
    password: '',
    role: ''
  };

  public role : string;

  public creationError: string = "";

  constructor(private userService : UserService,
    private _snackBar: MatSnackBar) { }

  ngOnInit() {
  }

  public setRole(role:string){
    this.role = role;
  }

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 5000,
    });
  }

  public Create(writerForm: NgForm) {
    this.user.username = writerForm.value.username;
    this.user.password = writerForm.value.password;
    this.user.role = this.role;
    this.userService.CreateUser(this.user)
    .subscribe(
      (response) => {
        this.openSnackBar(response, 'Success!');
      },
      (error) => {
        this.creationError = error.error;
      }
    )
  }

}
