import { Counter } from "./pages/Counter";
import { FetchData } from "./pages/FetchData";
import { Home } from "./pages/Home";
import { Todo } from "./pages/Todo";
import { TodoAdd } from "./pages/TodoAdd";
import { TodoEdit } from "./pages/TodoEdit";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/todo',
    element: <Todo />
  },
  {
    path: '/todo/add',
    element: <TodoAdd />
  },
  {
    path: '/todo/edit/:id',
    element: <TodoEdit />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  }
];

export default AppRoutes;
