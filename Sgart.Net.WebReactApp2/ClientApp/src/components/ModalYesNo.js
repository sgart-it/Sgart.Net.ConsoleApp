import React from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button } from 'reactstrap';

export default function ModalYesNo(props) {
  const { show, title, body, textYes, textCancel } = props;
  const textOkInt = textYes === undefined ? 'Ok' : textYes;
  const textCancelInt = textCancel === undefined ? 'Annulla' : textCancel;

  return (
    show === true &&
    <Modal isOpen={true} toggle={props.handleCancel}>
      <ModalHeader toggle={props.handleCancel}>{title}</ModalHeader>
      <ModalBody>{body}</ModalBody>
      <ModalFooter>
          <Button color="primary" onClick={(e) => { e.preventDefault(); props.onClick(true, e); }}>{textOkInt}</Button>
        {' '}
          <Button color="secondary" onClick={(e) => { e.preventDefault(); props.onClick(false, e); }}>{textCancelInt}</Button>
      </ModalFooter>
    </Modal>
  );
}
