import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, distinctUntilChanged, map, Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { SignIn } from '../_authentication/sign-in/models/sign-in';
import { SingUp } from '../_authentication/sign-up/models/sign-up';
import { KanUser } from '../_models/kan-user';
import { Result } from '../_models/result';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private readonly apiUrl = environment.apiUrl;
  private readonly usersUrl = this.apiUrl + 'users/';
  private token = '';
  private loggedUser = new BehaviorSubject<KanUser>({
    id: 0,
    userName: '',
    creationDate: new Date(),
  });
  private readonly tokenKey = 'token';
  private readonly userKey = 'user';

  public loggedUser$: Observable<KanUser> = this.loggedUser.pipe(
    map((e) => {
      if (e.id !== 0) return e;
      const userJSON = localStorage.getItem(this.userKey);
      if (!userJSON) return e;
      const parsedUser = JSON.parse(userJSON!);
      this.loggedUser.next(parsedUser);
      return parsedUser;
    }),
    distinctUntilChanged()
  );

  constructor(private http: HttpClient) {}

    public getUsers(query: string) {
      var params = new HttpParams();
      params = params.append('query', query);
      const options = {...this.getOptions(), params};
      return this.http.get<Result<KanUser[]>>(this.usersUrl, options);
    }

  public signUp(signUp: SingUp) {
    return this.http
      .post(this.usersUrl + 'sign-up', signUp);
  }

  public signIn(signIn: SignIn) {
    return this.http
      .post(this.usersUrl + 'sign-in', signIn)
      .pipe(map((response: any) => {
        const content = response.content;
        this.token = content.token;
        localStorage.setItem(this.tokenKey, content.token);
        localStorage.setItem(this.userKey, JSON.stringify(content.loggedUser));
        this.loggedUser.next(content.loggedUser);
      }));
  }

  public signOut() {
    localStorage.clear();
    this.loggedUser.next({
      id: 0,
      userName: '',
      creationDate: new Date(),
    });

    this.token = '';
  }

  public getOptions() {
    return {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + this.getLazyToken(),
      })
    };
  }

  private getLazyToken() {
    if(!!this.token) return this.token;

    var item = localStorage.getItem(this.tokenKey);

    if(!item) return item;

    this.token = item;

    return this.token;
  }
}
