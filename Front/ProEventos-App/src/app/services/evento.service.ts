import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';
import { take } from 'rxjs/operators';
import { environment } from '@environments/environment';

@Injectable( )
export class EventoService {
  baseURL = environment.apiURL + 'api/eventos/'
  tokenHeader = new HttpHeaders({ 'Authorization': 'Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIzIiwidW5pcXVlX25hbWUiOiJkb3VnbGFzMiIsIm5iZiI6MTY2MTE5MjcxMywiZXhwIjoxNjYxMjc5MTEzLCJpYXQiOjE2NjExOTI3MTN9.fLMkHq1oV9dkMs5tdBOljGWMEYQRiv6eJUCYXW_ux2SoY48QFaG7hAQfWuPpumCS4fq2ZgogFXNRIhm6YwR7Fg' });

  constructor(private http:HttpClient) { }

  public getEvento(): Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseURL, {headers: this.tokenHeader} ).pipe(take(1));
  }

  public getEventosByTema(tema: string): Observable<Evento[]> {
    return this.http
      .get<Evento[]>(`${this.baseURL, {headers: this.tokenHeader}}${tema}/tema`)
      .pipe(take(1));
  }

  public getEventoById(id: number): Observable<Evento> {
    return this.http
      .get<Evento>(`${this.baseURL, {headers: this.tokenHeader}}${id}`)
      .pipe(take(1));
  }

  public post(evento: Evento): Observable<Evento> {
    return this.http
      .post<Evento>(`${this.baseURL, {headers: this.tokenHeader}}`,evento)
      .pipe(take(1));
  }

  public put(evento: Evento): Observable<Evento> {
    return this.http
    .put<Evento>(`${this.baseURL, {headers: this.tokenHeader}}${evento.id}`, evento)
    .pipe(take(1));
  }

  public deleteEvento(id:number): Observable<any> {
    return this.http
      .delete(`${this.baseURL, {headers: this.tokenHeader}}${id}`)
      .pipe(take(1));
  }

  postUpload(eventoId: number, file: File): Observable<Evento> {

    const fileToUpload = file[0] as File;
    const formData = new FormData();

    formData.append('file', fileToUpload);

    return this.http
      .post<Evento>(`${this.baseURL, {headers: this.tokenHeader}}upload-image/${eventoId}`, formData)
      .pipe(take(1));
  }


}
