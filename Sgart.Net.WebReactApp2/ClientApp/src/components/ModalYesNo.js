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
          <Button color="primary" onClick={(event) => { event.preventDefault(); props.onClick(true, event); }}>{textOkInt}</Button>
        {' '}
          <Button color="secondary" onClick={(event) => { event.preventDefault(); props.onClick(false, event); }}>{textCancelInt}</Button>
      </ModalFooter>
    </Modal>
  );
}
