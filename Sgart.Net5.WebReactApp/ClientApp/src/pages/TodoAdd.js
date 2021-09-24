import React, { Component, useState } from 'react';
import { Button, Form, FormGroup, Label, Input, CustomInput } from 'reactstrap';
import { Link } from 'react-router-dom';
import appService from '../services/TodoService'
import { Loading } from '../components/Loading';
import { PageHeader } from '../components/PageHeader';

export class TodoAdd extends Component {
  static displayName = TodoAdd.name;

  constructor(props) {
    super(props);
    this.state = {
      form: {
        message: '',
        completed: false
      },
      loading: true,
      error: null
    };
  }

  componentDidMount() {
  }


  handleSubmit = (e) => {
    e.preventDefault()
    console.log(e)

  }

  isButtonEnabled = () => {
    return this.loading === true;
  }


  render() {
    const { message, completed } = this.state;

    const contents = <Form onSubmit={this.handleSubmit}>
      <FormGroup>
        <Label for="message">Messaggio</Label>
        <Input type="text" value={message} onChange={(e) => this.setState({ form: { message: e.target.value } })} name="message" id="message" placeholder="" />
      </FormGroup>
      <FormGroup check>
        <CustomInput type="switch" id="exampleCustomSwitch" name="customSwitch" label="Completato"
          value={completed} onChange={(e) => this.setState({ form: { completed: e.target.value } })} />
      </FormGroup>
      <p></p>
      <Button size="sm" disabled={!this.isButtonEnabled()}>Salva</Button>
      <Button variant="outline-light" size="sm" tag={Link} to='/todo'>Annulla</Button>
      {message}
    </Form>;

    return (
      <div>
        <PageHeader title='Todo' description='Aggiunta todo' message={this.state.error} />
        {contents}
      </div>

    );
  }

  async populateData() {
    this.setState({
      loading: true,
      error: null
    });

    const result = await appService.AddTodo();;

    if (result.status === 200) {
      this.setState({
        items: result.data,
        loading: false,
        error: null
      });
    } else {
      this.setState({
        items: null,
        loading: false,
        error: result.message
      });
    }

  }
}
