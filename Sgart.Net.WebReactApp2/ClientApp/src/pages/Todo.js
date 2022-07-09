import React, { useState, useEffect } from 'react';
import { Button } from 'reactstrap';
import { Link } from 'react-router-dom';
import todoService from '../services/TodoService'
import PageHeader from '../components/PageHeader';
import DateTime from '../components/DateTime';
import ModalYesNo from '../components/ModalYesNo';
import LoadingInline from '../components/LoadingInline';
import './Todo.css';

export default function Todo() {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [itemToDelete, setItemToDelete] = useState(0);

  const refreshData = async () => {
    setLoading(true);
    setError(null);

    const result = await todoService.getAll();

    setItems(result.ok === true ? result.data : null);
    setError(result.message);
    setLoading(false);
  };

  useEffect(() => {
    console.log('component mount')

    refreshData();

    // return a function to execute at unmount
    return () => {
      console.log('component will unmount')
    }
  }, []); // nessuna dipendenza = componentDidMount


  const handleExport = () => window.location.assign("/api/todo/excel?t=" + (new Date()).getTime().toString());
  const handleShowDelete = async (todoId) => setItemToDelete(todoId);
  const handleDelete = async (confirm, event) => {
    event.preventDefault();

    setLoading(true);
    setError(null);

    const todoId = itemToDelete;

    if (confirm === true && todoId !== 0)
      await todoService.delete(todoId);

    setItemToDelete(0);
    //setError(null);
    //setLoading(true);

    await refreshData();
  };

  const renderTable = (items, handleShowDelete) => {
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
                  <Button color="secondary" size="sm" onClick={(event) => handleShowDelete(item.todoId, event)}>Cancella</Button>
                </td>
              </tr>
            )
          }
        </tbody>
      </table>
    );
  };

  return (
    <div>
      <PageHeader title='Todo' description='Esempio lettura API in React' message={error} />
      <div className='buttons-bar'>
        <Button color="primary" size="sm" tag={Link} to='/todo/add'>Aggiungi</Button>

        <Button color="secondary" size="sm" onClick={refreshData}>Aggiorna</Button>

        <Button color="secondary" size="sm" onClick={handleExport}>Export</Button>

        <LoadingInline show={loading} />
      </div>
      {renderTable(items, handleShowDelete)}
      {/* modal di conferma cancellazione */}
      <ModalYesNo show={itemToDelete !== 0} onClick={handleDelete} title='Delete' body={`Vuoi cancellare l'item con id ${itemToDelete} ?`} />
    </div>
  );
}
