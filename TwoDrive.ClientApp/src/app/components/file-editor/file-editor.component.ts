import { Component, Inject } from '@angular/core';
import { MatSnackBar, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ElementService } from 'src/app/services/element.service';
import { Element } from 'src/app/components/interfaces/interfaces.model';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-file-editor',
  templateUrl: './file-editor.component.html',
  styleUrls: ['./file-editor.component.css']
})
export class FileEditorComponent {
  
  public file: Element = {
    folderChildren: null,
    id: 0,
    name: '',
    parentFolderId: 0,
    ownerId: 0,
    creationDate: null,
    dateModified: null,
    isFolder: false,
    ownerName: "",
    path: "",
    content: '',
    shouldRender: false,
    Type: ''
  };
  action: string;

  constructor(private elementService: ElementService,
    private _snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<FileEditorComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
        this.file.id = data.id;
        this.file.content = data.content;
        this.file.shouldRender = data.shouldRender;
     }

    onDismiss(): void {
      this.dialogRef.close();
    }

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 5000,
    });
  }

  saveElement(dataForm: NgForm){
    this.file.content = dataForm.value.content;
    this.file.shouldRender = dataForm.value.render;
    this.dialogRef.close(this.file);
  }

  isHTML(){
    if(this.file.shouldRender != null && this.file.shouldRender != undefined){
      this.file.Type = "HTML";
      return true;
    }else{
      this.file.Type = "TXT";
      return false;
    }
  }

}

