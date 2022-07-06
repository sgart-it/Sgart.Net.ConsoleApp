import React, { Component } from 'react';
import './ErrorMessage.css';

export class ErrorMessage extends Component {
  static displayName = ErrorMessage.name;

  render() {
    const { message } = this.props;

    return (
      message !== undefined && message !== null && message !== '' && <div className='error-message'>{message}</div>
    );
  }
}
