import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import axios from "axios";
var serialize = require('form-serialize');
import styles from './Registration.css';

class Registration extends Component {
    constructor() {
        super();

        this.state = {
            FirstName: '',
            LastName: '',
            Email: '',
            Password: '',
            ConfirmPassword: '',
            __RequestVerificationToken: ''
        };
    }

    componentWillMount() {
        axios.get("XsrfToken/Get").then(res => {
            this.setState({ __RequestVerificationToken: res.data.token });
        });
    }

    // need to add a method that will authenticate that all of the  
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

    passwordCheck(e) {
        e.preventDefault();
        const { password, passwordConfirm } = this.state;
        if (password === passwordConfirm) {
            alert('The password check worked');
            this.auth();
        } else {
            alert("Your passwords are different dumbass");
        }
    }

    auth() {
        // loop over state
        // for each value, check to make sure that only letters, @, and numbers are in valid.
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


    // This method will update our inputs and store the inserted data into our state so we can send it to the server.
    keyPress(event) {
        const name = event.target.name;
        this.setState({
            [name]: event.target.value
        });
    }

    render() {

        return (
            <div className='container formContainer'>
                <form id="registerForm">
                    <div className="form-group">
                        <label>First Name</label>
                        <input value={this.state.FirstName} type="text" className="noRadius form-control"
                               name='FirstName'
                               aria-describedby="EmailHelp"
                               placeholder="Enter First Name"
                               onChange={(event) => this.keyPress(event)}/>
                    </div>
                    <div className="form-group">
                        <label>Last Name</label>
                        <input value={this.state.LastName} type="text" className="noRadius form-control"
                               name='LastName'
                               placeholder="Enter Last Name"
                               onChange={(event) => this.keyPress(event)}/>
                    </div>
                    <div className="form-group">
                        <label>Email address</label>
                        <input value={this.state.Email} type="Email" className="noRadius form-control"
                               name='Email'
                               aria-describedby="EmailHelp"
                               placeholder="Enter Email"
                               onChange={(event) => this.keyPress(event)}/>
                    </div>
                    <div className="form-group">
                        <label>Password</label>
                        <input value={this.state.Password} type="Password" className="noRadius form-control"
                               name='Password'
                               placeholder="Password"
                               onChange={(event) => this.keyPress(event)}/>
                    </div>
                    <div className="form-group">
                        <label>Confirm Password</label>
                        <input value={this.state.ConfirmPassword} type="Password" className="noRadius form-control"
                               name='ConfirmPassword'
                               placeholder="Confirm Password"
                               onChange={(event) => this.keyPress(event)} />
                    </div>
                    <input type="hidden" name="__RequestVerificationToken" value={this.state.__RequestVerificationToken} />
                    <button type="submit" className="btn btn-success"
                            id='submitButton'
                            onClick={(e) => this.registerUser(e)}>
                        Register
                    </button>
                </form>
                <Link to={'/Login'} className="registrationLink">Already have an account? Login in Here</Link>
            </div>
        );


    }
}

export default Registration;