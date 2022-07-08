import React, { Component } from 'react';

export class DateTime extends Component {
  static displayName = DateTime.name;

  render() {
    // TODO: l'aggiunta della 'Z' è un workaround temporano finchénon capisco come serializzare in automatico le date in UTC
    const dt = new Date(this.props.date + 'Z'); // es.: 2019-07-26T00:00:00

    const showTime = this.props.showTime !== undefined || this.props.showTime !== null || this.props.showTime === true;
    const showDate = this.props.showTime === undefined || this.props.showTime === null || this.props.showTime === true || showTime === false;

    const pad = (num) => {
      return (num < 10 ? '0' : '') + num;
    };

    // formatto la data in italiano dd/MM/yyyy HH:mm:ss
    const dtString = (
      showDate === true
        ? pad(dt.getDate()) + '/' + pad(dt.getMonth() + 1) + '/' + pad(dt.getFullYear())
        : ''
    ) + (showTime === true
      ? ' ' + pad(dt.getHours()) + ':' + pad(dt.getMinutes()) + ':' + pad(dt.getSeconds())
      : ''
      );

    return (
      <span className='date-time'>{dtString}</span>
    );
  }
}
