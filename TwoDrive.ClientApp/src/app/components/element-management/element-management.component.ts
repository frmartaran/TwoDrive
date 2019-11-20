import { WriterService } from 'src/app/services/writer.service';
import { FlatTreeControl} from '@angular/cdk/tree';
import { Component } from '@angular/core';
import { MatTreeFlatDataSource, MatTreeFlattener} from '@angular/material/tree';
import { Writer, Element, ElementFlatNode } from 'src/app/components/interfaces/interfaces.model';
import { ElementService } from 'src/app/services/element.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'element-management',
  templateUrl: './element-management.component.html',
  styleUrls: ['./element-management.component.css']
})
export class ElementManagementComponent{
  writer: Writer;
  elements: Element[];
  isLazyLoaded$: Observable<boolean>; 

  private _transformer = (node: Element, level: number) => {
    return {
      expandable: node.isFolder,
      name: node.id != this.writer.id && level == 0
        ? node.ownerName + '/' + node.name
        : node.name,
      level: level,
      id: node.id,
      hasChildrenLoaded: this.FolderHasChildrenLoaded(node)
    };
  }

  treeControl = new FlatTreeControl<ElementFlatNode>(
      node => node.level, node => node.expandable);

  treeFlattener = new MatTreeFlattener(
      this._transformer, node => node.level, node => node.expandable, node => node.folderChildren);

  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  constructor(private writerService : WriterService,
    private elementService: ElementService) {
    this.writerService.GetLoggedInWriter()
    .subscribe(
      (response) => {
        var responseString = JSON.stringify(response);
        this.writer = JSON.parse(responseString);
        this.elements = this.writerService.GetElementsFromWriter(this.writer);
        this.dataSource.data = this.elements;
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
       }
      )
    }
  }

  private FolderHasChildrenLoaded(element: Element){
    return element.isFolder && element.folderChildren != null && element.folderChildren.length > 0   
  }
}
