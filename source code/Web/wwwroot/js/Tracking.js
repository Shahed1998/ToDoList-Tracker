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

$(".create-modal-btn, .report-modal-btn").click(function (e) {
    var viewNumber = 1;

    if ($(this).attr('id').toLowerCase() == "reportModal".toLowerCase()) viewNumber = 2;

    switch (viewNumber)
    {
        case 1:
            $('#modalLabel').html("Create");
            break;
        case 2:
            $('#modalLabel').html("Report");
            break;
        default:
            $('#modalLabel').html("");
            break;
    }

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

$('#Edit').click(function(e) {
    e.preventDefault();
    $('#modalLabel').html("Edit");
    $('#loadModalData').html("Loading data while editing");

    $.ajax({
        type: "GET",
        url: $(this).attr('href'),
        success: function (response) {
            $('#loadModalData').html(response);
        },
        error: function () {
            $('#loadModalData').html("Error Generating View");
        }
    })

    $(".edit-modal-btn").click();
})

$(document).on("submit", "#registerForm", function (e) {
    if ($("#Completed").val().length < 1 || $("#Planned").val().length < 1) {
        alert("Both field are required");
        e.preventDefault();
    }
});