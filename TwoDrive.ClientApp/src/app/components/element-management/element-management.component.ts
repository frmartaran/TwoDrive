import { WriterService } from 'src/app/services/writer.service';
import { FlatTreeControl} from '@angular/cdk/tree';
import { Component } from '@angular/core';
import { MatTreeFlatDataSource, MatTreeFlattener, MatSnackBar, MatMenuTrigger, MatDialog} from '@angular/material';
import { Writer, Element, ElementFlatNode } from 'src/app/components/interfaces/interfaces.model';
import { ElementService } from 'src/app/services/element.service';
import { MoveFolderDialogComponent } from 'src/app/components/move-folder-dialog/move-folder-dialog.component';

@Component({
  selector: 'element-management',
  templateUrl: './element-management.component.html',
  styleUrls: ['./element-management.component.css']
})
export class ElementManagementComponent{
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
      isChildFromLoggedInWriter: this.writer.id == node.ownerId
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
    private elementService: ElementService,
    private _snackBar: MatSnackBar,
    public dialog: MatDialog) {
    this.writerService.GetLoggedInWriter()
    .subscribe(
      (response) => {
        var responseString = JSON.stringify(response);
        this.writer = JSON.parse(responseString);
        this.elements = this.writerService.GetElementsFromWriter(this.writer);
        this.dataSource.data = this.elements;
      },
      (error) => {
        this.openSnackBar(error.error, 'Error!');
      }
    )
  }

  hasChild = (_: number, node: ElementFlatNode) => node.expandable;

  getAllChildren(node: ElementFlatNode){
    if(!node.hasChildrenLoaded){
      var index = this.elements.findIndex(e => e.id == node.id);
      var elementToUpdate = this.elements[index];
      this.elementService.GetFolder(elementToUpdate.id)
      .subscribe(
        (response) => {
          var responseString = JSON.stringify(response);
          var elementUpdated = JSON.parse(responseString);
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

  private FolderHasChildrenLoaded(element: Element){
    return element.isFolder && element.folderChildren != null && element.folderChildren.length > 0   
  }

  openMenu(event: MouseEvent, node: ElementFlatNode, viewChild: MatMenuTrigger) {
    if(node.isChildFromLoggedInWriter || !node.expandable){
      event.preventDefault();
      this.matMenuData = node;
      viewChild.openMenu();
    }
  }

  openMoveFolderDialog() {
    let dialogRef = this.dialog.open(MoveFolderDialogComponent);
    dialogRef.afterClosed().subscribe(res => {
      if (res) {
        var elementToMove = this.elements.find(e => e.id == this.matMenuData.id);
        var elementDestinationIndex = this.elements.findIndex(e => e.path === res);
        var elementDestination = this.elements[elementDestinationIndex];
        if(elementDestination == null){
          this.openSnackBar('Folder not found, please enter a correct path', 'Error!');
        }
        else{
          this.elementService.MoveFolder(this.matMenuData.id, elementDestination.id)
          .subscribe(
            (response) => {
              var oldParentIndex = this.elements.findIndex(e => e.id == elementToMove.parentFolderId);
              this.elements[oldParentIndex].folderChildren = this.elements[oldParentIndex].folderChildren.filter(c => c.id == elementToMove.id);
              this.elements[elementDestinationIndex].folderChildren.push(elementToMove);
              this.dataSource.data = this.elements;
              this.openSnackBar(response, 'Success!');
            },
            (error) => {
              this.openSnackBar(error, 'Error!');
            }
          )
        }
      }
    });
  }
}
