import React, { Component } from 'react';
import { Button, Spinner } from 'reactstrap';
import { Link } from 'react-router-dom';
import todoService from '../services/TodoService'
import { PageHeader } from '../components/PageHeader';
import { DateTime } from '../components/DateTime';
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

  handleRefresh = () => {
    this.populateData();
  }

  handleDelete = async (todoId, e) => {
    e.preventDefault()

    this.setState({
      loading: true,
      error: null
    });

    await todoService.delete(todoId);

    this.setState({
      loading: true,
      error: null
    });


    this.populateData();
  }

  // questo metodo è statico quindi non posso usare this, ma devo passargli gli oggetti che mi servono
  static renderTable(items, handleDelete) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Id</th>
            <th>Messaggio</th>
            <th>Completato</th>
            <th>Modificato il</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {items.map(item =>
            <tr key={item.todoId}>
              <td>{item.todoId}</td>
              <td>{item.message}</td>
              <td><div className={item.completed ? 'todo-completed' : ''}>{item.completed ? 'Si' : 'No'}</div></td>
              <td><DateTime date={item.modified} /></td>
              <td>
                <Button variant="outline-light" size="sm" tag={Link} to={'/todo/edit/' + item.todoId}>Modifica</Button>
                {' ' /* attenzione il Button usa l'evento onClick non onChange */}
                <Button variant="outline-light" size="sm" onClick={(e) => handleDelete(item.todoId, e)}>Cancella</Button>
              </td>
            </tr>
          )}
        </tbody>
      </table >
    );
  }

  render() {
    let contents = Todo.renderTable(this.state.items, this.handleDelete);

    return (
      <div>
        <PageHeader title='Todo' description='Esempio lettura API in React' message={this.state.error} />
        <div className='buttons-bar'>
          <Button variant="outline-light" size="sm" tag={Link} to='/todo/add'>Aggiungi</Button>

          <Button variant="outline-light" size="sm" onClick={this.handleRefresh}>Aggiorna</Button>
          {this.state.loading === true && <Spinner color="secondary" />}
        </div>
        {contents}
      </div>
    );
  }

  async populateData() {
    this.setState({
      loading: true,
      error: null
    });

    const result = await todoService.getAll();

    this.setState({
      items: result.ok === true ? result.data : null,
      loading: false,
      error: result.message
    });
  }

}
