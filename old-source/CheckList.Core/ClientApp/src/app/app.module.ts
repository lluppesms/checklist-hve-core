import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CollectionsComponent } from './collections/collections.component';
import { ListsComponent } from './lists/lists.component';
import { ActionsComponent } from './actions/action-list.component';
import { ActionAddComponent } from './actions/action-add.component';
import { ActionEditComponent } from './actions/action-edit.component';

import { ModelModule } from "./shared/models/model.module";
import { LoginFormComponent } from './account/login-form/login-form.component';
import { RegistrationFormComponent } from './account/registration-form/registration-form.component';
import { SpinnerComponent } from './spinner/spinner.component';  
//import { AuthService } from './shared/services/auth.service';
//import { SharedService } from './shared/services/sharedservice';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ActionsComponent,
    ActionAddComponent,
    ActionEditComponent,
    CollectionsComponent,
    ListsComponent,
    LoginFormComponent,
    RegistrationFormComponent,
    SpinnerComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ModelModule,
    RouterModule.forRoot([
      { path: '', component: LoginFormComponent, pathMatch: 'full' },
      { path: 'collections', component: CollectionsComponent },
      { path: 'lists/:id', component: ListsComponent },
      { path: 'action/list/:id', component: ActionsComponent },
      { path: 'action/add/:id', component: ActionAddComponent },
      { path: 'action/edit/:id', component: ActionEditComponent },
      { path: 'login', component: LoginFormComponent },
      { path: 'register', component: RegistrationFormComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
