import { Component } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent {
  public name: string | null = ''
  public profileImageUrl: string | null = ''

  constructor() {
    this.name = localStorage.getItem('username')
    this.profileImageUrl = localStorage.getItem('profileImageUrl')
  }
}
