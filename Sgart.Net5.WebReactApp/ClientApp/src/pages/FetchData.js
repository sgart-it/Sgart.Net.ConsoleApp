import React, { Component } from 'react';
import { Loading } from '../components/Loading';
import { PageHeader } from '../components/PageHeader';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = {
      forecasts: [],
      loading: true
    };
  }

  componentDidMount() {
    this.populateWeatherData();
  }

  static renderForecastsTable(forecasts) {
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
  }

  render() {
    let contents = this.state.loading
      ? <Loading loading={this.state.loading} />
      : FetchData.renderForecastsTable(this.state.forecasts);

    return (
      <div>
        <PageHeader title='Weather forecast' description='This component demonstrates fetching data from the server.' />
        <p></p>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('api/weatherforecast');
    const data = await response.json();
    this.setState({ forecasts: data, loading: false });
  }
}
