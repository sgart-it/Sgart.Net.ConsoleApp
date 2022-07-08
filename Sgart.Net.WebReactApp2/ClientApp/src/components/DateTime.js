import React from 'react';

export default function DateTime(props) {
  const { date, showTime, showDate } = props;
  // TODO: l'aggiunta della 'Z' è un workaround temporano finché non capisco come serializzare in automatico le date in UTC
  const dt = new Date(date + 'Z'); // es.: 2019-07-26T00:00:00

  const pad = (num) => (num < 10 ? '0' : '') + num;

  const showTimeInt = showTime !== undefined || showTime !== null || showTime === true;
  // formatto la data in italiano dd/MM/yyyy HH:mm:ss
  const dtString = (
    showTimeInt === true
      ? pad(dt.getDate()) + '/' + pad(dt.getMonth() + 1) + '/' + pad(dt.getFullYear())
      : ''
  ) + (showDate === undefined || showDate === null || showDate === true || showTimeInt === false
    ? ' ' + pad(dt.getHours()) + ':' + pad(dt.getMinutes()) + ':' + pad(dt.getSeconds())
    : ''
    );

  return (
    <span className='date-time'>{dtString}</span>
  );
}