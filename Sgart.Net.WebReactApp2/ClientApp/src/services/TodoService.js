import baseService from './BaseService'

export class AppService {

  async getAll() {
    return await baseService.fetchGet('api/todo');
  }

  async get(todoId) {
    return await baseService.fetchGet('api/todo/' + todoId);
  }

  async save(todo) {
    if (todo.todoId === undefined || todo.todoId === null || todo.todoId === 0) {
      //add
      return await baseService.fetchPost('api/todo', todo);
    } else {
      //edit
      return await baseService.fetchPut('api/todo', todo);
    }
  }

  async delete(todoId) {
    return await baseService.fetchDelete('api/todo/' + todoId);
  }
}

const appService = new AppService();

export default appService;
