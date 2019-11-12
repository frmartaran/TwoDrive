import { WriterService } from 'src/app/services/writer.service';
import { Component, OnInit, ViewChild} from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSnackBar} from '@angular/material';

export interface Writer {
  id: number;
  role: string;
  userName: string;
  friends: [];
  claims: [];
}

@Component({
  selector: 'app-writer-management',
  templateUrl: './writer-management.component.html',
  styleUrls: ['./writer-management.component.css']
})
export class WriterManagementComponent implements OnInit {
  displayedColumns: string[] = ['username', 'role', 'action'];
  dataSource: MatTableDataSource<Writer>;
  writers: [];
  writerLoggedInId: number;

  constructor(private writerService : WriterService,
    private _snackBar: MatSnackBar){}

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 5000,
    });
  }

  ngOnInit() {
    this.writerService.GetAllWriters()
    .subscribe(
      (response) => {
        var responseString = JSON.stringify(response);
        var responseParsed = this.writerService.ParseGetAllWritersResponse(responseString);
        this.writers = responseParsed
        this.dataSource =  new MatTableDataSource<Writer>(this.writers);
        this.dataSource.paginator = this.paginator;        
      },
      (error) => {
        this.openSnackBar(error, 'Error!');
      }
    )
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

}