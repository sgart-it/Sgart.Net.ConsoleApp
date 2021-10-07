import React, { Component } from 'react';
import { Spinner } from 'reactstrap';

export class LoadingInline extends Component {
  static displayName = LoadingInline.name;

  render() {
    return (
      this.props.show === true && <Spinner size="sm" color="secondary" children={false} />
    );
  }
}
