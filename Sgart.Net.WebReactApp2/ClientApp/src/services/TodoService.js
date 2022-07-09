import baseService from './BaseService'

const getAll = async () => await baseService.fetchGet('api/todo');
const get = async (todoId) => await baseService.fetchGet('api/todo/' + todoId);
const save = async (todo) => {
  if (todo.todoId === undefined || todo.todoId === null || todo.todoId === 0) {
    //add
    return await baseService.fetchPost('api/todo', todo);
  } else {
    //edit
    return await baseService.fetchPut('api/todo', todo);
  }
};
const deleteInt = async (todoId) => await baseService.fetchDelete('api/todo/' + todoId);

const todoService = {
  getAll,
  get,
  save,
  delete: deleteInt
};

export default todoService;
