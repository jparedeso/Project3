import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import axios from "axios";
const serialize = require('form-serialize');
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
                <div className="row">
                    <p id="regReq">WAIT... THE  PRINCESS  REQUESTS  THAT  YOU  REGISTER  OR  LOGIN  BEFORE  ENTERING THE  KINGDOM!</p>
                    <br />
                    <div id="form" className="col-sm-6">
                        <form id="registerForm">
                            <div className="form-group">
                                <label>FIRST NAME</label>
                                <input value={this.state.FirstName} type="text" className="noRadius form-control"
                               name='FirstName'
                               aria-describedby="EmailHelp"
                               placeholder="FIRST NAME"
                               onChange={(event) => this.keyPress(event)}/>
                            </div>
                            <div className="form-group">
                             <label>LAST NAME</label>
                                <input value={this.state.LastName} type="text" className="noRadius form-control"
                               name='LastName'
                               placeholder="LAST NAME"
                               onChange={(event) => this.keyPress(event)}/>
                            </div>
                            <div className="form-group">
                                <label>EMAIL ADDRESS</label>
                                <input value={this.state.Email} type="Email" className="noRadius form-control"
                               name='Email'
                               aria-describedby="EmailHelp"
                               placeholder="EXAMPLE@EMAIL.MAIL"
                               onChange={(event) => this.keyPress(event)}/>
                            </div>
                            <div className="form-group">
                                <label>PASSWORD</label>
                                <input value={this.state.Password} type="Password" className="noRadius form-control"
                               name='Password'
                               placeholder="PASSWORD"
                               onChange={(event) => this.keyPress(event)}/>
                            </div>
                            <div className="form-group">
                                <label>CONFIRM PASSWORD</label>
                                <input value={this.state.ConfirmPassword} type="Password" className="noRadius form-control"
                               name='ConfirmPassword'
                               placeholder="CONFIRM PASSWORD"
                               onChange={(event) => this.keyPress(event)} />
                            </div>
                            <input type="hidden" name="__RequestVerificationToken" value={this.state.__RequestVerificationToken} />
                            <button type="submit" className="btn btn-success"
                            id='submitButton'
                            onClick={(e) => this.registerUser(e)}>
                                 REGISTER
                            </button>
                            <br/>
                            <Link to={'/Login'} className="registrationLink">ALREADY HAVE AN ACCOUNT? LOGIN HERE</Link>
                        </form>
                    </div>
                    <div id="marioPipe" className="col-sm-6">
                        {/* ////////////////////////////////// */}
                        <div className="marioPic">
                            <img id="mario" src="https://reichanjapan.files.wordpress.com/2016/02/mariogiftcard.png?w=230&h=335" />
                        </div>
                    </div>
                </div>
            </div>
        );


    }
}

export default Registration;