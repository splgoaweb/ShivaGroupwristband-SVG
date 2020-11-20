//REDO BUTTON

//UNDO BUTTON

//ZOOM BUTTON

//REFRESH BUTTON
const refreshButton = document.querySelector('.ti-reload');
const refreshPage = () => {
    location.reload();
}

refreshButton.addEventListener('click', refreshPage)

//PREVIEW BUTTON
// Get the modal
var modal = document.getElementById("preview");

// Get the button that opens the modal
var btn = document.getElementById("btn-Preview-Image");

// Get the <span>s element that closes the modal
var span = document.getElementsByClassName("close")[0];
var crt = document.getElementsByClassName("ce")[0];

// When the user clicks the button, open the modal 
// btn.onclick = function() {
//     modal.style.display = "block";
// }

// When the user clicks on <span> (x), close the modal
// span.onclick = function() {
//     modal.style.display = "none";
// }
// crt.onclick = function() {
//     modal.style.display = "none";
// }

// When the user clicks anywhere outside of the modal, close it
window.onclick = function(event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

//SAVE BUTTON