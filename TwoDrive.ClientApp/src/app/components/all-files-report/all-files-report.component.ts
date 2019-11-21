import { Component, OnInit, ViewChild } from '@angular/core';
import { ElementService } from 'src/app/services/element.service';
import { MatTableDataSource, MatPaginator, MatSnackBar, MatSort } from '@angular/material';
import { AllFilesReport } from 'src/app/components/interfaces/interfaces.model';

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
    private _snackBar: MatSnackBar) { }

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

  public delete(element: Element) {
    var id = Number.parseInt(element.id);
    this.elementService.DeleteFile(id)
      .subscribe( (res) => {
        var id = Number.parseInt(element.id);
        this.dataSource.data = this.dataSource.data.filter(f => f.id != id);
        this.allFiles.filter(f => f.id != id);
        this.dataSource._updateChangeSubscription();
      }, 
      (error) => {
        this.openSnackBar(error.message, 'Error!');

      }
      );
  }

}
