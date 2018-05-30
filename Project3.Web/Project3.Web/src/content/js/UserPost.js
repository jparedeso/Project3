

  $(document).ready(function () {
    $("addPost").click(function () {
      $("commentModal").modal();
    });
  });
  
  var post = function() {
    function init() {
        initEventHandlers();
    }

    function initEventHandlers() {
        submitPost();
    }
    
    function submitPost() {
        $("#submitPost").on("click", function(event) {
            event.preventDefault();

            var newPost = {
                Content: $("#newPost").val().trim()
            };
            
            console.log(newPost);
            
            $("#displayPosts").append("<div class='row'>" + "<div class='col-sm-2 userInfoCard commentComponent'>" + "<div class='card'>" + "<img class='card-img-top' src='http://images6.fanpop.com/image/photos/37500000/Paper-Mario-the-super-mario-fan-club-37561055-500-500.jpg' alt='Card image'>" + "<div class='card-body'>" + "<h5 class='card-title'>" + "MarioFan4ever" + "</h5>" + "<p class='card-text'>" + "Member of the kingdom since: 2018" + "</p></div></div></div>" + "<div class='col-sm-9 userComment'>" + "<p class='comment'>" + newPost.Content + "</div>");

        });
    }

    return {
        init: init
    }
  }();

  $(function() {
      post.init();
  });
