var Forum = function() {
    function init() {
        initEventHandlers();
    }

    function initEventHandlers() {
        submitThread();
    }
    
    function submitThread() {
        $("#submitThread").on("click", function(event) {
            event.preventDefault();
            var newThread = {
                Name: $("#modalThreadTitle").val().trim()
            };

            var newPost = {
                Content: $("#modalPost").val().trim()
            };
            console.log(newThread, newPost);

            // Send the POST request to create new thread.

            // $.ajax("/api/thread/", {
            //     type: "POST",
            //     data: newThread
            // }).then(
            //     function() {
            //         console.log("added new thread");
            //         $.ajax("/api/thread//post", {
            //             type: "POST",
            //             data: newPost
            //         }).then(
            //             function() {
            //                 console.log("added new post");
            //                 // Reload the page
            //                 location.reload();
            //             }
            //         );
            //     }
            // );
        });
    }

    function displayThreads() {
        if ($("#displayThreads")[0]) {
            // get data for  threads
            $.ajax({
                url: "/api/thread/",
                method: "GET",
                success: function(data) {
                    console.log(data);

                    // empty to displayThreads before adding new content
                    $("#displayThreads").empty();
                    // if the data is not there, then return an error message
                    if (!data) {
                        $("#displayThreads").append("<h2> I'm sorry, but there aren't any  threads yet</h2>");
                    }
                    else {
                        for (var i = 0; i < data.length; i++) {
                            $("#displayThreads").append("<tr>" + 
                            "<td><a class='artTitle' href='#'>" + data[i].Name + "</a></td>" + 
                            "<td class='align'>" + data[i].userName + "</td>" +
                            "<td class='align'>" + data[i].Created + "</td>" +
                            "<td class='align'>" + data[i].LastActivity + "</td>" +
                            "</tr>");
                        }
                    }
                },
                error: function(xhr, status, error) {
                    console.log(xhr);
                    console.log(status);
                    console.log(error);
                }
            });
        }
    }

    return {
        init: init
    }
}();

$(function() {
    Forum.init();
});

