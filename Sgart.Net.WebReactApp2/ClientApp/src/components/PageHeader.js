import React from 'react';
import ErrorMessage from './ErrorMessage';

export default function PageHeader(props) {
  const { title, description, message } = props;

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

