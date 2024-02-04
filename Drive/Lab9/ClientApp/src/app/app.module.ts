import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {RouterModule} from '@angular/router';

import {AppComponent} from './app.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {HomeComponent} from './home/home.component';
import {CounterComponent} from './counter/counter.component';
import {FetchDataComponent} from './fetch-data/fetch-data.component';
import {RegisterComponent} from './reactive-forms/register/register.component';
import {LoginComponent} from './reactive-forms/login/login.component';
import {ErrorInterceptor} from "./core/interceptors/error.interceptor";
import {AuthGuard} from "./core/guards/auth.guard";
import {ParentComponent} from "./components/parent/parent.component";
import {ChildComponent} from "./components/child/child.component";
import { AuthenticationService } from './core/services/authentication.service';
import {CommonModule} from "@angular/common";
import { HttpClientModule } from '@angular/common/http';
import { JwtModule } from "@auth0/angular-jwt";
import { EntriesComponent } from "./components/entries/entries.component";

export function tokenGetter() {
  return localStorage.getItem("jwt");
}

@NgModule({
  declarations: [
    AppComponent,
    ChildComponent,
    ParentComponent,
    FetchDataComponent,
    CounterComponent,
    HomeComponent,
    NavMenuComponent,
    EntriesComponent,
  ],
  exports: [ChildComponent],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:44473"],
      },
    }),
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'login', component: LoginComponent },
      { path: 'routing', canActivate: [AuthGuard], component: ParentComponent },
      { path: 'storage/:path', component: EntriesComponent}
    ])
  ],
  providers: [AuthenticationService],
  bootstrap: [AppComponent]
})
export class AppModule {
}
