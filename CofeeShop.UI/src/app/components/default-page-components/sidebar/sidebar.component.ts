import { Component } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent {
  private jwtHelper: JwtHelperService
  public name: string | null = ''
  public profileImageUrl: string | null = ''
  defaultImageUrl = environment.apiUrl + '/images'

  constructor() {
    this.jwtHelper = new JwtHelperService()

    this.name = localStorage.getItem('username')
    this.profileImageUrl = localStorage.getItem('profileImageUrl')
  }
}
