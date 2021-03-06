﻿const canvas = document.getElementById('sketchpad');
const context = canvas.getContext('2d');

canvas.width = window.outerWidth;
canvas.height = window.outerHeight;

context.fillStyle = "black";
context.fillRect(0, 0, canvas.width, canvas.height);

const colorPicker = new iro.ColorPicker('#color-picker-container', {
    width: window.outerWidth > window.outerHeight ? window.outerHeight * 0.6 : window.outerWidth * 0.6,
    color: {
        r: Math.floor(Math.random() * 256),
        g: Math.floor(Math.random() * 256),
        b: Math.floor(Math.random() * 256),
        a: 1,
        saturation: 100,
        hue: 360
    }
});

const cpContainer = document.getElementById('color-picker-container');

window.addEventListener('load', function () {
    const drawer = {
        isDrawing: false,
        touchstart: function (coors) {
            context.beginPath();
            context.moveTo(coors.x , coors.y );
            this.isDrawing = true;
        },
        touchmove: function (coors) {
            if (this.isDrawing) {
                context.lineTo(coors.x, coors.y);

                if(window.outerHeight < window.outerWidth){
                    context.lineWidth = window.outerWidth * 0.03;
                } else {
                    context.lineWidth = window.outerHeight * 0.03;
                }
                context.strokeStyle = colorPicker.color.hexString;
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

}, false);

const saveButton = document.getElementById('btnSendPictureToGrenke');
saveButton.onclick = function uploadImage(event) {
    const loader = document.getElementById('loaderContainer');
    loader.style.visibility = 'visible';

    let dataURL = canvas.toDataURL("image/png");
    const postImage = dataURL.replace('data:image/png;base64,', '');

    $.ajax({
        type: 'POST',
        url: "../../Home/UploadImage",
        data: JSON.stringify({ imageAsBase64: postImage }),
        contentType: 'application/json; charset=utf-8',
        error: function() {
            location.href = location.href;
        },
        success:function() {
            location.href = location.href;
        }
    });
};

const fabButton = document.getElementById('fab-button');
fabButton.onclick = function openColorPicker(event) {
    if (cpContainer.style.display === "none" || !cpContainer.style.display) {
        cpContainer.style.display = "block";
    } else {
        cpContainer.style.display = "none";
    }
};

const uploadButton = document.getElementById('imageInputButton');
uploadButton.onclick = function openColorPicker(event) {
    const imageInput = document.getElementById('imageInput');
    imageInput.click();
};

const imageInput = document.getElementById('imageInput');
imageInput.onchange = function (e) {
    if (e.target.files && e.target.files[0]) {
        const image = new Image();
        image.height = canvas.height;
        image.width = canvas.width;
        image.src = URL.createObjectURL(e.target.files[0]);
        image.onload = function() {
            context.drawImage(image, 0, 0, image.width, image.height);
        }
    }
};

imageInput.onclick = function () {
    this.value = null;
};

fabButton.style.backgroundColor = colorPicker.color.hexString;

function onColorChange(color) {
    fabButton.style.backgroundColor = color.hexString;
}

colorPicker.on('color:change', onColorChange);
