import React, { Component } from 'react';
import todoService from '../services/TodoService'
import { Button, Form, FormGroup, Label, Input, CustomInput, Alert, FormFeedback } from 'reactstrap';
import { Link, Redirect } from 'react-router-dom';
import { PageHeader } from '../components/PageHeader';
import { LoadingInline } from '../components/LoadingInline';

export class TodoEdit extends Component {
  static displayName = TodoEdit.name;

  constructor(props) {
    super(props);
    this.state = {
      id: props.match.params.id | 0,
      message: '',
      completed: false,
      loading: false,
      error: null,
      redirecUrl: null
    };
  }

  componentDidMount() {
    this.populateData();
  }

  handleSubmit = async (e) => {
    e.preventDefault()

    this.setState({
      loading: true,
      error: null
    });

    const result = await todoService.save({
      todoId: this.state.id,
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

  isIdValid = () => {
    return typeof this.state.id === 'number' && this.state.message.id !== 0;
  }

  isMessageValid = () => {
    return this.state.message !== null && this.state.message.length > 0;
  }

  isButtonEnabled = () => {
    return this.isMessageValid() && this.state.loading === false;
  }

  renderForm = () => {
    const { message, completed, loading } = this.state;

    return (
      <Form onSubmit={this.handleSubmit}>
        <FormGroup>
          <Label for="message1">Messaggio</Label>
          <Input type="text" value={message} onChange={(e) => this.setState({ message: e.target.value })} invalid={!this.isMessageValid()}
            name="message1" id="message1" placeholder="" />
          <FormFeedback invalid='true'>Il messaggio non pu&ograve; essere vuoto</FormFeedback>
        </FormGroup>
        <FormGroup>
          <CustomInput type="switch" id="completed-1" label={completed === true ? 'Completato' : 'Da completare'}
            checked={completed} onChange={(e) => this.setState({ completed: e.target.checked })} />
        </FormGroup>
        <div className='buttons-bar'>
          <Button size="sm" disabled={!this.isButtonEnabled()}>Salva</Button>
          <Button variant="outline-light" size="sm" tag={Link} to='/todo'>Annulla</Button>
          <LoadingInline show={loading} />
        </div>
        <Alert color="secondary">Debug only: State={JSON.stringify(this.state)}</Alert>
      </Form>
    );
  }

  render() {
    if (this.state.redirecUrl != null) {
      return (
        <Redirect to={this.state.redirecUrl} />
      );
    }



    return (
      <div>
        <PageHeader title={'Todo modifica id: ' + this.state.id} description='Esempio aggiunta item' message={this.state.error} />
        {this.isIdValid()
          ? this.renderForm()
          : <Alert color="secondary">L'id non pu&ograve; essere nullo</Alert>
        }
      </div>

    );
  }

  populateData = async () => {
    this.setState({
      loading: true,
      error: null
    });

    const result = await todoService.get(this.state.id);

    const valid = result.ok === true;

    if (valid === false && (result.message == null || result.message === '')) {
      result.message = 'Error undefined';
    }

    this.setState({
      message: valid ? result.data.message : '',
      completed: valid ? result.data.completed : false,
      loading: false,
      error: result.message
    });
  }
}
