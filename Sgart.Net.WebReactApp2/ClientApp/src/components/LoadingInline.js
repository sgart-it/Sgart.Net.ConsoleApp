import React from 'react';
import { Spinner } from 'reactstrap';

export default function LoadingInline(props) {
  return (
    props.show === true && <Spinner size="sm" color="secondary" />
  );
}