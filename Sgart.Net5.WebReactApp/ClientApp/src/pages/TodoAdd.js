import React, { Component, useState } from 'react';
import todoService from '../services/TodoService'
import { Button, Form, FormGroup, Label, Input, CustomInput, Alert, FormFeedback, Spinner } from 'reactstrap';
import { Link, Redirect } from 'react-router-dom';
import { PageHeader } from '../components/PageHeader';

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
        <Redirect to={this.state.redirecUrl} />
      );
    }

    const { message, completed, loading } = this.state;

    const contents = <Form onSubmit={this.handleSubmit}>
      <FormGroup>
        <Label for="message1">Messaggio</Label>
        <Input type="text" value={message} onChange={(e) => this.setState({ message: e.target.value })} invalid={!this.isMessageValid()}
          name="message1" id="message1" placeholder="" />
        <FormFeedback invalid>Il messaggio non pu&ograve; essere vuoto</FormFeedback>
      </FormGroup>
      <FormGroup>
        <CustomInput type="switch" id="completed1" name="completed1" label={completed === true ? 'Completato' : 'Da completare'}
          value={completed} onChange={(e) => this.setState({ completed: e.target.checked })} />
      </FormGroup>
      <p></p>
      <Button size="sm" disabled={!this.isButtonEnabled()}>Salva</Button> {loading === true && <Spinner color="secondary" />}
      <span> </span>
      <Button variant="outline-light" size="sm" tag={Link} to='/todo'>Annulla</Button>

      <p></p>
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
