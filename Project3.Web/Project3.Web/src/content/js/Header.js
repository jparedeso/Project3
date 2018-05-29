
var Header = function() {
  function init() {
      initEventHandlers();
  }

  function initEventHandlers() {
      forumDropdown();
      userDropdown();
  }

  function forumDropdown() {
    $("#forumsBtn").on("click", function(event) {
      event.preventDefault();
      document.getElementById("forumsDrpDwn").classList.toggle("show");
    });
  }

  function userDropdown() {
    $("#userBtn").on("click", function(event) {
      event.preventDefault();
      document.getElementById("userDrpDwn").classList.toggle("show");
    });
  }

  window.onclick = function (event) {
    if (!event.target.matches('.dropbtn')) {

      var dropdowns = document.getElementsByClassName("dropdown-content");
      var i;
      for (i = 0; i < dropdowns.length; i++) {
        var openDropdown = dropdowns[i];
        if (openDropdown.classList.contains('show')) {
          openDropdown.classList.remove('show');
        }
      }
    }
  }

  return {
    init: init
  }
}();

$(function() {
  Header.init();
});



// function showUserMenu() {
//   document.getElementById("userDrpDwn").classList.toggle("show");
// }

// function showForums() {
//   document.getElementById("forumsDrpDwn").classList.toggle("show");
// }