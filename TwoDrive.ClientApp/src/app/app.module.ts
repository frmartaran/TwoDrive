import { CommonModule } from '@angular/common'; 
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { LoginComponent } from './components/login/login.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';

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
  MatMenuModule
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
  MatMenuModule
];

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    LoginComponent,
    ConfirmDialogComponent
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
    ]),
    BrowserAnimationsModule,
    MaterialModules
  ],
  providers: 
  [
    UserLoggedIn,
    UserNotLoggedIn,
  ],
  entryComponents: [ConfirmDialogComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
