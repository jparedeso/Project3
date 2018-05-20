import React, { Component } from 'react';
import RequestLogin from '../../RequestLogin/RequestLogin';
import Game from '../../Game/Game';

class Profile extends Component {
    constructor() {
        super();

        this.state = {
            loggedIn: true,
            gameCollection: [
                { name: 'Super Mario World', value: '$14.50', platform: 'snes', condition: 'Good', imageSrc: 'https://vignette.wikia.nocookie.net/mario/images/6/60/SMBBoxart.png/revision/latest?cb=20120609143443', id: 1 },
                { name: 'Zelda', value: '$35.25', platform: 'nes', condition: 'Alright', imageSrc: "https://www.nintendo.com/games/detail/the-legend-of-zelda-breath-of-the-wild-switch", id: 2 }
            ],
            value: []
        };

        
        // const nameSearch = this.state.gameCollection[0].name;
        // const api_call = `https://cors-anywhere.herokuapp.com/https://svcs.ebay.com/services/search/FindingService/v1?RESPONSE-DATA-FORMAT=JSON&categoryId=139973&keywords=${nameSearch},nes&categoryId=139973`;
        // console.log(api_call);
   
        
        const gameSearch = this.state.gameCollection;
        let value = this.state.value;

        for (var i = 0; i < gameSearch.length; i++) {
            const gameName = gameSearch[i].name;
            const platformName = gameSearch[i].platform;
            console.log(gameName);
            console.log(platformName);


            var settings = {
                "async": true,
                "crossDomain": true,
                "url": `https://cors-anywhere.herokuapp.com/https://svcs.ebay.com/services/search/FindingService/v1?RESPONSE-DATA-FORMAT=JSON&keywords=${gameName},${platformName}&categoryId=139973`,
                "method": "GET",
                "headers": {
                "X-EBAY-SOA-SERVICE-VERSION": "1.13.0",
                "X-EBAY-SOA-OPERATION-NAME": "findCompletedItems",
                "X-EBAY-SOA-SECURITY-APPNAME": "FuzzyJon-RetroGam-PRD-92cc9f5ed-70036e56",
                "Cache-Control": "no-cache",
                "Postman-Token": "2533f0d4-46b4-4ae3-90c5-bfa1c936774a"
                }
            }

            let that = this;
            
            $.ajax(settings).done(function (response) {
                var results = JSON.parse(response);
                // console.log(results);
                var items = results.findCompletedItemsResponse[0].searchResult[0].item;
                var priceArray = [];
        
                for (var i = 0; i < items.length; i++) {
                    // console.log(items[i]);
                    var sellingState = items[i].sellingStatus[0].sellingState;
                    var sellingPrice = items[i].sellingStatus[0].currentPrice[0].__value__;
                    // console.log(sellingState);
                    // var sum = 0;
                    if (sellingState == "EndedWithSales") {
                        // console.log("true");
                        // console.log(sellingPrice);
                        priceArray.push(sellingPrice);
                        // console.log(priceArray);
                    }
                }
                // console.log(priceArray);
                
                var sum = 0;
        
                for (var j = 0; j < priceArray.length; j++) {
                    sum += parseInt( priceArray[j])
                }
        
                var averagePrice = sum/priceArray.length;
                // console.log(sum);
                console.log("game name: " + gameName + "game price: " + averagePrice);

                value.push(averagePrice);
                
                that.setState({ value: value });
                console.log(that.state.value, "++");
            });

        }
        
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
                        values={this.state.value}
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