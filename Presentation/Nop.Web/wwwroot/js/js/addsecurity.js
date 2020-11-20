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

