import { Component, OnInit, ViewChild } from '@angular/core';
import { ElementService } from 'src/app/services/element.service';
import { MatTableDataSource, MatPaginator, MatSnackBar, MatSort, MatDialog } from '@angular/material';
import { AllFilesReport } from 'src/app/components/interfaces/interfaces.model';
import { Router } from '@angular/router';
import { FileEditorComponent } from '../file-editor/file-editor.component';

@Component({
  selector: 'app-all-files-report',
  templateUrl: './all-files-report.component.html',
  styleUrls: ['./all-files-report.component.css']
})
export class AllFilesReportComponent implements OnInit {
  displayedColumns: string[] = ['name', 'creationDate', 'dateModified', 'ownerName', 'action'];
  dataSource: MatTableDataSource<AllFilesReport>;
  elementType: string;
  allFiles: AllFilesReport[];

  constructor(private elementService: ElementService,
    public dialog: MatDialog,
    private _snackBar: MatSnackBar,
    private router: Router) { }

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 5000,
    });
  }

  ngOnInit() {
    this.elementService.GetAllFiles()
      .subscribe(
        (response) => {
          var responseString = JSON.stringify(response);
          this.allFiles = JSON.parse(responseString);
          this.dataSource = new MatTableDataSource<AllFilesReport>(this.allFiles);
          this.dataSource.sort = this.sort;
          this.dataSource.paginator = this.paginator;
        },
        (error) => {
          this.openSnackBar(error.error, 'Error!');
        }
      )
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  openEditDialog(element: Element) {
    var ref = this.dialog.open(FileEditorComponent, { data: element });
    ref.afterClosed().subscribe(res => {
      if (res) {

      }
    });
  }

}
