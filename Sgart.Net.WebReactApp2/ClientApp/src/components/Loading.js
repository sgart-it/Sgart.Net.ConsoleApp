import React, { Component } from 'react';
import { Spinner } from 'reactstrap';
import './Loading.css';

export class Loading extends Component {
  static displayName = Loading.name;

  render() {
    return (
      this.props.show === true && <div className='page-loading'>
        <Spinner animation="border" role="status" aria-hidden="true" className="spinner" />
        <span>Attendi ...</span>
      </div>
    );
  }
}
