import React from 'react';
import './TestHeader.css';

// Image imports
import arrowSrc from './Images/arrows.jpg';
import buttonSrc from './Images/redButton.png';
// Component imports
import DropDownButton from '../DropDownButton/DropDownButton';

class Header extends React.Component {
    constructor() {
        super();
        this.state = {
            forumDropDown: false,
            userDropDown: false,
        }
    }

    forumsDropDown() {
        this.setState({
            forumDropDown: !this.state.forumDropDown
        });
    }

    userDropDown() {
        this.setState({
            userDropDown: !this.state.userDropDown
        });
    }
    

    render() {
        return (
            <header className="sticky">
                <div className="container">
                <div className="row">
                    <div className="col-sm-3">
                        <img src={arrowSrc} />
                    </div>
                <div className="col-sm-4">
                    <div className="middle"></div>
                    <div className="middle"></div>
                    <div id="text" className="middle">
                        <div className="row">
                        <div className="col-sm-6">
                            <p id="select" className="noFcn">SELECT</p>
                        </div>
                        <div className="col-sm-6">
                            <p id="start" className="noFcn">START</p>
                        </div>
                        </div>
                    </div>
                    <div id="lrg" className="middle">
                        <button className="blkButtons"></button>
                        <button className="blkButtons"></button>
                    </div>
                </div>
                <div className="col-sm-5">
                        <div className="row">
                            <h2>APP NAME HERE</h2>
                        </div>
                        <div
                        ref = {element => {
                            this.dropDownMenu = element;
                        }}>
                            <DropDownButton 
                            forumBool = {this.state.forumDropDown}
                            forumMethod = {() => this.forumsDropDown()}
                            userBool = {this.state.userDropDown}
                            userMethod = {() => this.userDropDown()}
                            />
                        </div>

                        
                    {/* <div className="row">
                        <div className="col-sm-5 dropdown">
                            <img id="forumsBtn" role="button" className="redButton dropbtn" src={buttonSrc} onclick={() => someMethod()} />
                        <div id="forumsDrpDwn" className="dropdown-menu dropdown-menu-right">
                            <a href="#">FORUMS HOME</a>
                            <a href="#">Link 2</a>
                            <a href="#">Link 3</a>
                        </div>
                            <p id="forums">FORUMS</p>
                        </div>
                        <div className="col-sm-5 dropdown">
                            <img id="userBtn" role="button" className="redButton dropbtn" src={buttonSrc} onclick="showUserMenu()" />
                        <div id="userDrpDwn" className="dropdown-menu dropdown-menu-right">
                            <a href="#">REGISTER</a>
                            <a href="#">LOG-IN</a>
                            <a href="#">PROFILE</a>
                            <a href="#">SIGN-OUT</a>
                        </div>
                            <p id="user">USER</p>
                        </div>
                    </div> */}
                    </div>
                </div>      
                </div>
            </header>
        )
    }

}

export default Header;
