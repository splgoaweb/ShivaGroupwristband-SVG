//Change background color of SVG
function changeColor(id) {
    document.getElementById("bg_image").setAttribute("xlink:href", "");
    var btn = document.querySelectorAll(".Cbtn");
    var nox = document.getElementById("nox");

    for (let i = 0; i < btn.length; i++) {
        btn[i].addEventListener("click", function() {
            nox.style.fill = this.getAttribute("id");
            //this.style.id=this.getAttribute("id");
        })
    }
}


//change the background pattern of SVG
function P(pattern) {
    var base_url = window.location.origin;
    console.log('base_url > ', base_url);
    //var p = base_url+pattern;
    //console.log('p > ', p);
    document.getElementById("nox").setAttribute("style", "fill: none");
    var xhr = new XMLHttpRequest();
    xhr.open("GET", pattern, true);
    xhr.responseType = "blob";
    xhr.onload = function(e) {
        console.log('---> ',this.response);
        var reader = new FileReader();
        reader.onload = function(event) {
            var res = event.target.result;
            console.log(res);
            document.getElementById("bg_image").setAttribute("xlink:href", res);
        }
        var file = this.response;
        reader.readAsDataURL(file)

    };
    xhr.send()

}

//change the background gradient of SVG
function G(design) {
    document.getElementById("nox").setAttribute("style", "fill: none");
    var xhr = new XMLHttpRequest();
    xhr.open("GET", design, true);
    xhr.responseType = "blob";
    xhr.onload = function(e) {
        console.log(this.response);
        var reader = new FileReader();
        reader.onload = function(event) {
            var res = event.target.result;
            console.log(res);
            document.getElementById("bg_image").setAttribute("xlink:href", res);
        }
        var file = this.response;
        console.log('file > ', file)
        reader.readAsDataURL(file)

    };
    xhr.send()

}


//Color Picker for SVG
function bgcolor() {
    var x = document.getElementById("myColor").value;
    document.getElementById("nox").style.fill = x;
}