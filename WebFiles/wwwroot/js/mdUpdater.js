var textField = document.getElementById("mdEditor");
var previewBlock = document.getElementById("mdPreview");
var updButton = document.getElementById("previewUpdateButton");

function updateMd(){
    previewBlock.innerHTML = marked.parse(textField.value);
}

updateMd(); //Initial update

updButton.onclick = updateMd;