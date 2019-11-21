import { WriterService } from './../../services/writer.service';
import { ElementService } from './../../services/element.service';
import { Component, OnInit, ViewChild, ChangeDetectorRef} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Writer, ElementFlatNode, Element } from 'src/app/components/interfaces/interfaces.model';
import { MatTableDataSource, MatPaginator, MatSnackBar} from '@angular/material';

@Component({
  selector: 'app-share-elements',
  templateUrl: './share-elements.component.html',
  styleUrls: ['./share-elements.component.css']
})
export class ShareElementsComponent implements OnInit {
  node: ElementFlatNode;
  loggedInWriter: Writer;
  writers: Writer[];
  dataSource: MatTableDataSource<Writer>;
  displayedColumns: string[];

  constructor(private activatedroute: ActivatedRoute,
    private elementService: ElementService,
    private writerService: WriterService,
    private _snackBar: MatSnackBar,
    private changeDetectorRefs: ChangeDetectorRef) { }

  ngOnInit() {
    this.displayedColumns = ['username', 'role', 'action'];
    this.activatedroute.data.subscribe(data => {
      this.node = history.state.node;
      this.loggedInWriter = history.state.loggedInWriter;
    });
    this.getAllWriters();
  }

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 5000,
    });
  }

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  private getAllWriters(){
    this.writerService.GetAllWriters()
    .subscribe(
      (response) => {
        var responseParsed = this.writerService.ParseGetAllWritersResponseForSharePage(response, this.node.id);
        this.writers = responseParsed
        this.dataSource =  new MatTableDataSource<Writer>(this.writers);
        this.dataSource.paginator = this.paginator;        
      },
      (error) => {
        this.openSnackBar(error.message, 'Error!');
      }
    )
  }

  shareElement(element: Element){
    this.elementService.ShareElement(this.node.id, element.id, this.node.expandable)
    .subscribe(
      (response) => {
        this.dataSource.data.find(w => w.id == element.id).hasClaimsForElement = true;
        this.writers.find(w => w.id == element.id).hasClaimsForElement = true;
        this.changeDetectorRefs.detectChanges();  
      },
      (error) => {
        this.openSnackBar(error.message, 'Error!');
      }
    )
  }

  stopShareElement(element: Writer){
    this.elementService.StopShareElement(this.node.id, element.id, this.node.expandable)
    .subscribe(
      (response) => {
        this.dataSource.data.find(w => w.id == element.id).hasClaimsForElement = false;
        this.writers.find(w => w.id == element.id).hasClaimsForElement = false;
        this.changeDetectorRefs.detectChanges();  
      },
      (error) => {
        this.openSnackBar(error.message, 'Error!');
      }
    )
  }

}
