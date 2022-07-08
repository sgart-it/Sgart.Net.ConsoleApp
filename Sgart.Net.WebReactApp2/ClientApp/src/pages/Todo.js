import React, { Component } from 'react';
import { Button } from 'reactstrap';
import { Link } from 'react-router-dom';
import todoService from '../services/TodoService'
import { PageHeader } from '../components/PageHeader';
import { DateTime } from '../components/DateTime';
import { ModalYesNo } from '../components/ModalYesNo';
import { LoadingInline } from '../components/LoadingInline';
import './Todo.css';

export class Todo extends Component {
  static displayName = Todo.name;

  constructor(props) {
    super(props);
    this.state = {
      items: [],
      loading: true,
      error: null,
      itemToDelete: 0
    };
  }

  componentDidMount() {
    this.populateData();
  }

  handleRefresh = () => {
    this.populateData();
  }

  handleExport = () => {
    window.location.assign("/api/todo/excel?t=" + (new Date()).getTime().toString());
  }

  handleShowDelete = async (todoId, e) => {
    this.setState({
      itemToDelete: todoId
    });
  }

  handleDelete = async (confirm, e) => {
    e.preventDefault()

    this.setState({
      loading: true,
      error: null
    });

    const todoId = this.state.itemToDelete;

    if (confirm === true && todoId !== 0)
      await todoService.delete(todoId);

    this.setState({
      loading: true,
      error: null,
      itemToDelete: 0
    });


    this.populateData();
  }

  // questo metodo è statico quindi non posso usare this, ma devo passargli gli oggetti che mi servono
  static renderTable(items, handleShowDelete) {
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
          {items == null
            ? <tr><td colSpan="5">No items</td></tr>
            : items.map(item =>
              <tr key={item.todoId}>
                <td>{item.todoId}</td>
                <td>{item.message}</td>
                <td><div className={item.completed ? 'todo-completed' : ''}>{item.completed ? 'Si' : 'No'}</div></td>
                <td><DateTime date={item.modified} /></td>
                <td>
                  <Button color="primary" size="sm" tag={Link} to={'/todo/edit/' + item.todoId}>Modifica</Button>
                  {' ' /* attenzione il Button usa l'evento onClick non onChange */}
                  <Button color="secondary" size="sm" onClick={(e) => handleShowDelete(item.todoId, e)}>Cancella</Button>
                </td>
              </tr>
            )
          }
        </tbody>
      </table>
    );
  }

  render() {
    let contents = Todo.renderTable(this.state.items, this.handleShowDelete);

    return (
      <div>
        <PageHeader title='Todo' description='Esempio lettura API in React' message={this.state.error} />
        <div className='buttons-bar'>
          <Button color="primary" size="sm" tag={Link} to='/todo/add'>Aggiungi</Button>

          <Button color="secondary" size="sm" onClick={this.handleRefresh}>Aggiorna</Button>

          <Button color="secondary" size="sm" onClick={this.handleExport}>Export</Button>

          <LoadingInline show={this.state.loading} />
        </div>
        {contents}
        {/* modal di conferma cancellazione */}
        <ModalYesNo show={this.state.itemToDelete !== 0} onClick={this.handleDelete} title='Delete' body={`Vuoi cancellare l'item con id ${this.state.itemToDelete} ?`} />
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
