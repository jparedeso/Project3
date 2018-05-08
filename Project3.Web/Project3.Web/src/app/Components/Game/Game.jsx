import React from 'react';
import './Game.css';

const Game = props => {
    const { games } = props;
    return (
        <div>
            {games.map( game  => {
                return (
                    <div className='game-grid-container' key={game.id}>
                        <img src={game.imageSrc} alt={game.imageSrc} className='gameImage'/>
                        <span className='gameName'> Title: {game.name} </span>
                        <span className='gameValue'>Estimated Value: {game.value} </span>
                        <span className='gameCondition'>Estimated Condition: {game.condition}</span>
                        <p className='gameSummary'><strong>Summary:</strong> Lorem ipsum dolor sit amet consectetur adipisicing elit. Iusto earum voluptatem facere culpa quia expedita, sunt eaque officia est nesciunt mollitia aliquam. Neque velit reiciendis blanditiis, perferendis ipsam dicta molestiae?</p>
                    </div>
                )
            })}
        </div>
    )

}

export default Game;