import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ITopic } from '../Models/itopic';

@Injectable({
  providedIn: 'root',
})
export class TopicService {
  private httpClient = inject(HttpClient);
  private url = 'https://localhost:7055/api/Topic/';

  getAllTopics(): Observable<ITopic[]> {
    return this.httpClient.get<ITopic[]>(this.url + 'GetAllTopics');
  }
}