import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Inject } from '@angular/core';
import { ElementFlatNode } from 'src/app/components/interfaces/interfaces.model';

@Component({
  selector: 'app-show-content',
  templateUrl: './show-content.component.html',
  styleUrls: ['./show-content.component.css']
})
export class ShowContentComponent implements OnInit {
  public node: ElementFlatNode;

  constructor(public dialogRef: MatDialogRef<ShowContentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
    ) {
      this.node = data.node;
    }

  ngOnInit() {
  }

}
