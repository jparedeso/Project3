import React, { Component } from 'react';
import Dropdown from '../Dropdown/Dropdown';
import './Navbar.css';

class Navabar extends Component {
    constructor() {
        super();

        this.state = {
            forumDropdown: ["General Discussion", "Reviews"],
            userDropdown: ["REGISTER", "LOGIN", "PROFILE", "SIGNOUT"],
            forumMenu: false,
            userMenu: false
        };

        //this.showforumDropdown = this.showforumDropdown.bind(this);
        this.showUserDropdown = this.showUserDropdown.bind(this);
        this.closeUserMenu = this.closeUserMenu.bind(this);
        this.closeForumMenu = this.closeForumMenu.bind(this);
    }

    //state = {
    //    forumDropdown: ["General Discussion", "Reviews"],
    //    userDropdown: ["Register", "Login", "Profile", "Sign Out"],
    //    forumMenu: false,
    //    userMenu: false
    //};

    showforumDropdown() {
        this.setState({
            forumMenu: true
        }, () => {
            document.addEventListener('click', this.closeForumMenu);
        });
    }

    showUserDropdown() {
        this.setState({
            userMenu: true
        }, () => {
            document.addEventListener('click', this.closeUserMenu);
        })
    }

    closeUserMenu(event) {
        if (!this.dropDownMenu.contains(event.target)) {
            this.setState({
                userMenu: false
            }, () => {
                document.removeEventListener('click', this.closeUserMenu);
            });
        };
    }

    closeForumMenu (event) {
        if (!this.dropDownMenu.contains(event.target)) {
            this.setState({
                forumMenu: false
            }, () => {
                document.removeEventListener('click', this.closeForumMenu);
            });
        };
    }

    
    render() {

        return (
            <div className='header'>
                <button onClick={this.showforumDropdown.bind(this)}
                        className="btn btn-success headerButtons"
                >
                    <i className="fas fa-caret-down"></i> test clicker forum
                </button>

                <button onClick={this.showUserDropdown}
                        className="btn btn-success headerButtons"
                >
                    <i className="fas fa-caret-down"></i> test clicker user
                </button>
                
                
                <div
                    className='menu dropDownDiv'
                    ref={element => {
                    this.dropDownMenu = element;
                }}
                >
                    { this.state.forumMenu ?
                        <div className='dropDownForum'>
                            <Dropdown items={this.state.forumDropdown} style='dropDownForumLinks' >
                                    Forums
                                </Dropdown>
                            </div>
                        : null }

                    { this.state.userMenu ?
                        <div className='dropDownUser'> 
                            <Dropdown items={this.state.userDropdown}
                            style='dropDownUserLinks'
                            >
                                User Option
                            </Dropdown>
                        </div>
                        : null }
                </div> 
            </div>
        );
    }
};

export default Navabar;