import React from 'react';
import './DropDownButton.css';
import arrowSrc from './Images/arrows.jpg';
import buttonSrc from './Images/redButton.png';
import { Link } from 'react-router-dom';

const DropDownButton = props => {
    const { userBool, forumBool } = props;
    return (
        <div className="row"
        >
            <div className="col-sm-5 dropdown">
                <img id="forumsBtn" role="button" className="redButton dropbtn" src={buttonSrc} onClick={() => props.forumMethod()} />
            { forumBool ? 
            <div id="forumsDrpDwn" className="dropdown-menu dropdown-menu-right">
                <Link to={'/Forum'} >Forum Home</Link>
                <a href="#">Link 2</a>
                <a href="#">Link 3</a>
            </div> : null
            }
                <p id="forums">FORUMS</p>
            </div>

            
            <div className="col-sm-5 dropdown">
                <img id="userBtn" role="button" className="redButton dropbtn" src={buttonSrc} onClick={() => props.userMethod()} />
            { userBool ?
            <div id="userDrpDwn" className="dropdown-menu dropdown-menu-right">
                 <Link to={'/'} >Register</Link>
                 <Link to={'/Login'} >Login</Link>
                 <Link to={'/Profile'} >Profile</Link>
                <a href="#">SIGN-OUT</a>
            </div> : null
            }
                <p id="user">USER</p>
            </div>
            
        </div>
            
    )
}

export default DropDownButton;