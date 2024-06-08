// Function to remove the success message after 10 seconds
setTimeout(function () {
    var successMessage = document.getElementById('successMessage');
    var failedMessage = document.getElementById('failedMessage');
    if (successMessage) {
        successMessage.remove();
    }
    else if (failedMessage) {
        failedMessage.remove();

    }
}, 10000); // 10 seconds in milliseconds

$(".modal-btn").click(function (e) {

    var viewNumber = 1;

    if ($(this).attr('id').toLowerCase() == "reportModal".toLowerCase()) viewNumber = 2;

    $.ajax({
        type: "GET",
        url: $(this).data('url') + '?viewNumber=' + viewNumber,
        success: function (response) {
            $('#loadModalData').html(response);
        },
        error: function () {
            $('#loadModalData').html("Error Generating View");
        }
    })
});

$(document).on("submit", "#registerForm", function (e) {
    if ($("#Completed").val().length < 1 || $("#Planned").val().length < 1) {
        alert("Both field are required");
        e.preventDefault();
    }
});