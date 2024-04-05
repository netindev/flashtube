// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $("#fireAction").click(function () {
        var inputData = $("#inputData").val(); // Get the input data
        $.ajax({
            type: "GET",
            url: "/Home/FireAction",
            data: { inputData: inputData }, // Send input data with the request
            success: function (data) {
                console.log(data);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });
});
