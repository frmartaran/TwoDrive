import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-move-folder-dialog',
  templateUrl: './move-folder-dialog.component.html',
  styleUrls: ['./move-folder-dialog.component.css']
})
export class MoveFolderDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<MoveFolderDialogComponent>) {}

  path: string

  ngOnInit() {
  }

}
