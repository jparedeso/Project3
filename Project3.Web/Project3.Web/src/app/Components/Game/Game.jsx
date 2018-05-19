import React from 'react';
import './Game.css';

const Game = props => {
  const { games } = props;
  return (
    <div>
    <div className="inventory">
      <h1>INVENTORY</h1>
    </div>
    <div>
      {games.map( game  => {
        return (
          <div className="game-grid-container">
            <div id="gameStats" className="row">
              {/* <div className="col-sm-3 gameImage">
                <img className='' src={game.imageSrc} alt={game.imageSrc} />
              </div> */}
              <div className="col-sm-3 gameName"><strong>TITLE: </strong> {game.name}
              </div>
              <div className="col-sm-2 gameValue"><img id="coin" src="https://media0.giphy.com/media/yCyVbqru5Ggfu/giphy.gif"></img><strong>EST. VALUE: </strong> {game.value}
              </div>
              <div className="col-sm-2 gameCondition dropdown-toggle" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><strong>EST. CONDITION: </strong> {game.condition}
                {/* <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton">
                <a class="dropdown-item" href="#"><img class="lifeHeart" scr="https://pbs.twimg.com/profile_images/554699922138624000/0AopZpk4_400x400.png"></img></a>
                <a class="dropdown-item" href="#">4 HEARTS</a>
                <a class="dropdown-item" href="#">3 HEARTS</a>
                <a class="dropdown-item" href="#">2 HEARTS</a>
                <a class="dropdown-item" href="#">1 HEART</a>
              </div> */}
              </div>
              <div className="col-sm-2 gamePlatform"><strong>PLATFORM: </strong> {game.platform}
              </div>
              <div className="col-sm-2 actions">
                <img id="edit" src="https://png.icons8.com/metro/1600/edit-property.png" role="button"></img>
                <img id="save" src="https://cdn4.iconfinder.com/data/icons/STROKE/computer_gadgets/png/400/floppy_disk.png" role="button"></img>
                <img id="delete" src="https://png.icons8.com/metro/1600/delete.png" role="button"></img>
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
     