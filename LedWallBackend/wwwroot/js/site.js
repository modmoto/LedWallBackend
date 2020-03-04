const canvas = document.getElementById('sketchpad');
const context = canvas.getContext('2d');

canvas.width = window.innerWidth;
canvas.height = window.innerHeight;

context.fillStyle = "black";
context.fillRect(0, 0, canvas.width, canvas.height);

const colorPicker = new iro.ColorPicker('#color-picker-container', {
    width: 420,
    color: "#0066ff"
});

const cpContainer = document.getElementById('color-picker-container');

window.addEventListener('load', function () {
    const drawer = {
        isDrawing: false,
        touchstart: function (coors) {
            context.beginPath();
            context.moveTo(coors.x, coors.y);
            this.isDrawing = true;
        },
        touchmove: function (coors) {
            if (this.isDrawing) {
                context.lineTo(coors.x, coors.y);
                context.strokeStyle = colorPicker.color.hexString;
                context.lineWidth = window.innerWidth / 40;
                context.lineCap = "round";

                context.stroke();
            }
        },
        touchend: function (coors) {
            if (this.isDrawing) {
                this.touchmove(coors);
                this.isDrawing = false;
            }
        }
    };

    function draw(event) {
        let type = null;
        switch(event.type){
            case "mousedown":
                event.touches = [];
                event.touches[0] = {
                    pageX: event.pageX,
                    pageY: event.pageY
                };
                type = "touchstart";
                break;
            case "mousemove":
                event.touches = [];
                event.touches[0] = {
                    pageX: event.pageX,
                    pageY: event.pageY
                };
                type = "touchmove";
                break;
            case "mouseup":
                event.touches = [];
                event.touches[0] = {
                    pageX: event.pageX,
                    pageY: event.pageY
                };
                type = "touchend";
                break;
        }

        let coors;
        if(event.type === "touchend") {
            coors = {
                x: event.changedTouches[0].pageX,
                y: event.changedTouches[0].pageY
            };
        }
        else {
            coors = {
                x: event.touches[0].pageX,
                y: event.touches[0].pageY
            };
        }
        type = type || event.type;
        drawer[type](coors);
    }

    let touchAvailable = ('createTouch' in document) || ('ontouchstart' in window);

    if(touchAvailable){
        canvas.addEventListener('touchstart', draw, false);
        canvas.addEventListener('touchmove', draw, false);
        canvas.addEventListener('touchend', draw, false);
    }
    else {
        canvas.addEventListener('mousedown', draw, false);
        canvas.addEventListener('mousemove', draw, false);
        canvas.addEventListener('mouseup', draw, false);
    }

    document.body.addEventListener('touchmove', function (event) {
        event.preventDefault();
    }, false);

}, false);

const saveButton = document.getElementById('btnSendPictureToGrenke');
saveButton.onclick = function uploadImage(event) {
    let dataURL = canvas.toDataURL("image/png");
    const postImage = dataURL.replace('data:image/png;base64,', '');

    $.ajax({
        type: 'POST',
        url: "../../Home/Index",
        data: JSON.stringify({ imageAsBase64: postImage }),
        contentType: 'application/json; charset=utf-8'
    });
    location.href = location.href;
};

const fabButton = document.getElementById('fab-button');
fabButton.onclick = function openColorPicker(event) {
    if (cpContainer.style.display === "none" || !cpContainer.style.display) {
        cpContainer.style.display = "block";
    } else {
        cpContainer.style.display = "none";
    }
};

fabButton.style.backgroundColor = colorPicker.color.hexString;

function onColorChange(color) {
    fabButton.style.backgroundColor = color.hexString;
}

colorPicker.on('color:change', onColorChange);
