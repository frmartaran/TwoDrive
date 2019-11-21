import { ActivatedRoute } from '@angular/router';
import { ReportService } from 'src/app/services/report.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSnackBar, MatDatepickerInputEvent } from '@angular/material';
import { ModificationReport } from 'src/app/components/interfaces/interfaces.model';

@Component({
  selector: 'app-element-modification-report',
  templateUrl: './element-modification-report.component.html',
  styleUrls: ['./element-modification-report.component.css']
})
export class ElementModificationReportComponent implements OnInit {

  displayedColumns: string[] = ['owner', 'amount'];
  dataSource: MatTableDataSource<ModificationReport>;
  elementType: string;
  modficationReport: ModificationReport[];
  startDate: Date;
  endDate: Date;
  public errorMessage: string = "";

  constructor(private reportService: ReportService,
    private activatedroute: ActivatedRoute,
    private _snackBar: MatSnackBar) { }

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 5000,
    });
  }

  ngOnInit() {
    this.activatedroute.data.subscribe(data => {
      this.elementType = data.elementType;
    });
    this.dataSource = new MatTableDataSource<ModificationReport>([]);
    this.dataSource.paginator = this.paginator;
  }

  public LoadTable(dateType: string, event: MatDatepickerInputEvent<Date>) {
    if (dateType == 'start') {
      this.startDate = event.value;
    } else if (dateType == 'end') {
      this.endDate = event.value;
    }
    if (this.startDate != null && this.endDate != null)
      this.reportService.GetModificationsReport(this.elementType, this.startDate, this.endDate)
        .subscribe((response) => {
          var responseString = JSON.stringify(response);
          this.modficationReport = JSON.parse(responseString);
          this.dataSource = new MatTableDataSource<ModificationReport>(this.modficationReport);
          this.dataSource.paginator = this.paginator;
        },
          (error) => {
            this.errorMessage = error.error
          });
  }

}


