import { CommonModule } from '@angular/common'; 
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app-component/app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { LoginComponent } from './components/login/login.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { CreateWriterComponent } from './components/create-writer/create-writer.component';
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
  MatPaginatorModule
} from '@angular/material';

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
  MatPaginatorModule
];

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    LoginComponent,
    ConfirmDialogComponent,
    CreateWriterComponent,
    WriterManagementComponent
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
      { path: 'counter', component: CounterComponent, canActivate: [UserNotLoggedIn] },
      { path: 'create-user', component: CreateWriterComponent, canActivate: [UserNotLoggedIn] },
      { path: 'writer-management', component: WriterManagementComponent, canActivate: [UserNotLoggedIn] }
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
