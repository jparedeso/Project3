const Profile = function () {

    function init() {
        initEventHandlers();
        getUserGames();
    }

    function initEventHandlers() {
        searchGames();
        checkValue();
        valueModalClose();
    }

    function searchGames() {

        $("#searchGameForm").on("submit", function (e) {
            e.preventDefault();

            const name = $(".gameTitle").val().trim();

            $.ajax({
                url    : this.action,
                method : this.method,
                data   : {name},
                success: response => {
                    // console.log(response);
                    $("#gameReturn").html("");
                    for (let i = 0; i < response.length; i++) {
                        // array for platforms to push into for each games
                        let platformArray = [];
                        // console.log(platformArray);

                        let platforms = response[i].platforms;
                        // console.log(platforms);

                        for (let i = 0; i < platforms.length; i++) {
                            platformArray.push(platforms[i]);
                            // console.log(platforms[j].platformName);
                            // $(".platformReturn").append(`
                            // <div class="platformSelection" style="padding-left: 25px; color: black; display: none;">${platforms[j].platformName}</div>
                            // `)
                        }

                        // creating a li to insert into the ul below
                        let list;
                        for (let i = 0; i < platformArray.length; i++) {
                            list += "<li class='platformListItem' platform-name='" + platformArray[i].platformName  + "'>" + platformArray[i].platformName + "</li>";
                        }
                        
                        // append a list of platforms to append into the gamesReturn div
                        $("#gameReturn").append(`
                        <div class="gameSelection" data-id="${response[i].gameId}">${response[i].name}</div>
                        <div class="platformReturn" style="padding-left: 25px; color: black;">
                            <ul class="platformList" style="display: none;">
                                ${list}
                            </ul>
                        </div>
                        `);
                    }

                    // highlight game selection
                    let gameSelection = $(".gameSelection").on("click", function () {
                        const gameHighlight = "gameHighlight";
                        const id = $(this).attr("data-id");
                        gameSelection.removeClass(gameHighlight);
                        $(this).addClass(gameHighlight);
                        // console.log(id);

                        // show the platforms list that is hidden
                        // $(".platformList").show();
                    });

                    // highlight platform selection

                    // let platformListItem = $(".platformListItem").on("click", function () {
                    //     const platformHighlight = "platformHighlight";
                    //     const platformName = $(this).attr("platform-name");
                    //     platformListItem.removeClass(platformHighlight);
                    //     $(this).addClass(platformHighlight);
                    //     console.log(platformName);
                    // });
                }
            });
        });

        $("#addGameBtn").on("click", function () {
            let id = $(".gameHighlight").attr("data-id");
                // console.log("game highlighted: " + id);
            // let platformChosen = $(".platformHighlight").attr("platform-name");
            //     console.log("platform highlighted: " + platformChosen);
                $.ajax({
                    url: "/Games/InsertGame",
                    method: "POST",
                    data: {id},
                    success: response => {
                        // console.log(response);
                        $('#gameSearchModal').modal('hide');
                        getUserGames();
                        // location.reload();
                    }
                });
        });
    }

    function getUserGames() {
        $.ajax({
            url: "/Games/GameInfo",
            method: "GET",
            success: response => {
                // console.log(response);
                $("#displayGames").html("");
                for (var i = 0; i < response.length; i++) {

                    // console.log(response[i].platforms);

                    // everything commented below makes the ebay api work

                    // let gameName = response[i].name;
                    // // console.log(gameName);

                    // let value= [];
                    // // console.log(value);

                    // const settings = {
                    //     "async"      : true,
                    //     "crossDomain": true,
                    //     "url"        : "https://cors-anywhere.herokuapp.com/https://svcs.ebay.com/services/search/FindingService/v1?RESPONSE-DATA-FORMAT=JSON&keywords=" + gameName + "&categoryId=139973",
                    //     "method"     : "GET",
                    //     "headers"    : {
                    //         "X-EBAY-SOA-SERVICE-VERSION" : "1.13.0",
                    //         "X-EBAY-SOA-OPERATION-NAME"  : "findCompletedItems",
                    //         "X-EBAY-SOA-SECURITY-APPNAME": "FuzzyJon-RetroGam-PRD-92cc9f5ed-70036e56",
                    //         "Cache-Control"              : "no-cache",
                    //         "Postman-Token"              : "2533f0d4-46b4-4ae3-90c5-bfa1c936774a"
                    //     }
                    // };

                    // $.ajax(settings).done(function (response) {
                    //     const results = JSON.parse(response);
                    //     // console.log(results);
                    //     const items = results.findCompletedItemsResponse[0].searchResult[0].item;
                    //     const priceArray = [];
            
                    //     for (let i = 0; i < items.length; i++) {
                    //         // console.log(items[i]);
                    //         const sellingState = items[i].sellingStatus[0].sellingState;
                    //         const sellingPrice = items[i].sellingStatus[0].currentPrice[0].__value__;
                    //         // console.log(sellingState);
                    //         // var sum = 0;
                    //         if (sellingState == "EndedWithSales") {
                    //             // console.log("true");
                    //             // console.log(sellingPrice);
                    //             priceArray.push(sellingPrice);
                    //             // console.log(priceArray);
                    //         }
                    //     }
                    //     // console.log(priceArray);
            
                    //     let sum = 0;
            
                    //     for (let j = 0; j < priceArray.length; j++) {
                    //         sum += parseInt(priceArray[j]);
                    //     }
            
                    //     const average = sum / priceArray.length;
                    //     // console.log(sum);
                    //     // console.log("game name: " + gameName + "game price: " + average);
            
                    //     const averagePrice = average.toFixed(2);
                    //     console.log("game: " + gameName + " / price: $" + averagePrice);

                    //     value.push(averagePrice);
                    //     console.log(value); 
                        
                    // });

                    $("#displayGames").append(`
                        <div class="game-grid-container" data-id="${response[i].gameId}">
                            <div id="gameStats" class="row">
                                <div class="col-sm-3 gameName">
                                    <strong>TITLE: </strong>${response[i].name}
                                </div>
                                <div class="col-sm-2 gameValue">
                                    <img id="coin" src="https://media0.giphy.com/media/yCyVbqru5Ggfu/giphy.gif">
                                    <strong>EST. VALUE: </strong>
                                </div>
                                <div class="col-sm-2 gameCondition dropdown-toggle" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <strong id="conditionBtn" role="button">EST. CONDITION: </strong>
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
                                    <strong>PLATFORM: </strong>${response[i].platforms[0].platformName}
                                </div>
                                <div class="col-sm-2 actions">
                                    <img id="edit" src="https://png.icons8.com/metro/1600/edit-property.png" role="button">
                                    <img id="save" src="https://cdn4.iconfinder.com/data/icons/STROKE/computer_gadgets/png/400/floppy_disk.png" role="button">
                                    <img id="delete" src="https://png.icons8.com/metro/1600/delete.png" role="button">
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 gameImage">
                                    <img class="" src="https:${response[i].cover}" alt="" />
                                </div>
                                <br>
                                <div class="col-sm-10">
                                    <p class='gameSummary'>
                                    <strong>SUMMARY:</strong>${response[i].summary}
                                    </p>
                                </div>
                            </div>
                        </div>
                    `);
                }
            }
        });
    }

    function checkValue() {
        $("#searchValueForm").on("submit", function (e) {
            e.preventDefault();

            const title = $(".valueGameTitle").val().trim();
            const platform = $(".valuePlatform").val().trim();
            // console.log(title);
            // console.log(platform);

            const settings = {
                "async"      : true,
                "crossDomain": true,
                "url"        : "https://cors-anywhere.herokuapp.com/https://svcs.ebay.com/services/search/FindingService/v1?RESPONSE-DATA-FORMAT=JSON&keywords=" + title + "," + platform + "&categoryId=139973",
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
        
                const averageValue = average.toFixed(2);
                // console.log("game: " + title + " / Value: $" + averageValue);

                $('#valueReturn').append(`
                <h5 id="estimatedValueText">ESTIMATED VALUE:</h5>
                <h5 id="estimatedValue">$${averageValue}</h5>
                `)
        
                });
        });
    }

    function valueModalClose() {
        // reloads page after closing modal to clear values
        $('#checkValueModal').on('hidden.bs.modal', function () {
            location.reload();
           })
    }

    function gameValue() {
        let gameName = response[i].name;
        // console.log(gameName);

        let value= [];
        // console.log(value);

        const settings = {
        "async"      : true,
        "crossDomain": true,
        "url"        : "https://cors-anywhere.herokuapp.com/https://svcs.ebay.com/services/search/FindingService/v1?RESPONSE-DATA-FORMAT=JSON&keywords=" + gameName + "&categoryId=139973",
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
        console.log("game: " + gameName + " / price: $" + averagePrice);

        value.push(averagePrice);
        console.log(value); 

        });
    }

    return {
        init: init
    };
}();

$(function() {
    Profile.init();
});