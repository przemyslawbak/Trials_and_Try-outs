import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

export interface Todo {
  userId: number;
  id: number;
  title: string;
  completed: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class TodoService {
  url: string = 'https://jsonplaceholder.typicode.com/todos';

  constructor(private http: HttpClient) {}

  getAllTodos() {
    return this.http.get(this.url);
  }

  getSingleTodo(id: number) {
    return this.http.get(`${this.url}/${id}`);
  }

  createTodo(item: Todo) {
    return this.http.post(this.url, item);
  }

  updateTodo(id: number, updatedItem: Todo) {
    return this.http.put(`${this.url}/${id}`, updatedItem);
  }

  deleteTodo(id: number) {
    return this.http.delete(`${this.url}/${id}`);
  }

  displayError(message: string) {
    console.log(message);
  }
}
