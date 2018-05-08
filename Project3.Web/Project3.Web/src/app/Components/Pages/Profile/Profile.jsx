import React, { Component } from 'react';
import RequestLogin from '../../RequestLogin/RequestLogin';
import Game from '../../Game/Game';

class Profile extends Component {
    constructor() {
        super();

        this.state = {
            loggedIn: false,
            gameCollection: [
                { name: 'Mario', value: '$25.97', condition: 'Good', imageSrc: '/images/sm64.jpg', id: 1 },
                { name: 'Zelda', value: '$18.75', condition: 'Alright', imageSrc: '/images/zelda64.jpg', id: 1 }
            ]
        };
    }


    //state = {
    //    loggedIn: false,
    //    gameCollection: [
    //        {name: 'Mario', value: '$25.97', condition: 'Good', imageSrc: '/images/sm64.jpg', id: 1 },
    //        {name: 'Zelda', value: '$18.75', condition: 'Alright', imageSrc: '/images/zelda64.jpg', id: 1 }
    //    ]
    //};

    // If a user is not logged in. We want to render something that informs
    // the user to log in if they wish to view their collection



    render() {

        const styles = {
            textAlign: 'center'
        }



        return (
            <div>
                { this.state.loggedIn ? 
                    <div styles={styles}>
                        <Game 
                        games={this.state.gameCollection}
                        />
                    </div> :
                    <RequestLogin/>
                }
            </div>
        );
    }
};

export default Profile;