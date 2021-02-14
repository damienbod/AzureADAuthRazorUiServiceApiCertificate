import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-student-api-call',
  templateUrl: 'studentApiCall.component.html',
})
export class StudentApiCallComponent implements OnInit {
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
      .get('https://localhost:44323/api/studentData')
      .pipe(finalize(() => (this.httpRequestRunning = false)));
  }
}
