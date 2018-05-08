import React from 'react';
import { Link } from 'react-router-dom';

const RequestLogin = () => {

    const styles = {
        textAlign: 'center'
    }

    return (
    <div style={styles}>
        <h3>You need to Login or Register in order to keep track of your collection</h3>
        <Link type='button' to={'/Login'} className="registrationLink">Login Here</Link>
   </div>
    );
}

export default RequestLogin;