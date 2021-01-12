import { TestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { Todo, TodoService } from './todo.service';
import { HttpErrorResponse } from '@angular/common/http';
//https://dev.to/qarunqb/tdd-in-angular-further-http-testing-3mf2
let service: TodoService;
let httpMock: HttpTestingController;
//let displayErrorSpy: jasmine.Spy;

beforeEach(() => {
  //displayErrorSpy = spyOn(service, 'displayError');
  TestBed.configureTestingModule({
    imports: [HttpClientTestingModule],
  });
  service = TestBed.inject(TodoService);
  httpMock = TestBed.inject(HttpTestingController);
});

/*
Notice that in the afterEach block, we called httpMock.verify().
This ensures that there are no pending requests in our mock network
before moving on to other tests.
*/

afterEach(() => {
  httpMock.verify();
});

it('should be created', () => {
  expect(service).toBeTruthy();
});

it('getSingleTodo should send a GET request and return a single item', (done) => {
  service.getSingleTodo(1).subscribe(
    (item: any) => {
      expect(item).toBeDefined();
      done();
    },
    (error) => {
      fail(error.message);
    }
  );

  const testRequest = httpMock.expectOne(
    'https://jsonplaceholder.typicode.com/todos/1'
  );
  expect(testRequest.request.method).toBe('GET');
  testRequest.flush({ id: 1, userId: 1, title: 'Test Todo', completed: false });
});

it('createTodo should send a POST request and return the newly created item', (done) => {
  const item: Todo = {
    id: 2,
    userId: 2,
    title: 'Walk dog',
    completed: false,
  };

  service.createTodo(item).subscribe(
    (data: any) => {
      expect(data).toBeDefined();
      expect(data).toEqual(item);
      done();
    },
    (error) => {
      fail(error.message);
    }
  );

  const testRequest = httpMock.expectOne(
    'https://jsonplaceholder.typicode.com/todos'
  );
  expect(testRequest.request.method).toBe('POST');
  testRequest.flush(item);
});

it('should display an error message if the request is unauthorized', (done) => {
  service
    .updateTodo(1, { userId: 1, title: 'Walk dog', completed: true } as Todo)
    .subscribe(
      (data) => {
        expect(data).toBeNull();
        //expect(displayErrorSpy).toHaveBeenCalledWith('Unauthorized request');
        done();
      },
      (error: HttpErrorResponse) => {
        console.log(error);
        done();
      }
    );

  const testRequest = httpMock.expectOne(
    'https://jsonplaceholder.typicode.com/todos/1'
  );
  expect(testRequest.request.method).toBe('PUT');
  testRequest.flush(null, { status: 401, statusText: 'Unauthorized request' });
});
