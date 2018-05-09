import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import styles from './Registration.css';

class Registration extends Component {
    constructor() {
        super();

        this.state = {
            firstName: '',
            lastName: '',
            displayName: '',
            email: '',
            password: '',
            passwordConfirm: '',
        };
    };

    passwordCheck(e) {
        e.preventDefault();
        const { password, passwordConfirm } = this.state;
        if (password === passwordConfirm) {
            alert('The password check worked');
            this.auth();
        } else {
            alert("Your passwords are different dumbass");
        }
    };

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
    };


    registerUser() {
        alert('valid input, sending request');
        const { firstName, lastName, displayName, email, password } = this.state;
        console.log(firstName, lastName, displayName, email, password);
        console.log(this.state);
        // fetch('Account/Register', {
        //     method: 'POST',
        //     headers: {
        //         'Accept': 'application/json',
        //         'Content-Type': 'application/json',
        //     },
        //     body: JSON.stringify({
        //         firstName,
        //         lastName,
        //         displayName,
        //         email,
        //         password
        //     })
        // });
        // const xhr = new XMLHttpRequest();
        // xhr.open("POST", "Register", true);
        // xhr.onload = function() {
        //     console.log(this.responseText);
        //     console.log(this.status);
        // }
        // xhr.send({
        //     firstName,
        //     lastName,
        //     displayName,
        //     email,
        //     password
        // });
        this.setState({
            firstName: '',
            lastName: '',
            displayName: '',
            email: '',
            password: '',
            passwordConfirm: '',
        });
    };


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
                <form>
                    <div className="form-group">
                        <label>First Name</label>
                        <input value={this.state.firstName} type="text" className="noRadius form-control" 
                        name='firstName'
                        aria-describedby="emailHelp" 
                        placeholder="Enter First Name"
                        onChange={(event) => this.keyPress(event)}
                        />
                    </div>
                    <div className="form-group">
                        <label>Last Name</label>
                        <input value={this.state.lastName} type="text" className="noRadius form-control" 
                        name='lastName'                       
                        placeholder="Enter Last Name"
                        onChange={(event) => this.keyPress(event)}
                        />
                    </div>
                    <div className="form-group">
                        <label>Display / Username</label>
                        <input value={this.state.displayName} type="text" className="noRadius form-control" 
                        name='displayName'                       
                        placeholder="Enter a Display Name"
                        onChange={(event) => this.keyPress(event)}
                        />
                    </div>
                    <div className="form-group">
                        <label>Email address</label>
                        <input value={this.state.email} type="email" className="noRadius form-control"
                        name='email'
                        aria-describedby="emailHelp" 
                        placeholder="Enter email"
                        onChange={(event) => this.keyPress(event)}
                        />
                    </div>
                    <div className="form-group">
                        <label>Password</label>
                        <input value={this.state.password} type="password" className="noRadius form-control" 
                        name='password'
                        placeholder="Password"
                        onChange={(event) => this.keyPress(event)}
                        />
                    </div>
                    <div className="form-group">
                        <label>Confirm Password</label>
                        <input value={this.state.passwordConfirm} type="password" className="noRadius form-control" 
                        name='passwordConfirm'
                        placeholder="Confirm your password"
                        onChange={(event) => this.keyPress(event)}
                        />
                    </div>
                    <button type="submit" className="btn btn-success"
                    id='submitButton'
                    onClick={(e) => this.passwordCheck(e)}
                    >
                        Register
                    </button>
                </form>
                <Link to={'/Login'} className="registrationLink">Already have an account? Login in Here</Link>
            </div>
        );


    }
}

export default Registration;