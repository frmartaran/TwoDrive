import {Component, OnInit, ViewChild} from '@angular/core';
import {MatTableDataSource, MatPaginator} from '@angular/material';

export interface WriterData {
  username: string;
  role: string;
}

const writerData: WriterData[] = [
  {username: 'Hydrogen', role: 'Hasdasdasdasd'},
  {username: 'Helium', role: 'Heasdasdasd'},
  {username: 'Lithium', role: 'Liasdasdasd'},
  {username: 'Beryllium', role: 'Beasdasdasd'},
  {username: 'Boron', role: 'Basdasdasdas'},
  {username: 'Carbon', role: 'Casdasdasd'},
  {username: 'Nitrogen', role: 'Nasdasdasd'},
  {username: 'Oxygen', role: 'Oasdasdasd'},
  {username: 'Fluorine', role: 'Fasdasdasd'},
  {username: 'Neon', role: 'Neasdasdasdasd'},
  {username: 'Sodium', role: 'Naasdasdasd'},
  {username: 'Magnesium', role: 'Mgasdasdasd'},
  {username: 'Aluminum', role: 'Alasdasdasd'},
  {username: 'Silicon', role: 'Siasdasdasd'},
  {username: 'Phosphorus', role: 'Pasdasdasd'},
  {username: 'Sulfur', role: 'Sasdasdasd'},
  {username: 'Chlorine', role: 'Clasdasdasd'},
  {username: 'Argon', role: 'Arasdasdasd'},
  {username: 'Potassium', role: 'Kasdasdasd'},
  {username: 'Calcium', role: 'Caasdasdads'},
];

@Component({
  selector: 'app-writer-management',
  templateUrl: './writer-management.component.html',
  styleUrls: ['./writer-management.component.css']
})
export class WriterManagementComponent implements OnInit {
  displayedColumns: string[] = ['username', 'role', 'action'];
  dataSource: MatTableDataSource<WriterData>;

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  ngOnInit() {
    this.dataSource =  new MatTableDataSource<WriterData>(writerData);
    this.dataSource.paginator = this.paginator;
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
}