
$(document).ready(function(){    
    
    $("#submitReviewThread").on("click", function(event) {
        event.preventDefault();
        var newThread = {
            Name: $("#modalThreadTitle").val().trim()
        };

        var newPost = {
            Content: $("#modalPost").val().trim()
        };
        console.log(newThread, newPost);

        // Send the POST request to create new thread.
        $.ajax("/api/thread/review", {
            type: "POST",
            data: newThread
        }).then(
            function() {
                console.log("added new thread");
                $.ajax("/api/thread/review/post", {
                    type: "POST",
                    data: newPost
                }).then(
                    function() {
                        console.log("added new post");
                        // Reload the page
                        location.reload();
                    }
                );
            }
        );
    });


    if ($("#displayReviewThreads")[0]) {
        // get data for review threads
        $.ajax({
            url: "/api/thread/review",
            method: "GET",
            success: function(data) {
                console.log(data);

                // empty to displayReviewThreads before adding new content
                $("#displayReviewThreads").empty();
                // if the data is not there, then return an error message
                if (!data) {
                    $("#displayReviewThreads").append("<h2> I'm sorry, but there aren't any review threads yet</h2>");
                }
                else {
                    for (var i = 0; i < data.length; i++) {
                        $("#displayReviewThreads").append("<tr>" + 
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

    
});
