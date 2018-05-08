import React from 'react';
import { Link } from 'react-router-dom';
import styles from './Login.css';

class Login extends React.Component {
    state ={
        email: '',
        password: ''
    };

    // Need to use a lifecycle hook to send data to backend on the submit button press
    


    // This method will update our inputs and store the inserted data into our state so we can send it to the server.
    keyPress(event) {
        const type = event.target.type;
        this.setState({
            [type]: event.target.value
        })
    }

    render() {

        return (
            <div className='container formContainer'>
                <form>
                    <div className="form-group">
                        <label>Email address</label>
                        <input value={this.state.email} type="email" className="noRadius form-control" 
                        aria-describedby="emailHelp" 
                        placeholder="Enter email"
                        onChange={(event) => this.keyPress(event)}
                        />
                    </div>
                    <div className="form-group">
                        <label>Password</label>
                        <input value={this.state.password} type="password" className="noRadius form-control" 
                        placeholder="Password"
                        onChange={(event) => this.keyPress(event)}
                        />
                    </div>
                    <button type="submit" className="btn btn-success" id='submitButton'>Login</button>
                </form>
                <Link to={'/'} className="registrationLink">Don't have an account? Create One Here</Link>
            </div>
        )


    }
}

export default Login;