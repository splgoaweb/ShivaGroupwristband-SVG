$(document).ready(function() {
    console.log('----------> Hi samihan  ---> $(document).ready ');



    var logoClicked = false;
    var textClicked = false;
    var validElementSelected = false;
    var clickedID = '';
    var elementTagName = '';
    var keydownCounter = 0;

    $('#canvas-area').hide();


    $('#close-preview').click(function() {
        $('#canvas-area').fadeOut();
    });
    $('.Cbtn').click(function() {
        refreshCanvas();
    });
    $('.swatch').click(function() {
        refreshCanvas();
    });
    $('#myP').click(function() {
        refreshCanvas();
    });
    $('#TextField').change(function() {
        refreshCanvas();
    });
    $('#TextField').keyup(function() {
        refreshCanvas();
    });
    $('#btn-Preview').click(function() {
        refreshCanvas();
        $('#canvas-area').fadeIn();
    });

    $('#logo_image').click(function() {
        logoClicked = true;
    });

    $('text').click(function() {
        textClicked = true;
    });



    function makeSVG(parent, tag, attrs) {
        var el = document.createElementNS('http://www.w3.org/2000/svg', tag);
        for (var k in attrs) {
            /*
            if (k == "xlink:href") {
                el.setAttributeNS('http://www.w3.org/1999/xlink', 'href', attrs[k]);
            } else {
                el.setAttribute(k, attrs[k]);
            }
            */
            el.setAttribute(k, attrs[k]);
        }
        console.log('makeSVG > el > ', el);
        document.getElementById(parent).appendChild(el);
    }

    $('#new_upload_btn').click(function() {
        // var html = document.getElementById('img-upload-area').innerHTML;
        var input = document.getElementById('img-upload-area').getElementsByTagName('input').length;
        console.log('input > ', input);
        var new_input = '<input id="upload" class="logo_image_' + input + '" type="file" /><br/><br/>';
        console.log('new_input > ', new_input);
        $("#img-upload-area").append(new_input); //append new upload btn to div
        var logo_image = document.getElementById('band').getElementsByClassName('logo').length;
        console.log('logo_image > ', logo_image);
        //  var new_svg_logo_image = '<image class="logo" id="logo_image_' + logo_image + '" xlink:href=""  x="82.2" y="-0.1" > </image>';
        //  console.log('new_svg_logo_image > ', new_svg_logo_image);
        //var SVGelements = document.getElementById('band').innerHTML;
        //console.log('SVGelements > ', SVGelements);
        //var newInner = SVGelements + new_svg_logo_image;
        //document.getElementById('band').innerHTML = newInner;
        //$("#band").append(new_svg_logo_image);
        var ID = 'logo_image_' + logo_image;
        makeSVG('band', 'image', {
            class: 'logo',
            id: ID,
            x: '82.2',
            y: '-0.1',
            "xlink:href": ''
        });
        resetElement();
    });

    function resetElement() {
        var svgElement = document.getElementById('band');
        console.log('svgElement > ', svgElement);
        var rawHTML = '@Html.Raw(ViewBag.svgcontent)';
        console.log('rawHTML > ', rawHTML);
    }


    $(document).on("change", "#upload", function(e) {
        var currentBlobData = null;
        console.log('e.target : ', e.target);
        var className = $(e.target).attr('class');
        console.log('className : ', className);
        var patternImage = document.querySelector("#" + className);
        console.log('patternImage > ', patternImage);

        var file = this.files[0];
        console.log('file : ', file);
        console.log('file type : ', file.type);
        if (file.type === 'image/png' || file.type === 'image/jpeg') {



            const reader = new FileReader();
            reader.addEventListener("load", function() {
                console.log(' reader.result : ', reader.result);
                patternImage.setAttribute("xlink:href", reader.result);
            }, false);
            if (file) {
                reader.readAsDataURL(file);
            }
            console.log('reader : ', reader);

            var SVGelements = document.getElementById('band').innerHTML;
            console.log('SVGelements > ', SVGelements);

        } else {
            alert('Please upload PNG & JPG format images only.');
        }




    });

    $('#band').click(function(event) {
        console.log('event > ', event);
        console.log('event.target > ', event.target);
        var svg_element = event.target;
        var tagname = svg_element.tagName;
        elementTagName = tagname;
        console.log('tagname > ', tagname);
        var id = svg_element.id;

        if (tagname === 'text' || tagname === 'image') {
            clickedID = id;
            validElementSelected = true;
            console.log('id > ', id);
            console.log('clickedID > ', clickedID);
            if (tagname === 'text') {
                if (clickedID === 'myP') {
                    console.log('Show edit textbox');
                    $('#myPopup').removeClass("popuptextVisible");
                    $('#myPopup').addClass('popuptextVisibleShow');
                    console.log('Show edit textbox after');
                }
            }
        }

        // console.log('svg_element.getBBox > ', svg_element.getBBox());
        // console.log('svg_element.getCTM > ', svg_element.getCTM());
    });

    //----------------------------LOGO Operations--------------------------------------


    $('#btn-remove').click(function() {
        if (elementTagName === 'text') {
            $('#btn-remove').focus();
            setTimeout(function() {
                var userInput = confirm('Do you want to delete this?');
                console.log('userInput > ', userInput);
                if (userInput) {
                    deleteText();
                }
            }, 50)

        }
        if (elementTagName === 'image') {
            $('#btn-remove').focus();
            setTimeout(function() {
                var userInput = confirm('Do you want to delete this?');
                console.log('userInput > ', userInput);
                if (userInput) {
                    deleteImage();
                }
            }, 50)
        }

    });



    $('html').keydown(function(e) {
        console.log('logoClicked > ', logoClicked);
        console.log('textClicked > ', textClicked);
        try {
            if (logoClicked || textClicked) {
                switch (e.keyCode) {
                    case 37:
                        keydownCounter = 0;
                        movementEventsForKeyDown('image_move_left', ++keydownCounter);
                        break;
                    case 39:
                        keydownCounter = 0;
                        movementEventsForKeyDown('image_move_right', ++keydownCounter);
                        break;
                    case 38:
                        keydownCounter = 0;
                        movementEventsForKeyDown('image_move_up', ++keydownCounter);
                        break;
                    case 40:
                        keydownCounter = 0;
                        movementEventsForKeyDown('image_move_bottom', ++keydownCounter);
                        break;
                    case 107:
                        keydownCounter = 0;
                        //console.log('107. Plus > ', e.keyCode);
                        IncreaseSize(elementTagName, ++keydownCounter);
                        break;
                    case 109:
                        keydownCounter = 0;
                        //console.log('109. Minus > ', e.keyCode);
                        DecreaseSize(elementTagName, ++keydownCounter);
                        break;
                    default:
                        console.log('Default. e > ', e.keyCode);

                }
            }
        } catch (Err) {
            console.log('html keydown > ', Err);
        }
    });




    $('html').keyup(function(e) {
        if (logoClicked || textClicked) {
            switch (e.keyCode) {
                case 46:
                    $('#btn-remove').focus();
                    setTimeout(function() {
                        var userInput = confirm('Do you want to delete this?');
                        console.log('userInput > ', userInput);
                        if (userInput) {
                            if (elementTagName === 'text') {
                                deleteText();
                            }
                            if (elementTagName === 'image') {
                                deleteImage();
                            }

                        }
                    }, 50)
                    break;
                case 37:
                    movementEvents('image_move_left');
                    break;

                case 39:
                    movementEvents('image_move_right');
                    break;
                case 38:
                    movementEvents('image_move_up');
                    break;
                case 40:
                    movementEvents('image_move_bottom');
                    break;
                case 107:
                    //console.log('107. Plus > ', e.keyCode);
                    IncreaseSize(elementTagName, 1);
                    break;
                case 109:
                    //console.log('109. Minus > ', e.keyCode);
                    DecreaseSize(elementTagName, 1);
                    break;

                default:
                    console.log('Default. e > ', e.keyCode);

            }
        }

    });

    function deleteText() {
        var patternImage = document.querySelector("#" + clickedID);
        console.log('deleteText > patternImage > ', patternImage);
        if (patternImage.textContent === '') {} else {
            patternImage.textContent = '';
            if (patternImage.id === 'myP') {
                document.getElementById("TextField").value = '';
                document.getElementById("TextFieldEdit").value = '';
            } else {
                document.getElementById("TextField2").value = '';
                document.getElementById("TextFieldEdit2").value = '';
            }
        }



    }

    function deleteImage(patternImage) {
        var patternImage = document.querySelector("#" + clickedID);
        console.log('deleteImage > patternImage > ', patternImage);
        patternImage.setAttribute("xlink:href", '');
        patternImage.setAttribute("x", '82.2');
        patternImage.setAttribute("y", '-0.1');
        patternImage.setAttribute("transform", 'matrix(1,0,0,1,0,0.10000000149011612)');
        patternImage.setAttribute("style", 'width: 50px; height: 50px; touch-action: none; user-select: none; z-index: 1001; transform: translate(0px, 2px); cursor: move;');
        $('#upload').val('');
        logoClicked = false;
    }

    function movementEventsForKeyDown(id, counter) {
        console.log('movementEventsForKeyDown > counter > ', counter);
        try {
            switch (id) {
                case 'image_move_left':
                    console.log('1. id > ', id);
                    var patternImage = document.querySelector("#" + clickedID);
                    var x = patternImage.getAttribute('x');
                    if (isNaN(x) || x === null) {
                        x = '82.2'; //x="82.2" y="-0.1"
                    }
                    var newX = parseFloat(x) - counter;
                    patternImage.setAttribute("x", newX);
                    break;
                case 'image_move_right':
                    console.log('2. id > ', id);
                    var patternImage = document.querySelector("#" + clickedID);
                    var x = patternImage.getAttribute('x');
                    if (isNaN(x) || x === null) {
                        x = '82.2'; //x="82.2" y="-0.1"
                    }
                    var newX = parseFloat(x) + counter;
                    patternImage.setAttribute("x", newX);
                    break;
                case 'image_move_up':
                    console.log('3. id > ', id);
                    var patternImage = document.querySelector("#" + clickedID);
                    var y = patternImage.getAttribute('y');
                    if (isNaN(y) || y === null) {
                        y = '-0.1';
                    }
                    var newY = parseFloat(y) - counter;
                    patternImage.setAttribute("y", newY);
                    break;
                case 'image_move_bottom':
                    console.log('3. id > ', id);
                    var patternImage = document.querySelector("#" + clickedID);
                    var y = patternImage.getAttribute('y');
                    if (isNaN(y) || y === null) {
                        y = '-0.1';
                    }
                    var newY = parseFloat(y) + counter;
                    patternImage.setAttribute("y", newY);
                    break;
                default:
                    console.log('Default. id > ', id);

            }
        } catch (err) {
            console.log('movementEventsForKeyDown > ', err);
        }
    }

    function movementEvents(id) {
        switch (id) {
            case 'image_move_left':
                console.log('1. id > ', id);
                var patternImage = document.querySelector("#" + clickedID);
                var x = patternImage.getAttribute('x');
                if (isNaN(x) || x === null) {
                    x = '82.2'; //x="82.2" y="-0.1"
                }
                var newX = parseFloat(x) - 1;
                patternImage.setAttribute("x", newX);
                break;
            case 'image_move_right':
                console.log('2. id > ', id);
                var patternImage = document.querySelector("#" + clickedID);
                var x = patternImage.getAttribute('x');
                if (isNaN(x) || x === null) {
                    x = '82.2'; //x="82.2" y="-0.1"
                }
                var newX = parseFloat(x) + 1;
                patternImage.setAttribute("x", newX);
                break;
            case 'image_move_up':
                console.log('3. id > ', id);
                var patternImage = document.querySelector("#" + clickedID);
                var y = patternImage.getAttribute('y');
                if (isNaN(y) || y === null) {
                    y = '-0.1';
                }
                var newY = parseFloat(y) - 1;
                patternImage.setAttribute("y", newY);
                break;
            case 'image_move_bottom':
                console.log('3. id > ', id);
                var patternImage = document.querySelector("#" + clickedID);
                var y = patternImage.getAttribute('y');
                if (isNaN(y) || y === null) {
                    y = '-0.1';
                }
                var newY = parseFloat(y) + 1;
                patternImage.setAttribute("y", newY);
                break;
            default:
                console.log('Default. id > ', id);

        }
    }


    $('.movement-btn').click(function() {
        var id = $(this).attr('id');
        if (validElementSelected) {
            switch (id) {
                case 'image_move_left':
                    console.log('1. id > ', id);
                    var patternImage = document.querySelector("#" + clickedID);
                    var x = patternImage.getAttribute('x');
                    if (isNaN(x) || x === null) {
                        x = '82.2'; //x="82.2" y="-0.1"
                    }
                    var newX = parseFloat(x) - 1;
                    patternImage.setAttribute("x", newX);
                    break;
                case 'image_move_right':
                    console.log('2. id > ', id);
                    var patternImage = document.querySelector("#" + clickedID);
                    var x = patternImage.getAttribute('x');
                    if (isNaN(x) || x === null) {
                        x = '82.2'; //x="82.2" y="-0.1"
                    }
                    var newX = parseFloat(x) + 1;
                    patternImage.setAttribute("x", newX);
                    break;
                case 'image_move_up':
                    console.log('3. id > ', id);
                    var patternImage = document.querySelector("#" + clickedID);
                    var y = patternImage.getAttribute('y');
                    if (isNaN(y) || y === null) {
                        y = '-0.1';
                    }
                    var newY = parseFloat(y) - 1;
                    patternImage.setAttribute("y", newY);
                    break;
                case 'image_move_bottom':
                    console.log('3. id > ', id);
                    var patternImage = document.querySelector("#" + clickedID);
                    var y = patternImage.getAttribute('y');
                    if (isNaN(y) || y === null) {
                        y = '-0.1';
                    }
                    var newY = parseFloat(y) + 1;
                    patternImage.setAttribute("y", newY);
                    break;
                default:
                    console.log('Default. id > ', id);

            }
        }

    });


    //------------------------------LOGO Operations ENDS-------------------------------


    //----------------------------TEXT Operations--------------------------------------
    $('#btn-remove-text1').mouseover(function() {
        mouse_on_text1_remove_btn = true;
        $('#btn-remove-text1').show();

    });
    $('.myP').mouseover(function() {
        var t = document.getElementById("myP").textContent;
        // console.log('t > ',t);
        $('#btn-remove-text1').fadeIn();

    });
    $('.myP').click(function() {
        mouse_on_text1_remove_btn = true;
        $('#btn-remove-text1').fadeIn();

    });
    $('.myP').mouseout(function() {
        if (mouse_on_text1_remove_btn) {} else {
            $('#btn-remove-text1').fadeOut();
        }
    });

    $('#btn-remove-text1').click(function() {
        document.getElementById("myP").textContent = null;
        document.getElementById("TextField").value = null;
        document.getElementById("TextFieldEdit").value = null;
        $('#btn-remove-text1').hide();
    });

    $('#TextFieldEdit').keyup(function() {
        document.getElementById("myP").textContent = $(this).val();
        document.getElementById("TextField").value = $(this).val();
    });

    $('#TextFieldEdit').blur(function() {
        $('#myPopup').addClass("popuptextVisible");
    });



    //------------------------------TEXT Operations ENDS-------------------------------





    function refreshCanvas() {
        var canvas = document.getElementById('canvas');
        var ctx = canvas.getContext('2d');
        var data = (new XMLSerializer()).serializeToString(svg);
        var DOMURL = window.URL || window.webkitURL || window;
        var img = new Image();
        var svgBlob = new Blob([data], {
            type: 'image/svg+xml;charset=utf-8'
        });
        var url = DOMURL.createObjectURL(svgBlob);
        img.onload = function() {
            ctx.drawImage(img, 0, 0);
            DOMURL.revokeObjectURL(url);
            var imgURI = canvas
                .toDataURL('image/png')
                .replace('image/png', 'image/octet-stream');
        };
        img.src = url;
    }




    $('#tc').click(function() {
        var x = document.getElementById("tc").value;
        //  console.log('Hi -> textcolor : x = ', x);
        document.getElementById("myP").style.color = x;
        // console.log(' document.getElementById -> ', document.getElementById("myP"));
    });


    //DOWNLOAD AS PNG
    var btn = document.getElementById('save_png');
    var svg = document.getElementById('band');
    var canvas = document.querySelector('canvas');

    function triggerDownload(imgURI) {
        var evt = new MouseEvent('click', {
            view: window,
            bubbles: false,
            cancelable: true
        });
        var a = document.createElement('a');
        a.setAttribute('download', 'wristband.png');
        a.setAttribute('href', imgURI);
        a.setAttribute('target', '_blank');
        a.dispatchEvent(evt);
    }

    btn.addEventListener('click', function() {
        var canvas = document.getElementById('canvas');
        var ctx = canvas.getContext('2d');
        var data = (new XMLSerializer()).serializeToString(svg);
        var DOMURL = window.URL || window.webkitURL || window;
        var img = new Image();
        var svgBlob = new Blob([data], {
            type: 'image/svg+xml;charset=utf-8'
        });
        var url = DOMURL.createObjectURL(svgBlob);
        console.log('url : ', url);
        try {
            img.onload = function() {
                console.log('img.onload : ', img);
                ctx.drawImage(img, 0, 0);
                DOMURL.revokeObjectURL(url);

                var imgURI = canvas
                    .toDataURL('image/png')
                    .replace('image/png', 'image/octet-stream');
                console.log('imgURI : ', imgURI);

                triggerDownload(imgURI);
            };
        } catch (error) {
            console.log('error > ', error);
        }
        img.src = url;
    });




    function IncreaseSize(elementTagName, val) {
        console.log('IncreaseSize > ', elementTagName);
        switch (elementTagName) {
            case 'text':
                var headerTag = document.getElementById(clickedID);
                var currentFontSize = headerTag.style.fontSize;
                currentFontSize = currentFontSize.slice(0, -2);
                currentFontSize = parseInt(currentFontSize);
                var newFontSize = currentFontSize + val;
                newFontSize = newFontSize.toString() + "px";
                headerTag.style.fontSize = newFontSize;
                break;
            case 'image':
                var myImg = document.querySelector(".logo#" + clickedID); //document.getElementById("logo_image");
                console.log('myImg > ', myImg);
                console.log('myImg.style > ', myImg.style);

                var currWidth = myImg.style.width;
                var currHeight = myImg.style.height;
                var w = currWidth.includes("px");
                var h = currHeight.includes("px");
                console.log('w > ', w);
                console.log('h > ', h);
                if (w && h) {
                    console.log('currWidth > ', currWidth);
                    console.log('currHeight > ', currHeight);
                    currWidth = currWidth.replace("px", "");
                    currHeight = currHeight.replace("px", "");

                    var newWidth = (parseInt(currWidth) + val);
                    var newHeight = (parseInt(currHeight) + val);
                    newWidth = newWidth.toString() + "px";
                    newHeight = newHeight.toString() + "px";
                    console.log('newWidth > ', newWidth);
                    console.log('newHeight > ', newHeight);
                    myImg.style.width = newWidth;
                    myImg.style.height = newHeight;
                    console.log('myImg.style.width > ', myImg.style.width);
                    console.log('myImg.style.height > ', myImg.style.height);
                }
                break;

            default:
                console.log('Default. id > ');

        }
    }

    function DecreaseSize(elementTagName, val) {
        switch (elementTagName) {
            case 'text':
                var headerTag = document.getElementById(clickedID);
                var currentFontSize = headerTag.style.fontSize;
                currentFontSize = currentFontSize.slice(0, -2);
                currentFontSize = parseInt(currentFontSize);
                var newFontSize = currentFontSize - val;
                newFontSize = newFontSize.toString() + "px";
                headerTag.style.fontSize = newFontSize;
                break;
            case 'image':
                var myImg = document.querySelector(".logo#" + clickedID);
                console.log('myImg > ', myImg);
                console.log('myImg.style > ', myImg.style);

                var currWidth = myImg.style.width;
                var currHeight = myImg.style.height;
                var w = currWidth.includes("px");
                var h = currHeight.includes("px");
                console.log('w > ', w);
                console.log('h > ', h);
                if (w && h) {
                    console.log('currWidth > ', currWidth);
                    console.log('currHeight > ', currHeight);
                    currWidth = currWidth.replace("px", "");
                    currHeight = currHeight.replace("px", "");

                    var newWidth = (parseInt(currWidth) - val);
                    var newHeight = (parseInt(currHeight) - val);
                    newWidth = newWidth.toString() + "px";
                    newHeight = newHeight.toString() + "px";
                    console.log('newWidth > ', newWidth);
                    console.log('newHeight > ', newHeight);
                    myImg.style.width = newWidth;
                    myImg.style.height = newHeight;
                    console.log('myImg.style.width > ', myImg.style.width);
                    console.log('myImg.style.height > ', myImg.style.height);
                }
                break;

            default:
                console.log('Default. id > ');

        }
    }


    $('#logo_image_plus').click(function() {
        if (validElementSelected) {
            IncreaseSize(elementTagName, 1);
        }
    });

    $('#logo_image_minus').click(function() {
        if (validElementSelected) {
            DecreaseSize(elementTagName, 1);
        }
    });


    /*
        $('.logo').click(function(event) {

            console.log('Hi', event);
            console.log('Hi', event.which);
            switch (event.which) {
                case 1:
                    console.log('1. Hi', event.which);
                    
                    break;
                case 2:
                    console.log('2. Hi', event.which);
                     
                    break;
                case 3:
                    console.log('3. Hi', event.which);
                   
                    break;
                default:
                    console.log('4. Hi', event.which);
                    
            }
        });
    */



    $("#btn-svg").on('click', function() {
        console.log('btn-svg > ');

        var svgElement = document.getElementById('band');
        var svgXml = (new XMLSerializer).serializeToString(svgElement);
        var svgData = "data:image/svg+xml," + encodeURIComponent(svgXml);
        console.log('svgData > ', svgData);
        console.log('svgElement > ', svgElement);
        $("#btn-svg").attr("download", "customBandSvg.svg").attr("href", svgData);
    });

    $('#save-btn').on('click', function() {
        console.log('save-btn > ');

        var svgElementText = document.getElementById('divSVG').innerHTML.toString();
        console.log('save-btn > svgElementText > ', svgElementText);

        var Data = {
            id: '123456',
            svg: svgElementText
        };

        console.log('save-btn > Data > ', Data);





    });


    //-----------------------------------------------------------

    var mousedownonelement = false;

    window.getlocalmousecoord = function(svg, evt) {
        var pt = svg.createSVGPoint();
        pt.x = evt.clientX;
        pt.y = evt.clientY;
        var localpoint = pt.matrixTransform(svg.getScreenCTM().inverse());
        localpoint.x = Math.round(localpoint.x);
        localpoint.y = Math.round(localpoint.y);
        return localpoint;
    };

    window.createtext = function(localpoint, svg) {
        var myforeign = document.createElementNS('http://www.w3.org/2000/svg', 'foreignObject')
        var textdiv = document.createElement("div");
        var textnode = document.createTextNode("Click to edit");
        textdiv.appendChild(textnode);
        textdiv.setAttribute("contentEditable", "true");
        textdiv.setAttribute("class", "textBox");
        textdiv.setAttribute("width", "auto");
        myforeign.setAttribute("width", "40%");
        myforeign.setAttribute("height", "100%");
        myforeign.classList.add("foreign"); //to make div fit text
        textdiv.classList.add("insideforeign"); //to make div fit text
        textdiv.addEventListener("mousedown", elementMousedown, false);
        myforeign.setAttributeNS(null, "transform", "translate(" + localpoint.x + " " + localpoint.y + ")");
        svg.appendChild(myforeign);
        myforeign.appendChild(textdiv);

    };

    function elementMousedown(evt) {
        mousedownonelement = true;
    }

    /*
        $(('#band')).click(function(evt) {
            var svg = document.getElementById('band');
            var localpoint = getlocalmousecoord(svg, evt);
            console.log('localpoint : ', localpoint);
            return false;
            if (!mousedownonelement) {
                createtext(localpoint, svg);
            } else {
                mousedownonelement = false;
            }
        });
    */

    //---------------------------------------------------------------




});