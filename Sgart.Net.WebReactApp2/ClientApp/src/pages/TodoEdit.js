import React, { useState, useEffect } from 'react';
import { Link, Navigate, useParams } from 'react-router-dom'; // useParams per leggere i parametri dalla url
import todoService from '../services/TodoService'
import { Button, Form, FormGroup, Label, Input, Alert, FormFeedback } from 'reactstrap';
import PageHeader from '../components/PageHeader';
import LoadingInline from '../components/LoadingInline';

export default function TodoEdit() {
  //const id = this.props.match.params.id;
  //const id = window.location.pathname.split("/")[3];
  const params = useParams();

  const [id, setId] = useState(params.id | 0);
  const [message, setMessage] = useState('');
  const [completed, setCompleted] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [redirecUrl, setRedirecUrl] = useState(null);

  const populateData = async () => {
    setLoading(true);
    setError(null);

    const result = await todoService.get(id);

    const valid = result.ok === true;

    if (valid === false && (result.message == null || result.message === '')) {
      result.message = 'Error undefined';
    }

    setMessage(valid ? result.data.message : '');
    setCompleted(valid ? result.data.completed : false);
    setError(result.message);
    setLoading(false);
  };

  useEffect(() => {
    console.log('component mount')

    populateData();

    // return a function to execute at unmount
    return () => {
      console.log('component will unmount')
    }
  }, []); // nessuna dipendenza = componentDidMount

  const handleSubmit = async (event) => {
    event.preventDefault()

    setLoading(true);
    setError(null);

    const result = await todoService.save({
      todoId: id,
      message: message,
      completed: completed
    });

    const valid = result.ok === true;

    setLoading(false);
    setError(result.message);
    setRedirecUrl(valid ? '/todo' : null);
  };

  const isIdValid = () => typeof id === 'number' && id !== 0;
  const isMessageValid = () => message !== null && message.length > 0;
  const isButtonEnabled = () => isMessageValid() && loading === false;

  const renderForm = () => {
    return (
      <Form onSubmit={handleSubmit}>
        <FormGroup>
          <Label for="message1">Messaggio</Label>
          <Input type="text" value={message} onChange={(event) => setMessage(event.target.value)} invalid={!isMessageValid()}
            name="message1" id="message1" placeholder="" />
          <FormFeedback invalid='true'>Il messaggio non pu&ograve; essere vuoto</FormFeedback>
        </FormGroup>
        <FormGroup>
          <Input type="switch" id="completed-1" label={completed === true ? 'Completato' : 'Da completare'}
            checked={completed} onChange={(event) => setCompleted(event.target.checked)} />
        </FormGroup>
        <div className='buttons-bar'>
          <Button color="primary" size="sm" disabled={!isButtonEnabled()}>Salva</Button>
          <Button color="secondary" size="sm" tag={Link} to='/todo'>Annulla</Button>
          <LoadingInline show={loading} />
        </div>
        {/*<Alert color="secondary">Debug only: State={JSON.stringify(state)}</Alert>*/}
        <Alert color="secondary">Debug only: State={JSON.stringify({
          id:id,
          message:message,
          completed:completed,
          loading:loading,
          error:error,
          redirectUrl:redirecUrl
        })}</Alert>
      </Form>
    );
  };

  if (redirecUrl != null) {
    return (
      <Navigate to={redirecUrl} />
    );
  }



  return (
    <div>
      <PageHeader title={'Todo modifica id: ' + id} description='Esempio modifica item' message={error} />
      {isIdValid()
        ? renderForm()
        : <Alert color="secondary">L'id non pu&ograve; essere nullo</Alert>
      }
    </div>

  );
}
