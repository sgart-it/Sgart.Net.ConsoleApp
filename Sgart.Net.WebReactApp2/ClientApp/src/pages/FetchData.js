import React, { useState, useEffect } from 'react';
import Loading from '../components/Loading';
import PageHeader from '../components/PageHeader';

export default function FetchData() {

  const [forecasts, setForecasts] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    console.log('component mount')

    const populateWeatherData = async () => {
      const response = await fetch('api/weatherforecast');
      const data = await response.json();
      setForecasts(data);
      setLoading(false);
    };

    populateWeatherData();

    // return a function to execute at unmount
    return () => {
      console.log('component will unmount')
    }
  }, []); // nessuna dipendenza = componentDidMount

  const renderForecastsTable = (forecasts) => {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {forecasts.map(forecast =>
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  };

  const contents = loading
    ? <Loading show={loading} />
    : renderForecastsTable(forecasts);

  return (
    <div>
      <PageHeader title='Weather forecast' description='This component demonstrates fetching data from the server.' />
      <p></p>
      {contents}
    </div>
  );

}
