document.getElementById('downloadForm').addEventListener('submit', function (event) {
    event.preventDefault();
    document.getElementById('downloadStatus').style.display = 'block';
    document.getElementById('downloadButton').disabled = true;
    var url = document.getElementById('urlInput').value;
    var format = document.getElementById('formatSelect').value;
    var formData = new FormData();
    formData.append('Url', url);
    formData.append('Format', format);
    fetch('/DownloadVideo', {
        method: 'POST',
        body: formData
    })
        .then(response => {
            if (response.ok) {
                return response.json();
            } else {
                throw new Error('Download failed');
            }
        })
        .then(data => {
            var fileName = data.fileName;
            var base64FileBytes = data.fileBytes;
            var binaryFile = atob(base64FileBytes);
            var fileBytes = new Uint8Array(binaryFile.length);
            for (var i = 0; i < binaryFile.length; i++) {
                fileBytes[i] = binaryFile.charCodeAt(i);
            }
            var blob = new Blob([fileBytes], { type: 'application/octet-stream' });
            var downloadUrl = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = downloadUrl;
            a.download = fileName;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            document.getElementById('downloadStatus').style.display = 'none';
            document.getElementById('downloadButton').disabled = false;
        })
        .catch(error => {
            console.error('Error:', error);
            document.getElementById('downloadStatus').style.display = 'none';
            document.getElementById('downloadButton').disabled = false;
            alert('An error occurred during download. Please try again.');
        });
});
