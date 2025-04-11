import { useEffect, useState } from 'react';
import './App.css';

function App() {
  const [accounts, setAccounts] = useState([]);

  useEffect(() => {
    fetch('/api/accounts')
      .then(res => res.json())
      .then(data => setAccounts(data))
      .catch(err => console.error('API fetch error:', err));
  }, []);

  return (
    <div>
      <h1>Bank Accounts</h1>
      <ul>
        {accounts.map((acc) => (
          <li key={acc.accountID}>
            {acc.accountID} - {acc.owner} - £{acc.balance}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;