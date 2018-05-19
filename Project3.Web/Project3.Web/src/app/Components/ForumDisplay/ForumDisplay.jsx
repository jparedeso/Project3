import React from 'react';
import './ForumDisplay.css';

const ForumDisplay = props => {
  return (
    <div className="container">
    <div className="row">
    <div className="col-sm-9 col-sm-col-lg-offset-2">
      <table className="table">
        <thead>
          <tr>
            <th className="topRow">[THREAD TOPIC NAME]<button id="addThread">NEW THREAD</button></th>
            <th className="align topRow">AUTHOR</th>
            <th className="align topRow">CREATED</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td><a className="artTitle" href="#">Donkey Kong Sucks!</a></td>
            <td className="align">Luigi</td>
            <td className="align">4:43pm</td>
          </tr>
          <tr>
            <td><a className="artTitle" href="#">I think they should bring back duck hunt!</a></td>
            <td className="align">D-Duck</td>
            <td className="align">3:04am</td>
          </tr>
          <tr>
            <td><a className="artTitle" href="#">Bowzer is really a gentle giant, not a filty rat!</a></td>
            <td className="align">Link</td>
            <td className="align">11:21am</td>
          </tr>
          <tr>
            <td>
              <a className="artTitle" href="#">Donkey Kong Sucks!</a>
            </td>
            <td className="align">Luigi</td>
            <td className="align">4:43pm</td>
          </tr>
          <tr>
            <td>
              <a className="artTitle" href="#">Donkey Kong Sucks!</a>
            </td>
            <td className="align">Luigi</td>
            <td className="align">4:43pm</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div >
</div>
  )
}

export default ForumDisplay;