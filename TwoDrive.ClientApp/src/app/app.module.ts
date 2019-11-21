import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './components/app-component/app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { ElementManagementComponent } from './components/element-management/element-management.component';
import { LoginComponent } from './components/login/login.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { ModifyWriterComponent } from './components/modify-writer/modify-writer.component';
import { WriterManagementComponent } from './components/writer-management/writer-management.component';

import { UserNotLoggedIn } from './guards/UserNotLoggedIn.guard';
import { UserLoggedIn } from './guards/UserLogggedIn.guard';

import { ElementModificationReportComponent } from './components/element-modification-report/element-modification-report.component';
import { TopWritersReportComponent } from './components/top-writers-report/top-writers-report.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
  MatCardModule,
  MatFormFieldModule,
  MatToolbarModule,
  MatButtonModule,
  MatSidenavModule,
  MatIconModule,
  MatListModule,
  MatStepperModule,
  MatInputModule,
  MatDialogModule,
  MatMenuModule,
  MatRadioModule,
  MatTableModule,
  MatPaginatorModule,
  MatTooltipModule,
  MatSnackBarModule,
  MatCheckboxModule,
  MatSortModule,
  MatTreeModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatSelectModule,
  MatButtonToggleModule,
} from '@angular/material';
import { CreateFolderComponent } from './components/create-folder/create-folder.component';
import { AllFilesReportComponent } from './components/all-files-report/all-files-report.component';
import { MoveFolderDialogComponent } from './components/move-folder-dialog/move-folder-dialog.component';
import { FileEditorComponent } from './components/file-editor/file-editor.component';

const MaterialModules = [
  MatCardModule,
  MatFormFieldModule,
  MatToolbarModule,
  MatButtonModule,
  MatSidenavModule,
  MatIconModule,
  MatListModule,
  MatStepperModule,
  MatInputModule,
  MatDialogModule,
  MatMenuModule,
  MatRadioModule,
  MatTableModule,
  MatPaginatorModule,
  MatTooltipModule,
  MatSnackBarModule,
  MatCheckboxModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatTreeModule,
  MatSortModule,
  MatSelectModule,
  MatButtonToggleModule,
];

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    ElementManagementComponent,
    LoginComponent,
    ConfirmDialogComponent,
    ModifyWriterComponent,
    WriterManagementComponent,
    ElementModificationReportComponent,
    TopWritersReportComponent,
    CreateFolderComponent,
    AllFilesReportComponent,
    MoveFolderDialogComponent,
    FileEditorComponent
  ],
  imports: [
    CommonModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: 'login', component: LoginComponent, canActivate: [UserLoggedIn] },
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'home-page', component: ElementManagementComponent, canActivate: [UserNotLoggedIn] },
      { path: 'create-writer', component: ModifyWriterComponent, canActivate: [UserNotLoggedIn], data: { action: 'Create' } },
      { path: 'edit-writer', component: ModifyWriterComponent, canActivate: [UserNotLoggedIn], data: { action: 'Edit' } },
      { path: 'writer-management', component: WriterManagementComponent, canActivate: [UserNotLoggedIn] },
      { path: 'file-modification', component: ElementModificationReportComponent, canActivate: [UserNotLoggedIn], data :{ elementType:'File'} },
      { path: 'folder-modification', component: ElementModificationReportComponent, canActivate: [UserNotLoggedIn], data :{ elementType:'Folder'} },
      { path: 'top-writers', component: TopWritersReportComponent, canActivate: [UserNotLoggedIn] },
      { path: 'all-files-report', component: AllFilesReportComponent, canActivate: [UserNotLoggedIn] }

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
    ConfirmDialogComponent,
    MoveFolderDialogComponent,
    FileEditorComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
