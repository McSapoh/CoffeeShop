import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-social-auth',
  templateUrl: './social-auth.component.html'
})
export class SocialAuthComponent {
  constructor(private service: AuthService){}
  
  externalLogin(provider: string) {
    this.service.externalLogin(provider)
  }
}
