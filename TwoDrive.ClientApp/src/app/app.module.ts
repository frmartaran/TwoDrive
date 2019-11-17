import { CommonModule } from '@angular/common'; 
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app-component/app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './components/login/login.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { ModifyWriterComponent } from './components/modify-writer/modify-writer.component';
import { WriterManagementComponent } from './components/writer-management/writer-management.component';

import { UserNotLoggedIn } from './guards/UserNotLoggedIn.guard';
import { UserLoggedIn } from './guards/UserLogggedIn.guard';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { 
  MatCardModule,
  MatFormFieldModule,
  MatToolbarModule, 
  MatButtonModule,
  MatSidenavModule,
  MatIconModule,
  MatListModule ,
  MatStepperModule,
  MatInputModule,
  MatDialogModule,
  MatMenuModule,
  MatRadioModule,
  MatTableModule,
  MatPaginatorModule,
  MatTooltipModule,
  MatSnackBarModule,
  MatCheckboxModule
} from '@angular/material';
import { ElementReportComponent } from './components/element-report/element-report.component';
import { TopWritersReportComponent } from './components/top-writers-report/top-writers-report.component';

const MaterialModules = [
  MatCardModule,
  MatFormFieldModule,
  MatToolbarModule, 
  MatButtonModule,
  MatSidenavModule,
  MatIconModule,
  MatListModule ,
  MatStepperModule,
  MatInputModule,
  MatDialogModule,
  MatMenuModule,
  MatRadioModule,
  MatTableModule,
  MatPaginatorModule,
  MatTooltipModule,
  MatSnackBarModule,
  MatCheckboxModule
];

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    ConfirmDialogComponent,
    ModifyWriterComponent,
    WriterManagementComponent,
    ElementReportComponent,
    TopWritersReportComponent
  ],
  imports: [
    CommonModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: 'login', component: LoginComponent, canActivate : [UserLoggedIn] },
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'home-page', component: HomeComponent, canActivate: [UserNotLoggedIn] },
      { path: 'create-writer', component: ModifyWriterComponent, canActivate: [UserNotLoggedIn], data :{ action:'Create'} },
      { path: 'edit-writer', component: ModifyWriterComponent, canActivate: [UserNotLoggedIn], data :{ action:'Edit'}},
      { path: 'writer-management', component: WriterManagementComponent, canActivate: [UserNotLoggedIn] },
      { path: 'file-modification', component: ElementReportComponent, canActivate: [UserNotLoggedIn], data :{ elementType:'File'} },
      { path: 'folder-modification', component: ElementReportComponent, canActivate: [UserNotLoggedIn], data :{ elementType:'Folder'} },
      { path: 'top-writers', component: TopWritersReportComponent, canActivate: [UserNotLoggedIn] }
    ]),
    BrowserAnimationsModule,
    MaterialModules
  ],
  providers: 
  [
    UserLoggedIn,
    UserNotLoggedIn,
  ],
  entryComponents: [
    ConfirmDialogComponent    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
