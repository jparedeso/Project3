import React, { Component } from 'react';
import styles from './ForumWrapper.css';
import ForumMenu from '../ForumMenu/ForumMenu';
import ForumTitle from '../ForumTitle/ForumTitle';
import ForumDisplay from '../ForumDisplay/ForumDisplay';
import UserPost from '../UserPost/UserPost';

class ForumWrapper extends Component {
    constructor() {
        super();

    }

    render() {
        return (
            <div>
                <ForumTitle />
                <ForumDisplay />
                <ForumMenu />
                <UserPost />
            </div>
        )
    }
}

export default ForumWrapper;
