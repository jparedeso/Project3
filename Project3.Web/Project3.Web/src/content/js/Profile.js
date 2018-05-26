// Just a comment to have something in file so files push

// add game, needs to run api first to return the data we want to send to Db. newGame is just using value of gameTitle for test
$(document).ready(function(){  

    $("#addGameBtn").on("click", function(event) {
        event.preventDefault();
        var newGame = {
            Name: $(".gameTitle").val().trim()
        };

        console.log(newGame);

        // Send the POST request to create new Game.

        // $.ajax("/api/game", {
        //     type: "POST",
        //     data: newGame
        // }).then(
        //     function() {
        //         console.log("added new game");
        //     }
        // );
    });

    

});


// ebay api in a function, still needs proper get route to test and input proper data
function gameValue() {
    var value = [];
    
    $.ajax({
        url: "/api/games",
        method: "GET",
        success: function(data) {

            for (var i = 0; i < data.length; i++) {
                const gameName = data[i].Name;
                const platformName = data[i].Platform;
                // console.log(gameName);
                // console.log(platformName);

                var settings = {
                    "async": false,
                    "crossDomain": true,
                    "url": "https://cors-anywhere.herokuapp.com/https://svcs.ebay.com/services/search/FindingService/v1?RESPONSE-DATA-FORMAT=JSON&keywords=" + gameName + "," + platformName + "&categoryId=139973",
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
            
                    var average = sum/priceArray.length;
                    // console.log(sum);
                    // console.log("game name: " + gameName + "game price: " + average);

                    var averagePrice = average.toFixed(2);

                    value.push(averagePrice);
                    
                    // that.setState({ value: value });
                    // console.log(that.state.value, "++");
                });

            }
        }

    });
}