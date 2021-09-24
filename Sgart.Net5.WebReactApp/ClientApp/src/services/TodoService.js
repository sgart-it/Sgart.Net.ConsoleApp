import baseService from './BaseService'

export class AppService {

  async getTodo() {
    return await baseService.fetchGet('api/todo');
  }

  async saveTodo(todo) {
    if (todo.todoId === undefined || todo.todoId === null || todo.todoId === 0) {
      //add
      return await baseService.fetchPost('api/todo', todo);
    } else {
      //edit
      return await baseService.fetchPut('api/todo', todo);
    }
  }

  async deleteTodo(todoId) {
    return await baseService.fetchDelete('api/todo?todoId=' + todoId);
  }
}

const appService = new AppService();

export default appService;
