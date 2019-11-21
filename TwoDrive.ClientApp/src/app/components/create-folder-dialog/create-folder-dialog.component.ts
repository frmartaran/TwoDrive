import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-create-folder-dialog',
  templateUrl: './create-folder-dialog.component.html',
  styleUrls: ['./create-folder-dialog.component.css']
})
export class CreateFolderDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<CreateFolderDialogComponent>) {}

  folderName: string;

  ngOnInit() {}

}

