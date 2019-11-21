import { WriterService } from 'src/app/services/writer.service';
import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { MatTableDataSource, MatPaginator, MatSnackBar, MatDialog } from '@angular/material';
import { Writer } from 'src/app/components/interfaces/interfaces.model';
import { ImportElementComponent } from '../import-element/import-element.component';

@Component({
  selector: 'app-writer-management',
  templateUrl: './writer-management.component.html',
  styleUrls: ['./writer-management.component.css']
})
export class WriterManagementComponent implements OnInit {
  displayedColumns: string[];
  dataSource: MatTableDataSource<Writer>;
  writers: Writer[];
  isFriendFilterActivated = false;
  isAdmin = false;

  constructor(private writerService: WriterService,
    private router: Router,
    private _snackBar: MatSnackBar,
    private changeDetectorRefs: ChangeDetectorRef,
    public dialog: MatDialog) { }

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 5000,
    })
  }

  ngOnInit() {
    this.isAdmin = (localStorage.getItem('isAdmin') == 'true');
    this.displayedColumns = this.isAdmin
      ? ['username', 'role', 'action', 'friendFilter', 'import']
      : ['username', 'role', 'friendFilter']
    this.writerService.GetLoggedInWriter()
      .subscribe(
        (response) => {
          var writer = JSON.parse(response);
          this.writerService.SetLoggedInWriter(writer);
          this.getAllWriters();
        },
        (error) => {
          this.openSnackBar(error.error, 'Error!');
        }
      )
  }

  friendFilterManagement() {
    if (!this.isFriendFilterActivated) {
      this.applyFriendFilter();
      this.isFriendFilterActivated = true;
    }
    else {
      this.removeFriendFilter();
      this.isFriendFilterActivated = false;
    }
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  applyFriendFilter() {
    this.dataSource.data = this.dataSource.data.filter(w => w.isFriendsWithUserLoggedIn === true);
    this.dataSource._updateChangeSubscription();
  }

  removeFriendFilter() {
    this.dataSource.data = this.writers;
    this.dataSource._updateChangeSubscription();
  }

  public addFriend(writer: Writer) {
    this.writerService.AddFriend(writer.id)
      .subscribe(
        (response) => {
          this.dataSource.data.find(w => w.id == writer.id).isFriendsWithUserLoggedIn = true;
          this.writers.find(w => w.id == writer.id).isFriendsWithUserLoggedIn = true;
          this.changeDetectorRefs.detectChanges();
        },
        (error) => {
          this.openSnackBar(error.message, 'Error!');
        }
      )
  }

  public removeFriend(writer: Writer) {
    this.writerService.RemoveFriend(writer.id)
      .subscribe(
        (response) => {
          this.dataSource.data.find(w => w.id == writer.id).isFriendsWithUserLoggedIn = false;
          this.writers.find(w => w.id == writer.id).isFriendsWithUserLoggedIn = false;
          this.changeDetectorRefs.detectChanges();
        },
        (error) => {
          this.openSnackBar(error.message, 'Error!');
        }
      )
  }

  private getAllWriters() {
    this.writerService.GetAllWriters()
      .subscribe(
        (response) => {
          var responseParsed = this.writerService.ParseGetAllWritersResponse(response);
          this.writers = responseParsed
          this.dataSource = new MatTableDataSource<Writer>(this.writers);
          this.dataSource.paginator = this.paginator;
        },
        (error) => {
          this.openSnackBar(error.message, 'Error!');
        }
      )
  }

  public deleteWriter(writer: Writer) {
    this.writerService.DeleteWriter(writer.id)
      .subscribe(
        (response) => {
          this.dataSource.data = this.dataSource.data.filter(w => w.id !== writer.id);
          this.writers = this.writers.filter(w => w.id !== writer.id);
          this.dataSource._updateChangeSubscription();
        },
        (error) => {
          this.openSnackBar(error.message, 'Error!');
        }
      )
  }

  public editWriter(writer: Writer) {
    this.router.navigateByUrl('/', { skipLocationChange: true })
      .then(() => this.router.navigate(['/edit-writer'], { state: writer }));
  }

  public openImportDialog(writer: Writer) {
    let dialogRef = this.dialog.open(ImportElementComponent, { data: writer });
    dialogRef.afterClosed().subscribe((res) => {

    }
    );

  }

}