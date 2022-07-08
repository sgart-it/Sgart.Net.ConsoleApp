import React from 'react';
import './ErrorMessage.css';

export default function ErrorMessage(props) {
  const { message } = props;

  return (
    message !== undefined && message !== null && message !== '' && <div className='error-message'>{message}</div>
  );
}
