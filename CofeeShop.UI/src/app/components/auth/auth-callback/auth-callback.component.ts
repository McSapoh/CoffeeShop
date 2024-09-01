import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-auth-callback',
  template: '<p>Loading...</p>',
})
export class AuthCallbackComponent implements OnInit {

  constructor(private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const token = params['token'];
      if (token) {
        localStorage.setItem('authToken', token);
        this.router.navigate(['/']);  // Navigate to the desired route after storing the token
      } else {
        // Handle the case where the token is not present
        console.error('Token not found in query parameters');
      }
    });
  }
}
