
$(document).ready(function(){    
    
    $("#submitGeneralThread").on("click", function(event) {
        event.preventDefault();
        var newThread = {
            Name: $("#modalThreadTitle").val().trim()
        };

        var newPost = {
            Content: $("#modalPost").val().trim()
        };
        console.log(newThread, newPost);

        // Send the POST request to create new thread.
        $.ajax("/api/thread/general", {
            type: "POST",
            data: newThread
        }).then(
            function() {
                console.log("added new thread");
                $.ajax("/api/thread/general/post", {
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

    if ($("#displayGeneralThreads")[0]) {
        // get data for general threads
        $.ajax({
            url: "/api/thread/general",
            method: "GET",
            success: function(data) {
                console.log(data);

                // empty to displayGeneralThreads before adding new content
                $("#displayGeneralThreads").empty();
                // if the data is not there, then return an error message
                if (!data) {
                    $("#displayGeneralThreads").append("<h2> I'm sorry, but there aren't any general threads yet</h2>");
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

