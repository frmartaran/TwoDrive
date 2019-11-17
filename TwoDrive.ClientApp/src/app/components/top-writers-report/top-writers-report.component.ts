import { ReportService } from 'src/app/services/report.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSnackBar} from '@angular/material';
import { TopWritersReport } from 'src/app/components/interfaces/interfaces.model';

@Component({
  selector: 'app-top-writers-report',
  templateUrl: './top-writers-report.component.html',
  styleUrls: ['./top-writers-report.component.css']
})
export class TopWritersReportComponent implements OnInit {

  displayedColumns: string[] = ['username', 'fileCount'];
  dataSource: MatTableDataSource<TopWritersReport>;
  elementType: string;
  topWriters: TopWritersReport[];

  constructor(private reportService : ReportService,
    private _snackBar: MatSnackBar){}

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 5000,
    });
  }

  ngOnInit() {
    this.reportService.GetTopWriters()
    .subscribe(
      (response) => {
        var responseString = JSON.stringify(response);
        this.topWriters = JSON.parse(responseString);
        this.dataSource =  new MatTableDataSource<TopWritersReport>(this.topWriters);
        this.dataSource.paginator = this.paginator;        
      },
      (error) => {
        this.openSnackBar(error.error, 'Error!');
      }
    )
  }
}