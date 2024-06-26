﻿$(document).ready(function () {
    document.getElementById('thumbnail').style.display = 'none';
});
document.getElementById('downloadForm').addEventListener('submit', function (event) {
    console.log($('#downloadButton').text());
    if ($('#downloadButton').text() === 'Download another!') {
        location.reload(true);
        return;
    }    
    event.preventDefault();
    $('#downloadStatus').html('Fetching video data...').css('display', 'block');
    document.getElementById('downloadButton').disabled = true;
    var url = document.getElementById('urlInput').value;
    var format = document.getElementById('formatSelect').value;

    var videoData = new FormData();
    videoData.append('Url', url);
    fetch('/VideoData', {
        method: 'POST',
        body: videoData
    })
        .then(data => data.json())
        .then(data => {
            var title = data.title;
            var thumb = data.thumb;
            $('#downloadStatus').html(title).css('display', 'block');
            $('#thumbnail').attr('src', thumb).css('display', 'block');
        })
        .catch(error => {
            console.error('Error:', error);
        });

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
            $('#downloadButton').html('Download another!').prop('disabled', false);            
        })
        .catch(error => {
            console.error('Error:', error);
            $('#downloadButton').prop('disabled', false);
            $('#downloadStatus').html('An error occurred while downloading. Please try again');            
        });
});
