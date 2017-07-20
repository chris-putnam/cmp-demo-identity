import { Component } from '@angular/core';
import { UserManager, User } from 'oidc-client';

const settings: any = {
  authority: 'https://localhost:44366/',
  client_id: 'spa',
  redirect_uri: 'http://localhost:4200/',
  post_logout_redirect_uri: 'http://localhost:4200/',
  response_type: 'id_token token',
  scope: 'openid email roles',
  silent_redirect_uri: 'http://localhost:4200/silent-renew.html',
  automaticSilentRenew: true,
  accessTokenExpiringNotificationTime: 4,
  // silentRequestTimeout:10000,
  filterProtocolClaims: true,
  loadUserInfo: true
};

export class Claim {
  key: string;
  value: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  mgr: UserManager = new UserManager(settings);
  user: User;
  claims: Claim[] = [];

  constructor() {
    this.mgr.getUser()
      .then((existingUser) => {
        if (existingUser) {
          this.setupUser(existingUser);
          console.log('existing', this.user);
        } else {
          this.mgr.signinRedirectCallback()
          .then(function (signedInUser) {
            if (signedInUser) {
              this.setupUser(signedInUser);
              console.log('signed in', this.user);
            } else {
              this.mgr.signinRedirect().then(function () {
                console.log('signinRedirect done');
              });
            }
          }).catch(() => {
            this.mgr.signinRedirect().then(function () {
              console.log('signinRedirect done');
            });
          });
        }
      });
  }

  private setupUser(user: User) {
    this.user = user;
    const keys = Object.keys(user.profile);
    keys.forEach((key) => {
      this.claims.push({
        key: key,
        value: user.profile[key]
      });
    });
  }
}
