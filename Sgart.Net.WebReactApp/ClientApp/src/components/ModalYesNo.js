import React, { Component } from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button } from 'reactstrap';

export class ModalYesNo extends Component {
  static displayName = ModalYesNo.name;


  render() {
    const { show, title, body, onClick } = this.props;
    const textOk = this.props.textYes === undefined ? 'Ok' : this.props.textYes;
    const textCancel = this.props.textCancel === undefined ? 'Annulla' : this.props.textCancel;


    return (
      show === true &&
      <Modal isOpen='true' toggle={this.handleCancel}>
        <ModalHeader toggle={this.handleCancel}>{title}</ModalHeader>
        <ModalBody>{body}</ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={(e) => { e.preventDefault(); onClick(true, e); }}>{textOk}</Button>
          {' '}
          <Button color="secondary" onClick={(e) => { e.preventDefault(); onClick(false, e); }}>{textCancel}</Button>
        </ModalFooter>
      </Modal>
    );
  }
}
