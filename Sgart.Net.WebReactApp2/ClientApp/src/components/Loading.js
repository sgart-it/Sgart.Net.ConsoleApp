import React from 'react';
import { Spinner } from 'reactstrap';
import './Loading.css';

export default function Loading(props) {
  return (
    props.show === true && <div className='page-loading'>
      <Spinner animation="border" role="status" aria-hidden="true" className="spinner" />
      <span>Attendi ...</span>
    </div>
  );
}
