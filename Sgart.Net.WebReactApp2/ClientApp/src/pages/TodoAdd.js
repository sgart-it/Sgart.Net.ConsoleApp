import React, { Component } from 'react';
import todoService from '../services/TodoService'
import { Button, Form, FormGroup, Label, Input, Alert, FormFeedback } from 'reactstrap';
import { Link, Navigate } from 'react-router-dom';
import { PageHeader } from '../components/PageHeader';
import { LoadingInline } from '../components/LoadingInline';

export class TodoAdd extends Component {
  static displayName = TodoAdd.name;

  constructor(props) {
    super(props);
    this.state = {
      message: '',
      completed: false,
      loading: false,
      error: null,
      redirecUrl: null
    };
  }

  handleSubmit = async (e) => {
    e.preventDefault()

    this.setState({
      loading: true,
      error: null
    });

    const result = await todoService.save({
      message: this.state.message,
      completed: this.state.completed
    });

    const valid = result.ok === true;

    this.setState({
      loading: false,
      error: result.message,
      redirecUrl: valid ? '/todo' : null
    });

  }

  isMessageValid = () => {
    return this.state.message !== null && this.state.message.length > 0;
  }

  isButtonEnabled = () => {
    return this.isMessageValid() && this.state.loading === false;
  }


  render() {
    if (this.state.redirecUrl != null) {
      return (
        <Navigate to={this.state.redirecUrl} />
      );
    }

    const { message, completed, loading } = this.state;

    const contents = <Form onSubmit={this.handleSubmit}>
      <FormGroup>
        <Label for="message1">Messaggio</Label>
        {/* il valore è in 'value', 
         * 'invalid' è un boolean e controlla FormFeedback che deve stare nello stesso FromGroup 
         * onChange aggiorna la proprietà 'message' nello state (e.target.value )*/}
        <Input type="text" value={message} onChange={(e) => this.setState({ message: e.target.value })} invalid={!this.isMessageValid()}
          name="message1" id="message1" placeholder="" />
        <FormFeedback invalid='true'>Il messaggio non pu&ograve; essere vuoto</FormFeedback>
      </FormGroup>
      <FormGroup>
        {/* deve avere un id univoco, usa la proprietà 'checked' e non 'value'
          * onChange aggiorna la proprietà 'completed' nello state (e.target.checked )*/}
        <Input type="switch" id="completed1" name="completed1" label={completed === true ? 'Completato' : 'Da completare'}
          checked={completed} onChange={(e) => this.setState({ completed: e.target.checked })} />
      </FormGroup>
      <div className='buttons-bar'>
        <Button color="primary" size="sm" disabled={!this.isButtonEnabled()}>Salva</Button>
        <Button color="secondary" size="sm" tag={Link} to='/todo'>Annulla</Button>
        <LoadingInline show={ loading } />
      </div>
      <Alert color="secondary">Debug only: Message={message} | Completed={completed.toString()}</Alert>
    </Form>;

    return (
      <div>
        <PageHeader title='Todo aggiungi' description='Esempio aggiunta item' message={this.state.error} />
        {contents}
      </div>

    );
  }

}
