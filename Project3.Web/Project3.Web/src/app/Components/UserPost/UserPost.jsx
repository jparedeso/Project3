import React from 'react';
import './UserPost.css';

const UserPost = props => {
  return (
    <div className="container">
      <div className="row commentComponent">
        <div className="col-sm-2 col-sm-offset-2 userInfoCard">
          <div className="card">
            <img className="card-img-top" src="https://vignette.wikia.nocookie.net/disney/images/d/d5/Donaldstar_1600.jpg/revision/latest?cb=20100629001545" alt="Card image"></img>
              <div className="card-body">
                <h4 className="card-title">D-Duck</h4>
                <p className="card-text">Member of the kingdom since: 2017</p>
              </div>
          </div>
        </div>
        <div className="col-sm-7 userComment">
          <p>User comment here. AND NO THESE PICS ARE FOR REAL, JUST HAVING FUN :)
          </p>
            <img role="button" className="up" src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTJbgvalDYmoSjhnLiAOos5FsxisuX6TUSWgIG7zPIB62NM74A9">
              {/* <!-- <img className="down" src="thumbdown.png"> --> */}</img>
        </div>
      </div>
    </div>
  )
}

export default UserPost;