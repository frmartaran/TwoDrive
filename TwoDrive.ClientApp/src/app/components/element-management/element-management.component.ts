import { CreateFolderDialogComponent } from './../create-folder-dialog/create-folder-dialog.component';
import { WriterService } from 'src/app/services/writer.service';
import { FlatTreeControl } from '@angular/cdk/tree';
import { Component } from '@angular/core';
import { MatTreeFlatDataSource, MatTreeFlattener, MatSnackBar, MatMenuTrigger, MatDialog } from '@angular/material';
import { Writer, Element, ElementFlatNode } from 'src/app/components/interfaces/interfaces.model';
import { ElementService } from 'src/app/services/element.service';
import { MoveFolderDialogComponent } from 'src/app/components/move-folder-dialog/move-folder-dialog.component';
import { ShowContentComponent } from 'src/app/components//show-content/show-content.component';
import { Router } from '@angular/router';

@Component({
  selector: 'element-management',
  templateUrl: './element-management.component.html',
  styleUrls: ['./element-management.component.css']
})
export class ElementManagementComponent {
  writer: Writer;
  elements: Element[];
  matMenuData: ElementFlatNode;

  private _transformer = (node: Element, level: number) => {
    return {
      expandable: node.isFolder,
      name: node.ownerId != this.writer.id && level == 0
        ? node.ownerName + '/' + node.name
        : node.name,
      level: level,
      id: node.id,
      hasChildrenLoaded: this.FolderHasChildrenLoaded(node),
      isChildFromLoggedInWriter: this.writer.id == node.ownerId,
      ownerId: node.ownerId,
      content: node.content,
      shouldRender: node.shouldRender != null
        ? node.shouldRender
        : false
    };
  }

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 5000,
    });
  }

  treeControl = new FlatTreeControl<ElementFlatNode>(
    node => node.level, node => node.expandable);

  treeFlattener = new MatTreeFlattener(
    this._transformer, node => node.level, node => node.expandable, node => node.folderChildren);

  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  constructor(private writerService : WriterService,
    private router: Router,
    private elementService: ElementService,
    private _snackBar: MatSnackBar,
    public dialog: MatDialog) {
    this.writerService.GetLoggedInWriter()
      .subscribe(
        (response) => {
          this.writer = JSON.parse(response);
          this.elements = this.writerService.GetElementsFromWriter(this.writer);
          this.dataSource.data = this.elements;
        },
        (error) => {
          this.openSnackBar(error.error, 'Error!');
        }
      )
  }

  hasChild = (_: number, node: ElementFlatNode) => node.expandable;

  getAllChildren(node: ElementFlatNode) {
    if (!node.hasChildrenLoaded) {
      var index = this.elements.findIndex(e => e.id == node.id);
      var elementToUpdate = this.elements[index];
      this.elementService.GetFolder(elementToUpdate.id)
      .subscribe(
        (response) => {
          var elementUpdated = JSON.parse(response);
          this.elements[index] = elementUpdated;
          this.dataSource.data = this.elements;
          var nodeToExpand = this.treeControl.dataNodes.find(n => n.id == node.id);
          this.treeControl.expand(nodeToExpand);
        },
        (error) => {
          this.openSnackBar(error.error, 'Error!');
        }
      )
    }
  }

  private FolderHasChildrenLoaded(element: Element) {
    return element.isFolder && element.folderChildren != null && element.folderChildren.length > 0
  }

  openMenu(event: MouseEvent, node: ElementFlatNode, viewChild: MatMenuTrigger) {
    if (node.isChildFromLoggedInWriter || !node.expandable) {
      event.preventDefault();
      this.matMenuData = node;
      viewChild.openMenu();
    }
  }

  openShowContentPopup(){
    let dialogRef = this.dialog.open(ShowContentComponent, {
      data: {
        node: this.matMenuData
      }
    });
  }

  openMoveFolderDialog() {
    let dialogRef = this.dialog.open(MoveFolderDialogComponent);
    dialogRef.afterClosed().subscribe(res => {
      if (res) {
        var writerRoot = this.elements.find(e => e.ownerId == this.writer.id);
        var elementDestination = this.elementService.GetElementFromPath(res, writerRoot);
        if (elementDestination == null) {
          this.openSnackBar('Folder not found, please enter a correct path', 'Error!');
        }
        else if(elementDestination.folderChildren.find(e => e.id == this.matMenuData.id) != null){
          this.openSnackBar('Can\'t move child to current destination.', 'Error!');
        }
        else{
          if(this.matMenuData.expandable){
            this.MoveFolder(this.matMenuData.id, elementDestination.id);
          }else{
            this.MoveFile(this.matMenuData.id, elementDestination.id);
          }
        }
      }
    });
  }

  addFolder(){
    let dialogRef = this.dialog.open(CreateFolderDialogComponent);
    dialogRef.afterClosed().subscribe(res => {
      if (res) {
        this.elementService.AddFolder(this.matMenuData.ownerId, this.matMenuData.id, res)
        .subscribe(
          (response) => {
            var rootUpdated = JSON.parse(response);
            this.elements = this.writerService.GetElementsFromWriter(rootUpdated);
            this.dataSource.data = this.elements;
            this.openSnackBar("Folder has been created!", 'Success!');
          },
          (error) => {
            this.openSnackBar(error, 'Error!');
          }
        )
      }
    });
  }

  private MoveFolder(elementToMoveId: number, elementDestinationId: number){
    this.elementService.MoveFolder(elementToMoveId, elementDestinationId)
    .subscribe(
      (response) => {
        var rootUpdated = JSON.parse(response);
        this.elements = this.writerService.GetElementsFromWriter(rootUpdated);
        this.dataSource.data = this.elements;
        this.openSnackBar("Folder has been moved!", 'Success!');
      },
      (error) => {
        this.openSnackBar(error, 'Error!');
      }
    )
  }

  private MoveFile(elementToMoveId: number, elementDestinationId: number){
    this.elementService.MoveFile(elementToMoveId, elementDestinationId)
    .subscribe(
      (response) => {
        var rootUpdated = JSON.parse(response);
        this.elements = this.writerService.GetElementsFromWriter(rootUpdated);
        this.dataSource.data = this.elements;
        this.openSnackBar("File has been moved!", 'Success!');
      },
      (error) => {
        this.openSnackBar(error, 'Error!');
      }
    )
  }

  public shareElement(){
    var shareData: any = {
      loggedInWriter: this.writer,
      node: this.matMenuData
    }
    this.router.navigate(['/share'], {state: shareData})
  }

  delete() {
    var id = this.matMenuData.id;
    var explandable = this.matMenuData.expandable;
    this.elementService.Delete(id, explandable)
      .subscribe((res) => {
        var rootUpdated = JSON.parse(res);
        this.elements = this.writerService.GetElementsFromWriter(rootUpdated);
        this.dataSource.data = this.elements
        this.openSnackBar('Element has been deleted!', 'Success!');
      },
      (error) => {
        this.openSnackBar(error.message, 'Error!');
      }
      );
  }
}
