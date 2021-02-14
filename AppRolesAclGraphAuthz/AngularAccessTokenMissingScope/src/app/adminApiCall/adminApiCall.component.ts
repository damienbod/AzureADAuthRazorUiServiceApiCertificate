import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-admin-api-call',
  templateUrl: 'adminApiCall.component.html',
})
export class AdminApiCallComponent implements OnInit {
  userData$: Observable<any>;
  dataFromAzureProtectedApi$: Observable<any>;
  isAuthenticated$: Observable<boolean>;
  httpRequestRunning = false;
  constructor(
    private authService: AuthService,
    private httpClient: HttpClient
  ) {}

  ngOnInit() {
    this.userData$ = this.authService.userData$;
    this.isAuthenticated$ = this.authService.signedIn$;
  }
  callApi() {
    this.httpRequestRunning = true;
    this.dataFromAzureProtectedApi$ = this.httpClient
      .get('https://localhost:44323/api/adminData')
      .pipe(finalize(() => (this.httpRequestRunning = false)));
  }
}
