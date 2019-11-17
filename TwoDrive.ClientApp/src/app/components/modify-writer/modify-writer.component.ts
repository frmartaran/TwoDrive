import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';
import { WriterService } from 'src/app/services/writer.service';
import { Component, OnInit } from '@angular/core';
import {MatSnackBar} from '@angular/material/snack-bar';
import { Writer } from 'src/app/components/interfaces/interfaces.model';

@Component({
  selector: 'app-modify-writer',
  templateUrl: './modify-writer.component.html',
  styleUrls: ['./modify-writer.component.css']
})
export class ModifyWriterComponent implements OnInit {
  public writer: Writer = {
    id: 0,
    role: '',
    userName: '',
    password: '',
    friends: null,
    claims: null,
    isFriendsWithUserLoggedIn: false
  };

  public action : string; 

  public role : string;

  public creationError: string = "";

  constructor(private writerService : WriterService,
    private activatedroute : ActivatedRoute,
    private _snackBar: MatSnackBar) { }

  ngOnInit() {
    this.activatedroute.data.subscribe(data => {
      this.action = data.action;
    });
    this.writer.id = history.state.id == null
    ? ''
    : history.state.id;
    this.writer.userName = history.state.userName == null
      ? ''
      : history.state.userName;
    this.writer.password = history.state.password == null
      ? ''
      : history.state.password
    this.writer.role = history.state.role == null
      ? ''
      : history.state.role;

    this.role = this.writer.role;
  }

  public setRole(role:string){
    this.role = role;
  }

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 5000,
    });
  }

  public Submit(writerForm: NgForm){
    if(this.action == 'Create'){
      this.Create(writerForm);
    }
    else{
      this.Edit(writerForm)
    }
  }

  public Create(writerForm: NgForm) {
    this.writer.userName = writerForm.value.userName;
    this.writer.password = writerForm.value.password;
    this.writer.role = this.role;
    this.writerService.CreateWriter(this.writer)
    .subscribe(
      (response) => {
        this.openSnackBar(response, 'Success!');
      },
      (error) => {
        this.creationError = error.error;
      }
    )
  }

  public Edit(writerForm: NgForm) {
    this.writer.userName = writerForm.value.userName;
    this.writer.password = writerForm.value.password;
    this.writer.role = this.role;
    this.writerService.UpdateWriter(this.writer)
    .subscribe(
      (response) => {
        this.openSnackBar(this.writer.userName + ' has been edited.', 'Success!');
      },
      (error) => {
        this.creationError = error.error;
      }
    )
  }

}
