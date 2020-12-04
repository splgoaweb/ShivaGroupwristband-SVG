const position = {
    x: 0,
    y: 0
}

interact('.logo')

.draggable({
        listeners: {
            start(event) {
                console.log('->', event.type, event.target)
            },
            move(event) {

                if (event.client.x >= 393) {
                    console.log('event -------->', event);
                    console.log('event.type -------->', event.type);
                    console.log('event.target -------->', event.target);
                    console.log('event.client -------->', event.client);

                    // if (event.client.y >= 329) {

                    position.x += event.dx
                    position.y += event.dy
                    console.log('position.x > ', position.x);
                    console.log('position.y > ', position.y);

                    // if (position.x > 0 && position.x <= 600) {
                    console.log('Move Now');
                    event.target.style.transform =
                        `translate(${position.x}px, ${position.y}px)`
                        // }
                        // }


                }


            },
        }
    })
    .resizable({
        edges: {
            top: true,
            left: true,
            bottom: true,
            right: true
        },
        listeners: {
            move: function(event) {
                let {
                    x,
                    y
                } = event.target.dataset

                x = (parseFloat(x) || 0) + event.deltaRect.left
                y = (parseFloat(y) || 0) + event.deltaRect.top

                console.log('event > ', event);
                console.log('event.target.style > ', event.target.style);
                console.log('event.rect.width > ', event.rect.width);
                console.log('event.rect.height > ', event.rect.height);

                if (event.rect.width <= 900 && event.rect.height <= 130) {
                    Object.assign(event.target.style, {
                        width: `${event.rect.width}px`,
                        height: `${event.rect.height}px`,
                        //transform: `translate(${x}px, ${y}px)`
                    })

                    Object.assign(event.target.dataset, {
                        x,
                        y
                    })
                }


            }
        }
    })

//----------------------------------------------------------------------------------------------------------------