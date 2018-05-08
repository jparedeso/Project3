import React from 'react';
import { Link } from 'react-router-dom';
import styles from './Registration.css';

class Registration extends React.Component {
    state ={
        firstName: '',
        lastName: '',
        email: '',
        password: '',
    };


    // need to add a method that will authenticate that all of the  
    registerUser(e) {
        e.preventDefault();
        const { firstName, lastName, email, password } = this.state;
        console.log(firstName, lastName, email, password);
        // fetch('https://mywebsite.com/endpoint/', {
        //     method: 'POST',
        //     headers: {
        //         'Accept': 'application/json',
        //         'Content-Type': 'application/json',
        //     },
        //     body: JSON.stringify({
        //         firstName,
        //         lastName,
        //         email,
        //         password
        //     })
        // })
        this.setState({
            firstName: '',
            lastName: '',
            email: '',
            password: '',
        })
    }


    // This method will update our inputs and store the inserted data into our state so we can send it to the server.
    keyPress(event) {
        const name = event.target.name;
        this.setState({
            [name]: event.target.value
        })
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
                    <button type="submit" className="btn btn-success"
                    id='submitButton'
                    onClick={(e) => this.registerUser(e)}
                    >
                        Register
                    </button>
                </form>
                <Link to={'/Login'} className="registrationLink">Already have an account? Login in Here</Link>
            </div>
        )


    }
}

export default Registration;