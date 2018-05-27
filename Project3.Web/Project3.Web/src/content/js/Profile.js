const Profile = function () {

    function init() {
        initEventHandlers();
        getUserGames();
    }

    function initEventHandlers() {
        //        addGame();

        // a test function to see if games can append dynamically. function is not displayGames() below add game, its below value function.
        // this function will be be removed and not called, and instead displayGames() will be used once we have the api route.
        //        displayGamesTest();
        searchGames();
    }

    function searchGames() {
        $("#addGameBtn").on("click", function () {
            //Ideally we want to just HIGHLIGHT the current game selection, and then ADD it when we PRESS this button.
        });

        $("#searchGameForm").on("submit", function (e) {
            e.preventDefault();

            const name = $(".gameTitle").val().trim();

            $.ajax({
                url    : this.action,
                method : this.method,
                data   : {name},
                success: response => {
                    console.log(response);
                    $("#gameReturn").html("");
                    for (let i = 0; i < response.length; i++) {
                        $("#gameReturn").append(`
                        <div class="gameSelection" data-id="${response[i].gameId}">${response[i].name}</div>
                        `);
                    }
                    $(".gameSelection").on("click", function () {
                        const id = $(this).attr("data-id");

                        $.ajax({
                            url: "/Games/InsertGame",
                            method: "POST",
                            data: {id},
                            success: response => {
                                console.log(response);
                                $('#gameSearchModal').modal('hide');
                                getUserGames();
                            }
                        });
                    });
                }
            });
        });
    }

    function getUserGames() {
        $.ajax({
            url: "/Games/SearchUserGames",
            method: "GET",
            success: response => {
                $("#displayGames").html("");
                for (var i = 0; i < response.length; i++) {
                    $("#displayGames").append(`
                    <div class="gameDisplay" data-id="${response[i].gameId}">${response[i].name}</div>
                    `);
                }
            }
        });
    }

    function addGame() {
        $("#addGameBtn").on("click", function (event) {
            event.preventDefault();
            const newGame = {
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
    }

    function displayGames() {
        if ($("#displayGames")[0]) {
            $.ajax({
                url    : "/api/games/",
                method : "GET",
                success: function (data) {
                    console.log(data);
                    // empty to displayGames before adding new content
                    $("#displayGames").empty();
                    // if the data is not there, then return an error message
                    if (!data) {
                        $("#displayGames").append("<h2> I'm sorry, but there aren't any games yet</h2>");
                    }
                    else {
                        for (let i = 0; i < data.length; i++) {
                            // the title, platform, condition, image, summary, and value will all be data.whatever from DB once api route is given and data can be tested.
                            $("#displayGames").append(
                                `
                                <div class="game-grid-container">
                                    <div id="gameStats" class="row">
                                    <div class="col-sm-3 gameName">
                                        <strong>TITLE: </strong>Zelda: Breath of the Wild
                                    </div>
                                    <div class="col-sm-2 gameValue">
                                        <img id="coin" src="https://media0.giphy.com/media/yCyVbqru5Ggfu/giphy.gif">
                                        <strong>EST. VALUE: </strong> $41.59
                                    </div>
                                    <div class="col-sm-2 gameCondition dropdown-toggle" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                        <strong id="conditionBtn" role="button">EST. CONDITION: </strong> EXCELLENT
                                        <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton">
                                        <a class="dropdown-item" href="#">
                                            <img class="lifeHeart" scr="https://pbs.twimg.com/profile_images/554699922138624000/0AopZpk4_400x400.png">
                                        </a>
                                        <a class="dropdown-item" href="#">4 HEARTS</a>
                                        <a class="dropdown-item" href="#">3 HEARTS</a>
                                        <a class="dropdown-item" href="#">2 HEARTS</a>
                                        <a class="dropdown-item" href="#">1 HEART</a>
                                        </div>
                                    </div>
                                    <div class="col-sm-2 gamePlatform">
                                        <strong>PLATFORM: </strong> NES
                                    </div>
                                    <div class="col-sm-2 actions">
                                        <img id="edit" src="https://png.icons8.com/metro/1600/edit-property.png" role="button">
                                        <img id="save" src="https://cdn4.iconfinder.com/data/icons/STROKE/computer_gadgets/png/400/floppy_disk.png" role="button">
                                        <img id="delete" src="https://png.icons8.com/metro/1600/delete.png" role="button">
                                    </div>
                                    </div>
                                    <div class="row">
                                    <div class="col-sm-2 gameImage">
                                        <img class='' src="" alt="" />
                                    </div>
                                    <br>
                                    <div class="col-sm-10">
                                        <p class='gameSummary'>
                                        <strong>SUMMARY:</strong> Lorem ipsum dolor sit amet consectetur adipisicing elit. Iusto earum voluptatem facere culpa
                                        quia expedita, sunt eaque officia est nesciunt mollitia aliquam. Neque velit reiciendis blanditiis, perferendis
                                        ipsam dicta molestiae? Lorem ipsum dolor sit amet consectetur adipisicing elit. Iusto earum voluptatem facere culpa
                                        quia expedita, sunt eaque officia est nesciunt mollitia aliquam. Neque velit reiciendis blanditiis, perferendis
                                        ipsam dicta molestiae? Iusto earum voluptatem facere culpa quia expedita, sunt eaque officia est nesciunt mollitia
                                        aliquam. Neque velit reiciendis blanditiis, perferendis ipsam dicta molestiae?
                                        </p>
                                    </div>
                                    </div>
                                </div>
                                `
                            );
                        }
                    }
                }
            });
        }
    }

    function gameValue() {
        // might exist outside function between the display games and the data loop
        let gameName = response[i].Name;
        let platformName = response[i].Platform;

        const settings = {
            "async"      : false,
            "crossDomain": true,
            "url"        : "https://cors-anywhere.herokuapp.com/https://svcs.ebay.com/services/search/FindingService/v1?RESPONSE-DATA-FORMAT=JSON&keywords=" + gameName + "," + platformName + "&categoryId=139973",
            "method"     : "GET",
            "headers"    : {
                "X-EBAY-SOA-SERVICE-VERSION" : "1.13.0",
                "X-EBAY-SOA-OPERATION-NAME"  : "findCompletedItems",
                "X-EBAY-SOA-SECURITY-APPNAME": "FuzzyJon-RetroGam-PRD-92cc9f5ed-70036e56",
                "Cache-Control"              : "no-cache",
                "Postman-Token"              : "2533f0d4-46b4-4ae3-90c5-bfa1c936774a"
            }
        };

        $.ajax(settings).done(function (response) {
            const results = JSON.parse(response);
            // console.log(results);
            const items = results.findCompletedItemsResponse[0].searchResult[0].item;
            const priceArray = [];

            for (let i = 0; i < items.length; i++) {
                // console.log(items[i]);
                const sellingState = items[i].sellingStatus[0].sellingState;
                const sellingPrice = items[i].sellingStatus[0].currentPrice[0].__value__;
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

            let sum = 0;

            for (let j = 0; j < priceArray.length; j++) {
                sum += parseInt(priceArray[j]);
            }

            const average = sum / priceArray.length;
            // console.log(sum);
            // console.log("game name: " + gameName + "game price: " + average);

            const averagePrice = average.toFixed(2);

            // value.push(averagePrice);

        });
    }

    function gameValue2() {
        const value = [];

        $.ajax({
            url    : "/api/games",
            method : "GET",
            success: function (data) {

                for (let i = 0; i < data.length; i++) {
                    const gameName = data[i].Name;
                    const platformName = data[i].Platform;
                    // console.log(gameName);
                    // console.log(platformName);

                    const settings = {
                        "async"      : false,
                        "crossDomain": true,
                        "url"        : "https://cors-anywhere.herokuapp.com/https://svcs.ebay.com/services/search/FindingService/v1?RESPONSE-DATA-FORMAT=JSON&keywords=" + gameName + "," + platformName + "&categoryId=139973",
                        "method"     : "GET",
                        "headers"    : {
                            "X-EBAY-SOA-SERVICE-VERSION" : "1.13.0",
                            "X-EBAY-SOA-OPERATION-NAME"  : "findCompletedItems",
                            "X-EBAY-SOA-SECURITY-APPNAME": "FuzzyJon-RetroGam-PRD-92cc9f5ed-70036e56",
                            "Cache-Control"              : "no-cache",
                            "Postman-Token"              : "2533f0d4-46b4-4ae3-90c5-bfa1c936774a"
                        }
                    };

                    $.ajax(settings).done(function (response) {
                        const results = JSON.parse(response);
                        // console.log(results);
                        const items = results.findCompletedItemsResponse[0].searchResult[0].item;
                        const priceArray = [];

                        for (let i = 0; i < items.length; i++) {
                            // console.log(items[i]);
                            const sellingState = items[i].sellingStatus[0].sellingState;
                            const sellingPrice = items[i].sellingStatus[0].currentPrice[0].__value__;
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

                        let sum = 0;

                        for (let j = 0; j < priceArray.length; j++) {
                            sum += parseInt(priceArray[j]);
                        }

                        const average = sum / priceArray.length;
                        // console.log(sum);
                        // console.log("game name: " + gameName + "game price: " + average);

                        const averagePrice = average.toFixed(2);

                        // value.push(averagePrice);

                    });

                }
            }

        });
    }

    // this is the display game TEST function, is just displaying what used to be in the Profile.cshtml file to test dynamic html
    // will be removed and take out the call in initEventHandlers once we have the displayGames() function working with api from DB
    function displayGamesTest() {
        $("#displayGames").append(
            `
<div class="game-grid-container">
<div id="gameStats" class="row">
<div class="col-sm-3 gameName">
<strong>TITLE: </strong>Zelda: Breath of the Wild
</div>
<div class="col-sm-2 gameValue">
<img id="coin" src="https://media0.giphy.com/media/yCyVbqru5Ggfu/giphy.gif">
<strong>EST. VALUE: </strong> $41.59
</div>
<div class="col-sm-2 gameCondition dropdown-toggle" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
<strong id="conditionBtn" role="button">EST. CONDITION: </strong> EXCELLENT
<div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton">
<a class="dropdown-item" href="#">
<img class="lifeHeart" scr="https://pbs.twimg.com/profile_images/554699922138624000/0AopZpk4_400x400.png">
</a>
<a class="dropdown-item" href="#">4 HEARTS</a>
<a class="dropdown-item" href="#">3 HEARTS</a>
<a class="dropdown-item" href="#">2 HEARTS</a>
<a class="dropdown-item" href="#">1 HEART</a>
</div>
</div>
<div class="col-sm-2 gamePlatform">
<strong>PLATFORM: </strong> NES
</div>
<div class="col-sm-2 actions">
<img id="edit" src="https://png.icons8.com/metro/1600/edit-property.png" role="button">
<img id="save" src="https://cdn4.iconfinder.com/data/icons/STROKE/computer_gadgets/png/400/floppy_disk.png" role="button">
<img id="delete" src="https://png.icons8.com/metro/1600/delete.png" role="button">
</div>
</div>
<div class="row">
<div class="col-sm-2 gameImage">
<img class='' src="" alt="" />
</div>
<br>
<div class="col-sm-10">
<p class='gameSummary'>
<strong>SUMMARY:</strong> Lorem ipsum dolor sit amet consectetur adipisicing elit. Iusto earum voluptatem facere culpa
quia expedita, sunt eaque officia est nesciunt mollitia aliquam. Neque velit reiciendis blanditiis, perferendis
ipsam dicta molestiae? Lorem ipsum dolor sit amet consectetur adipisicing elit. Iusto earum voluptatem facere culpa
quia expedita, sunt eaque officia est nesciunt mollitia aliquam. Neque velit reiciendis blanditiis, perferendis
ipsam dicta molestiae? Iusto earum voluptatem facere culpa quia expedita, sunt eaque officia est nesciunt mollitia
aliquam. Neque velit reiciendis blanditiis, perferendis ipsam dicta molestiae?
</p>
</div>
</div>
</div>
            `
        );
    };

    return {
        init: init
    };
}();

$(function() {
    Profile.init();
});