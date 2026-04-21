import { Component, OnInit } from '@angular/core';
//import { HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
//import { environment } from '../environments/environment';
import { AuthService } from "../app/shared/services/auth.service";
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
  public appIsLoggedIn = false;
  public appLoggedInUserName = "";

  //_hubConnection: HubConnection;
  // msgs: Message[] = [];

  constructor(private auth: AuthService, private router: Router) { }

  ngOnInit(): void {

    this.router.events.subscribe(event => {
      if (event.constructor.name === "NavigationEnd") {
        // --> trying to update the nav bar with the user name, but it's not refreshing...
        this.appIsLoggedIn = this.auth.isLoggedIn;
        this.appLoggedInUserName = this.auth.loggedInUserName;
      }
    })
    //// --> tried dozens of ways but each time I'm blocked by CORS error or /Chat/negotiate is 404...

    ////var baseSignal = "https://localhost:5001";
    ////var baseSignal = "https://localhost:44381/";
    //var baseSignal = "https://localhost:44376/";
    ////var apiUrl = environment.apiUrl + (environment.apiUrl.endsWith("/") ? "" : "/") + "hub/action";
    ////var apiUrl = baseSignal + (baseSignal.endsWith("/") ? "" : "/") + "hub";
    //var apiUrl = baseSignal + (baseSignal.endsWith("/") ? "" : "/") + "chat";
    ////var apiUrl = "/chat";

    //console.log("Connecting to SignalR on {0}", {apiUrl});
    //this._hubConnection = new HubConnectionBuilder()
    //  .configureLogging(LogLevel.Information)
    //  .withUrl(apiUrl)
    //  .build();
    //this._hubConnection
    //  .start()
    //  .then(() =>
    //     console.log('SignalR Connection started!')
    //  )
    //  .catch(err =>
    //    console.log('Error while establishing SignalR connection :( ' + err)
    //  );

    //this._hubConnection.on('BroadcastMessage', (type: string, payload: string) => {
    //  // this.msgs.push({ severity: type, summary: payload });
    //  console.log({ severity: type, summary: payload });
    //});
  }
}
