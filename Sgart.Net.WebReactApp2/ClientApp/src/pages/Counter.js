import React, { useState } from 'react';
import PageHeader from '../components/PageHeader';

export default function Counter() {

  const [currentCount, setCurrentCount] = useState(0);

  const incrementCounter = () => setCurrentCount(currentCount + 1);
  const decrementCounter = () => {
    if (currentCount > 0)
      setCurrentCount(currentCount - 1)
  };

  return (
    <div>
      <PageHeader title='Counter' description='This is a simple example of a React component.' />

      <p aria-live="polite">Current count: <strong>{currentCount}</strong></p>

      <button className="btn btn-primary" onClick={incrementCounter}>Increment +1</button>
      <span> </span>
      <button className="btn btn-primary" onClick={decrementCounter}>Decrement -1</button>
    </div>
  );
}
