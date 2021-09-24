import React, { Component } from 'react';
import { Button } from 'reactstrap';
import { Link } from 'react-router-dom';
import appService from '../services/TodoService'
import { Loading } from '../components/Loading';
import { PageHeader } from '../components/PageHeader';
import './Todo.css';

export class Todo extends Component {
  static displayName = Todo.name;

  constructor(props) {
    super(props);
    this.state = {
      items: [],
      loading: true,
      error: null
    };
  }

  componentDidMount() {
    this.populateData();
  }

  handleEdit() {
  };

  static renderTable(items, handleEdit, handleDelete) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Id</th>
            <th>Messaggio</th>
            <th>Completato</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {items.map(item =>
            <tr key={item.todoId}>
              <td>{item.todoId}</td>
              <td>{item.message}</td>
              <td><div className={item.completed ? 'todo-completed' : ''}>{item.completed ? 'Si' : 'No'}</div></td>
              <td>
                <Button variant="outline-light" size="sm" onClick={handleEdit}>Modifica</Button>
                {' '}
                <Button variant="outline-light" size="sm" onClick={handleDelete}>Cancella</Button>
              </td>
            </tr>
          )}
        </tbody>
      </table >
    );
  }

  render() {
    let contents = this.state.loading
      ? <Loading loading={this.state.loading} />
      : Todo.renderTable(this.state.items);

    return (
      <div>
        <PageHeader title='Todo' description='Esempio lettura API in React' message={this.state.error} />
        <Button variant="outline-light" size="sm" tag={Link} to='/todo/add'>Aggiungi</Button>
        {contents}
      </div>
    );
  }

  async populateData() {
    this.setState({
      loading: true,
      error: null
    });

    const result = await appService.getTodo();;

    this.setState({
      items: result.status === 200 ? result.data : null,
      loading: false,
      error: result.message
    });
  }

}
