import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { ImportService } from 'src/app/services/import.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-import-element',
  templateUrl: './import-element.component.html',
  styleUrls: ['./import-element.component.css']
})
export class ImportElementComponent implements OnInit {

  path: string;
  writerName: string;
  writerId: string;
  importers: string[] = [];
  errorMessage: string = '';
  successMessage: string = '';

  constructor(public dialogRef: MatDialogRef<ImportElementComponent>,
    private _snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private importService: ImportService) {
    this.writerId = data.id;
    this.writerName = data.userName;
  }

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 5000,
    });
  }

  ngOnInit() {

    this.importService.getImporters()
      .subscribe((res) => {
        var asString = JSON.stringify(res);
        var importers = JSON.parse(asString);
        importers.forEach(element => {
          this.importers.push(element.Name);
        });;

      });
  }

  public Import(importForm: NgForm) {
    var id = Number.parseInt(this.writerId);
    var importer = importForm.value.importer;
    var path = importForm.value.path;
    this.importService.import(id, importer, path)
      .subscribe((res) => {
          this.successMessage = JSON.parse(res);
          this.openSnackBar('Element imported sucessfully!', 'Error!');
      },
        (error) => {
          this.errorMessage = error.message;
          this.openSnackBar(error, 'Error!');
        }
      );
  }

}
