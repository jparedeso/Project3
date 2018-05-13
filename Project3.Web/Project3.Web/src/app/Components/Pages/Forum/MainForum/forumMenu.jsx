import React from 'react';
import './forumMenu.css';

const Menu = props => {
  const { menus } = props;
  return (
    <div>
      <div className="menu-container">
        <div id="menuItems" className="row">
          <div className="col-sm-3"><strong>FORUMS</strong>

          </div>
        </div>
      </div>
    </div>
  )

}

export default Menu;