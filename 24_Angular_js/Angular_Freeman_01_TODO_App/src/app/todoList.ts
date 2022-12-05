import { TodoItem } from './todoItem';
export class TodoList {

constructor(public user: string, private todoItems: TodoItem[] = []) {
// no statements required
}
get items(): readonly TodoItem[] {
return this.todoItems;
}
// tslint:disable-next-line: typedef
addItem(task: string) {
this.todoItems.push(new TodoItem(task));
}
}
