import React, { useState } from 'react';
import todoService from '../services/TodoService'
import { Button, Form, FormGroup, Label, Input, Alert, FormFeedback } from 'reactstrap';
import { Link, Navigate } from 'react-router-dom';
import PageHeader from '../components/PageHeader';
import LoadingInline from '../components/LoadingInline';

export default function TodoAdd() {
  const [message, setMessage] = useState('');
  const [completed, setCompleted] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [redirecUrl, setRedirecUrl] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault()

    setLoading(true);
    setError(null);

    const result = await todoService.save({
      message: message,
      completed: completed
    });

    const valid = result.ok === true;

    setLoading(false);
    setError(result.message);
    setRedirecUrl(valid ? '/todo' : null);
  };

  const isMessageValid = () => message !== null && message.length > 0;
  const isButtonEnabled = () => isMessageValid() && loading === false;

  if (redirecUrl != null) {
    return (
      <Navigate to={redirecUrl} />
    );
  }

  const contents = <Form onSubmit={handleSubmit}>
    <FormGroup>
      <Label for="message1">Messaggio</Label>
      {/* il valore è in 'value', 
         * 'invalid' è un boolean e controlla FormFeedback che deve stare nello stesso FromGroup 
         * onChange aggiorna la proprietà 'message' nello state (e.target.value )*/}
      <Input type="text" value={message} onChange={(event) => setMessage(event.target.value)} invalid={!isMessageValid()}
        name="message1" id="message1" placeholder="" />
      <FormFeedback invalid='true'>Il messaggio non pu&ograve; essere vuoto</FormFeedback>
    </FormGroup>
    <FormGroup>
      {/* deve avere un id univoco, usa la proprietà 'checked' e non 'value'
          * onChange aggiorna la proprietà 'completed' nello state (e.target.checked )*/}
      <Input type="switch" id="completed1" name="completed1" label={completed === true ? 'Completato' : 'Da completare'}
        checked={completed} onChange={(event) => setCompleted(event.target.checked)} />
    </FormGroup>
    <div className='buttons-bar'>
      <Button color="primary" size="sm" disabled={!isButtonEnabled()}>Salva</Button>
      <Button color="secondary" size="sm" tag={Link} to='/todo'>Annulla</Button>
      <LoadingInline show={loading} />
    </div>
    <Alert color="secondary">Debug only: Message={message} | Completed={completed.toString()}</Alert>
  </Form>;

  return (
    <div>
      <PageHeader title='Todo aggiungi' description='Esempio aggiunta item' message={error} />
      {contents}
    </div>

  );
}