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
                        <span className='gameName'><strong>Title:</strong> {game.name} </span>
                            <span className='gameValue'><strong>Estimated Value:</strong> {game.value} </span>
                        <span className='gameCondition'><strong>Estimated Condition:</strong> {game.condition}</span>
                        <p className='gameSummary'><strong>Summary:</strong> Lorem ipsum dolor sit amet consectetur adipisicing elit. Iusto earum voluptatem facere culpa quia expedita, sunt eaque officia est nesciunt mollitia aliquam. Neque velit reiciendis blanditiis, perferendis ipsam dicta molestiae?</p>
                        <img id="edit" className="" src="redButton.png" role="button" />
                        <img id="save" className="" src="redButton.png" role="button" />
                    </div>
                )
            })}
        </div>
    )

}

export default Game;