import { WriterService } from 'src/app/services/writer.service';
import {FlatTreeControl} from '@angular/cdk/tree';
import { Component, OnInit } from '@angular/core';
import {MatTreeFlatDataSource, MatTreeFlattener} from '@angular/material/tree';
import { Writer, Element, ElementFlatNode } from 'src/app/components/interfaces/interfaces.model';

@Component({
  selector: 'element-management',
  templateUrl: './element-management.component.html',
  styleUrls: ['./element-management.component.css']
})
export class ElementManagementComponent{
  writer: Writer;
  elements: Element[];

  private _transformer = (node: Element, level: number) => {
    return {
      expandable: node.isFolder,
      name: node.name,
      level: level,
    };
  }

  treeControl = new FlatTreeControl<ElementFlatNode>(
      node => node.level, node => node.expandable);

  treeFlattener = new MatTreeFlattener(
      this._transformer, node => node.level, node => node.expandable, node => node.folderChildren);

  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  constructor(private writerService : WriterService) {
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
}
