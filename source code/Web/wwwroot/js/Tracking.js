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

    $('#exampleModal').modal("show");
})

$(document).on("submit", "#registerForm", function (e) {
    if ($("#Completed").val().length < 1 || $("#Planned").val().length < 1) {
        alert("Both field are required");
        e.preventDefault();
    }
});

$('#deleteAll').click(function(e) {
    e.preventDefault();

    $('#exampleModal #modalLabel').text("Do you want to delete all trackers?");

    var deleteAllPermissionBtns = '<div class="p-2">'
        + '<button class="btn btn-danger yes-delete-all" data-url="/Tracking/DeleteAll">Yes</button>'
        + '<button class="btn btn-secondary mx-2 no-delete-all">No</button>'
        + '</div>';

    $('#exampleModal #loadModalData').html(deleteAllPermissionBtns);

    $('#exampleModal').modal("show");

});

$('#exampleModal').on('click', '.yes-delete-all', function (e) {
    console.log($(this).data('url'))
    $.ajax({
        type: "GET",
        url: $(this).data('url'),
        success: function (response) {
            window.location.href = "/Tracking/Index";
        },
        error: function (err) {
            console.log(err);
            alert("An error occured");
        }
    })
})

$('#exampleModal').on('click', '.no-delete-all', function (e) {
    $('#exampleModal').modal("hide");
});

$('.tdlst_modal_close_btn').click(function (e) {
    e.preventDefault();
    $("loadModalData").html("");
    $('#exampleModal').modal("hide");
});