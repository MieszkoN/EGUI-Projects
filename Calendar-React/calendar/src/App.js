import React from 'react';
import './App.css';
import Calendar from './Components/Calendar';
import { BrowserRouter, Switch, Route} from 'react-router-dom/cjs/react-router-dom.min';
import { SingleDay } from './Components/Calendar/singleDay';

function App() {
  console.log('refreshing')
  return (
    <BrowserRouter>
      <div className="App">
        <Switch>
          <Route path='/' component = {Calendar} exact/>
          <Route path='/singleDay' component = {SingleDay} />
        </Switch>
      </div>
    </BrowserRouter>
  );
}

export default App;
