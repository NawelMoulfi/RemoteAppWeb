function downloadFromUrl(url, fileName) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}

function saveAsFile(filename, bytesBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + bytesBase64;
    document.body.appendChild(link); // Needed for Firefox
    link.click();
    document.body.removeChild(link);
}

function openFile(bytesBase64) {
    let file = atob(bytesBase64);
    const byteNumbers = new Array(file.length);
    for (let i = 0; i < file.length; i++) {
        byteNumbers[i] = file.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    let blob = new Blob([byteArray], {type: getExtFromBase64(bytesBase64.charAt(0))});
    const url = URL.createObjectURL(blob);
    window.open(url);
}

function getExtFromBase64 (char) {
    switch(char) {
        case '/':
            return "image/jpg";
        case 'i':
            return "image/png";
        case 'R':
            return "image/gif";
        case 'U':
            return "image/webp";
        case 'J':
            return "application/pdf";
        default:
            return "image/png";
    }
}

