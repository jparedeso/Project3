import React from 'react';
import './Game.css';

const Game = props => {
  const { games } = props;
  return (
    <div>
      {games.map( game  => {
        return (
          <div className="game-grid-container">
            <div id="gameStats" className="row">
              {/* <div className="col-sm-3 gameImage">
                <img className='' src={game.imageSrc} alt={game.imageSrc} />
              </div> */}
              <div className="col-sm-5 gameName"><strong>TITLE: </strong> {game.name}
              </div>
              <div className="col-sm-2 gameValue"><strong>EST. VALUE: </strong> {game.value}
              </div>
              <div className="col-sm-2 gameCondition"><strong>EST. CONDITION: </strong> {game.condition}
              </div>
              <div className="col-sm-1">
                <button id="edit">EDIT</button>
              </div>
              <div className="col-sm-1">
                <button id="save">SAVE</button>
              </div>
            </div>
            <div className="row">
              <div className="col-sm-2 gameImage">
                <img className='' src="" alt="" />
              </div>
              <div className="col-sm-10">
                <p className='gameSummary'><strong>SUMMARY:</strong> Lorem ipsum dolor sit amet consectetur adipisicing elit. Iusto earum voluptatem facere culpa quia expedita, sunt eaque officia est nesciunt mollitia aliquam. Neque velit reiciendis blanditiis, perferendis ipsam dicta molestiae?
                Lorem ipsum dolor sit amet consectetur adipisicing elit. Iusto earum voluptatem facere culpa quia expedita, sunt eaque officia est nesciunt mollitia aliquam. Neque velit reiciendis blanditiis, perferendis ipsam dicta molestiae? Iusto earum voluptatem facere culpa quia expedita, sunt eaque officia est nesciunt mollitia aliquam. Neque velit reiciendis blanditiis, perferendis ipsam dicta molestiae?
                </p>
              </div>
            </div>
          </div>
        )
      })}
    </div>
  )

}

export default Game;

        //             <div className='game-grid-container' key={game.id}>
        //                 <img src={game.imageSrc} alt={game.imageSrc} className='gameImage'/>
        //                 <span className='gameName'><strong>Title:</strong> {game.name} </span>
        //                     <span className='gameValue'><strong>Estimated Value:</strong> {game.value} </span>
        //                 <span className='gameCondition'><strong>Estimated Condition:</strong> {game.condition}</span>
        //                 <p className='gameSummary'><strong>Summary:</strong> Lorem ipsum dolor sit amet consectetur adipisicing elit. Iusto earum voluptatem facere culpa quia expedita, sunt eaque officia est nesciunt mollitia aliquam. Neque velit reiciendis blanditiis, perferendis ipsam dicta molestiae?</p>
        //                 <img id="edit" className="" src="redButton.png" role="button" />
        //                 <img id="save" className="" src="redButton.png" role="button" />
        //             </div>
     