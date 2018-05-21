import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';
import { BrowserRouter as Router, Route, Link } from 'react-router-dom';
import Login from './Components/Pages/Login/Login';
import Registration from './Components/Pages/Registration/Registration';
import Navbar from './Components/Navbar/Navbar';
import Profile from './Components/Pages/Profile/Profile';
import Header from './Components/TestHeader/TestHeader';
import ForumWrapper from './Components/ForumWrapper/ForumWrapper';

class App extends Component {
  render() {
    return (
      <div>
        <Router>

          <div>
          <Header />

            <Route exact path="/" component={Registration} />
            <Route exact path="/Login" component={Login} />
            <Route exact path='/Profile' component={Profile}/>
            <Route exact path='/Forum' component={ForumWrapper}/>
          </div>
          {/* <div className="App">
            <header className="App-header">
              <img src={logo} className="App-logo" alt="logo" />
            </header>
            <Registration/>
          </div> */}
        </Router>
      </div>
    );
  }
}

export default App;
