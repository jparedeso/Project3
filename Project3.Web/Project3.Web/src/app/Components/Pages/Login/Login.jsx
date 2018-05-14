import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import styles from './Login.css';
import axios from "axios";
const serialize = require('form-serialize');

class Login extends Component {
    constructor() {
        super();

        this.state = {
            email: '',
            password: ''
        };
    };

    componentWillMount() {
        axios.get("XsrfToken/Get").then(res => {
            this.setState({ __RequestVerificationToken: res.data.token });
        });
    };

    auth() {
        // loop over state
        // for each value, check to make sure that only letters, @, and numbers are valid.
        const userObj = Object.assign({}, this.state);
        for (var property in userObj) {
            userObj[property]
            if (/[^a-zA-Z0-9@\-\/\s\.]/.test(userObj[property])) {
                alert("One of the props had an invalid character");
                return;
            };
            
            // INVALID: Undefined
            if (userObj[property] === "" || undefined) {
                alert("One of the props was empty or undefined");
                return;
            };
        }
        this.registerUser();
    }


    // Refactor this code
    registerUser(e) {
        e.preventDefault();
        //const { FirstName, LastName, Email, Password, ConfirmPassword, __RequestVerificationToken } = this.state;
        //console.log(FirstName, LastName, Email, Password, ConfirmPassword);
        axios.post("Account/Register", serialize(document.querySelector("#registerForm")),
            {
                withCredentials: true,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            }).then(() => {
                this.setState({
                    FirstName: '',
                    LastName: '',
                    Email: '',
                    Password: '',
                    ConfirmPassword: '',
                    __RequestVerificationToken: ''
                });
            });
        
    }
   

    


    // This method will update our inputs and store the inserted data into our state so we can send it to the server.
    keyPress(event) {
        const type = event.target.type;
        this.setState({
            [type]: event.target.value
        });
    }

    render() {

        return (
            <div id="main" className='container formContainer'>
                <div className="row">
                    <div className="col-sm-12">
                    <p id="regReq">WAIT... THE  PRINCESS  REQUESTS  THAT  YOU  REGISTER  OR  LOGIN  BEFORE  ENTERING THE  KINGDOM!
                    </p>
                    <br />
                    </div>
                </div>
                <div id="logInForm"className="row">
                <form>
                    <div className="form-group">
                        <label>EMAIL ADDRESS</label>
                        <input value={this.state.email} type="email" className="noRadius form-control" 
                        aria-describedby="emailHelp" 
                        placeholder="EXAMPLE@EMAIL.MAIL"
                        onChange={(event) => this.keyPress(event)}
                        />
                    </div>
                    <div className="form-group">
                        <label>PASSWORD</label>
                        <input value={this.state.password} type="password" className="noRadius form-control" 
                        placeholder="PASSWORD"
                        onChange={(event) => this.keyPress(event)}
                        />
                    </div>
                    <button type="submit" className="btn btn-success" id='submitButton'>LOGIN</button>
                    <br />
                    <Link to={'/'} className="registrationLink">DON'T HAVE AN ACCOUNT? CREATE ONE HERE</Link>
                </form>
                </div>
            </div>
        )


    }
}

export default Login;