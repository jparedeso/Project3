import React, { Component } from 'react';
import styles from './ForumWrapper.css';
import ForumMenu from '../ForumMenu/ForumMenu';
import ForumTitle from '../ForumTitle/ForumTitle';
import ForumDisplay from '../ForumDisplay/ForumDisplay';

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
            </div>
        )
    }
}

export default ForumWrapper;
