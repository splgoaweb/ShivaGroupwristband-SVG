//Textfield for line 1
function demo() {
    var p = document.getElementById("myP");
    var input = document.getElementById("TextField");
    p.textContent = input.value;
}
//Textfield for line 2
function newdemo() {
    var p = document.getElementById("myPnew");
    var input = document.getElementById("TextField2");
    p.textContent = input.value;
}

//Adding New Line option
function addline() {
    document.getElementById("newline").style.display = "block";
    document.getElementById("myPnew").style.display = "block";
    document.getElementById("add").style.display = "none";
}
//Removing New Line option
function removeline() {
    document.getElementById("newline").style.display = "none";
    document.getElementById("myPnew").style.display = "none";
    document.getElementById("add").style.display = "block";
}

//Fontstyle for Line 1 SVG
function fontstyle(selectTag) {
    var listValue = selectTag.options[selectTag.selectedIndex].text;
    document.getElementById("myP").style.fontFamily = listValue;
}
//Fontstyle for Line 2 SVG
function fontstyle2(selectTag) {
    var listValue = selectTag.options[selectTag.selectedIndex].text;
    document.getElementById("myPnew").style.fontFamily = listValue;
}

//INCREASE-DECREASE for line 1
function Increase() {
    //Get the tag you want to change
    var headerTag = document.getElementById("myP");

    // Get the current font size
    var currentFontSize = headerTag.style.fontSize;

    //Take out the px in the end
    currentFontSize = currentFontSize.slice(0, -2);

    //Make currenFontSize an Integer
    currentFontSize = parseInt(currentFontSize);

    //Make new font size while increase currenFontSize by 1
    var newFontSize = currentFontSize + 1;

    //Make newFontsize a string and add px in the end
    newFontSize = newFontSize.toString() + "px";

    //Set new style
    headerTag.style.fontSize = newFontSize;
}

function Decrease() {
    //Get the tag you want to change
    var headerTag = document.getElementById("myP");

    // Get the current font size
    var currentFontSize = headerTag.style.fontSize;

    //Take out the px in the end
    currentFontSize = currentFontSize.slice(0, -2);

    //Make currenFontSize an Integer
    currentFontSize = parseInt(currentFontSize);

    //Make new font size while decrease currenFontSize by 1
    var newFontSize = currentFontSize - 1;

    //Make newFontsize a string and add px in the end
    newFontSize = newFontSize.toString() + "px";

    //Set new style
    headerTag.style.fontSize = newFontSize;
}

//INCREASE-DECREASE for line 2 SVG
function Increase2() {
    //Get the tag you want to change
    var headerTag = document.getElementById("myPnew");

    // Get the current font size
    var currentFontSize = headerTag.style.fontSize;

    //Take out the px in the end
    currentFontSize = currentFontSize.slice(0, -2);

    //Make currenFontSize an Integer
    currentFontSize = parseInt(currentFontSize);

    //Make new font size while increase currenFontSize by 1
    var newFontSize = currentFontSize + 1;

    //Make newFontsize a string and add px in the end
    newFontSize = newFontSize.toString() + "px";

    //Set new style
    headerTag.style.fontSize = newFontSize;
}

function Decrease2() {
    //Get the tag you want to change
    var headerTag = document.getElementById("myPnew");

    // Get the current font size
    var currentFontSize = headerTag.style.fontSize;

    //Take out the px in the end
    currentFontSize = currentFontSize.slice(0, -2);

    //Make currenFontSize an Integer
    currentFontSize = parseInt(currentFontSize);

    //Make new font size while decrease currenFontSize by 1
    var newFontSize = currentFontSize - 1;

    //Make newFontsize a string and add px in the end
    newFontSize = newFontSize.toString() + "px";

    //Set new style
    headerTag.style.fontSize = newFontSize;
}

//Text functions for Text 1 SVG
function bold() {
    document.getElementById("myP").style.fontWeight =
        document.getElementById("myP").style.fontWeight === "bold" ?
        "unset" :
        "bold";
}

function italic() {
    document.getElementById("myP").style.fontStyle =
        document.getElementById("myP").style.fontStyle === "italic" ?
        "unset" :
        "italic";
}

function underline() {
    document.getElementById("myP").style.textDecoration =
        document.getElementById("myP").style.textDecoration === "underline" ?
        "unset" :
        "underline";
}

function uppercase() {
    document.getElementById("myP").style.textTransform =
        document.getElementById("myP").style.textTransform === "uppercase" ?
        "unset" :
        "uppercase";
}

function lowercase() {
    document.getElementById("myP").style.textTransform =
        document.getElementById("myP").style.textTransform === "lowercase" ?
        "unset" :
        "lowercase";
}

//Text functions for Text 2 SVG
function bold2() {
    document.getElementById("myPnew").style.fontWeight =
        document.getElementById("myPnew").style.fontWeight === "bold" ?
        "unset" :
        "bold";
}

function italic2() {
    document.getElementById("myPnew").style.fontStyle =
        document.getElementById("myPnew").style.fontStyle === "italic" ?
        "unset" :
        "italic";
}

function underline2() {
    document.getElementById("myPnew").style.textDecoration =
        document.getElementById("myPnew").style.textDecoration === "underline" ?
        "unset" :
        "underline";
}

function uppercase2() {
    document.getElementById("myPnew").style.textTransform =
        document.getElementById("myPnew").style.textTransform === "uppercase" ?
        "unset" :
        "uppercase";
}

function lowercase2() {
    document.getElementById("myPnew").style.textTransform =
        document.getElementById("myPnew").style.textTransform === "lowercase" ?
        "unset" :
        "lowercase";
}

//Color Picker to change color of SVG Text 1
function textcolor() {
   
    var x = document.getElementById("tc").value; 
    document.getElementById("myP").style.fill = x;
}
//Color Picker to change color of SVG Text 2
function textcolor2() {
    console.log('-In textcolor2-');
    var x = document.getElementById("tc2").value; console.log('x -> ', x);
    document.getElementById("myPnew").style.fill = x;
}




//Draggable Text For SVG
Draggable.create(".logo", {
    bounds: "#nox",
});
Draggable.create(".code_image", {
    bounds: "#noxBarcode",
});

Draggable.create(".myP", {
    //(Text Line 1)
    bounds: "#nox",
});



Draggable.create(".myPnew", {
    //(Text Line 2)
    bounds: "#nox",
});





console.clear();
var svg = document.getElementById("band");
var p = document.getElementById("myP");
var pnew = document.getElementById("myPnew");
var translate = { x: 0, y: 0 };

p.onmousedown = (e) => {
    const beforeMove = { x: e.clientX, y: e.clientY };
};
pnew.onmousedown = (e) => {
    const beforeMove = { x: e.clientX, y: e.clientY };
};

function moveAt(dx, dy) {
    p.style.transform = `translate(${dx}px, ${dy}px)`;
}

svg.onmousemove = (e) => {
    try {
        translate.x += e.clientX - beforeMove.x;
        translate.y += e.clientY - beforeMove.y;
        beforeMove.x = e.clientX;
        beforeMove.y = e.clientY;
        moveAt(translate.x, translate.y);
    } catch (error) { console.log('Error on > svg.onmousemove : ', error); }
};
document.onmouseup = (e) => {
    svg.onmousemove = null;
};