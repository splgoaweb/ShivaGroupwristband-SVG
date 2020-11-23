
/*
// Side Numbering Text
function numbering() {
    var p = document.getElementById('Pnumb');
    var input = document.getElementById('Serialn');
    p.textContent = input.value;
  } 

// barcode Numbering Text
function barcodenumber() {
  var b = document.getElementById('');
  var input = document.getElementById('barcoden');
  b.textContent = input.value;
}

// QR Code Text
function qr() {
  var q = document.getElementById('');
  var input = document.getElementById('qr_text');
  q.textContent = input.value;
}

 function qrcode() {
  document.getElementById("qr_text").style.display="block";
  document.getElementById("barcoden").style.display="none";
}
*/


// Side Numbering Text
function numbering() {
    var p = document.getElementById('Pnumb');
    var input = document.getElementById('Serialn');
    p.textContent = input.value;
}

// barcode Numbering Text
function barcodenumber() {
    var b = document.getElementById('');
    var input = document.getElementById('barcoden');
    b.textContent = input.value;
}

// QR Code Text
function qr() {
    var q = document.getElementById('');
    var input = document.getElementById('qr_text');
    q.textContent = input.value;
}

function qrcode() {
    document.getElementById("qr_text").style.display = "block";
    document.getElementById("barcoden").style.display = "none";
}


//function to change the 2D Codes on button click
function code(code) {
    var xhr = new XMLHttpRequest();
    xhr.open("GET", code, true);
    xhr.responseType = "blob";
    xhr.onload = function (e) {
        console.log(this.response);
        var reader = new FileReader();
        reader.onload = function (event) {
            var res = event.target.result;
            console.log(res);
            document.getElementById("code_image").setAttribute("xlink:href", res);
        }
        var file = this.response;
        console.log('file > ', file)
        reader.readAsDataURL(file)

    };
    xhr.send()

}