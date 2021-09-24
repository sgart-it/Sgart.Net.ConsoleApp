import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './pages/Home';
import { FetchData } from './pages/FetchData';
import { Counter } from './pages/Counter';
import { Todo } from './pages/Todo';
import { TodoAdd } from './pages/TodoAdd';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route exact path='/todo' component={Todo} />
        <Route exact path='/todo/add' component={TodoAdd} />
        {/*<Route path='/todo/edit/:id' component={Todo} />*/}
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data' component={FetchData} />
      </Layout>
    );
  }
}
