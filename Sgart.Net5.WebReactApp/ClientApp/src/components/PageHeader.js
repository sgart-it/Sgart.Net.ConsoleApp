import React, { Component } from 'react';
import { ErrorMessage } from './ErrorMessage';

export class PageHeader extends Component {
  static displayName = PageHeader.name;

  render() {
    const { title, description, message } = this.props;
    return (
      <div className='page-header'>
        <h1 className='page-header-title'>{title}</h1>
        {description !== undefined && description !== null && description !== ''
          && <p className='page-header-description'>{description}</p>
        }
        <ErrorMessage message={message} />
      </div>
    );
  }
}
