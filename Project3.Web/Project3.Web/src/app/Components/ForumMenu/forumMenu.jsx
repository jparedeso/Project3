import React from 'react';
import './ForumMenu.css';

const ForumMenu = props => {
  return (
    <div className="container">
      <div className="row">
        <h3 className="welcome">{WHICHEVER} FORUM</h3>
      </div>
      <div className="row">
        <div className="col-sm-2 forumMenu sticky">
          <a href="" className="forumTitle">GENERAL</a><br/>
            <a href="" className="forumTitle">REVIEW</a><br/>
              <p className="forumTitle">PLATFORMS</p>
            <ul>
              <a className="platform" href="">Atari 2600</a><br/>
              <a className="platform" href="">NES</a><br/>
              <a className="platform" href="">Nintendo 64</a><br/>
              <a className="platform" href="">Nintendo Gameboy</a><br/>
              <a className="platform" href="">Super Nintendo</a><br/>
              <a className="platform" href="">Playstation</a><br/>
              <a className="platform" href="">Playstation2</a><br/>
              <a className="platform" href="">Sega Genesis</a><br/>
              <a className="platform" href="">Sega Master System</a><br/>
              <a className="platform" href="">Xbox</a><br/>
            </ul>
        </div>
      </div>
      </div>
  )
}
export default ForumMenu;