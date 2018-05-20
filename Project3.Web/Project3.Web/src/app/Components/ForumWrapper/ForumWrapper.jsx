import React, { Component } from 'react';
import styles from './ForumWrapper.css';
import ForumMenu from '../ForumMenu/ForumMenu';

class ForumWrapper extends Component {
    constructor() {
        super();

    }

    render() {
        return (
            <div>
                <ForumMenu />
            </div>
        )
    }
}

export default ForumWrapper;
