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

$("#createModalButton").click(function (e) {
    var viewNumber = 1;
    $('#modalLabel').text("Create");

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

    $('#exampleModal').modal("show");

});

$('.Edit').click(function (e) {
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

$('#deleteAll').click(function(e) {
    e.preventDefault();

    $('#exampleModal #modalLabel').text("Do you want to delete all trackers?");

    var deleteAllPermissionBtns = '<div class="p-2">'
        + '<a class="btn btn-danger yes-delete-all" href="/Tracking/DeleteAll">Yes</a>'
        + '<button class="btn btn-secondary mx-2 hideModal">No</button>'
        + '</div>';

    $('#exampleModal #loadModalData').html(deleteAllPermissionBtns);

    $('#exampleModal').modal("show");

});

$('.delete-tracker').click(function (e) {
    e.preventDefault();

    var url = $(this).attr("href");

    $('#exampleModal #modalLabel').text("Do you want to delete?");

    var deletePermissionBtns = '<div class="p-2">'
        + `<a class="btn btn-danger" href="${url}">Yes</a>`
        + '<button class="btn btn-secondary mx-2 hideModal">No</button>'
        + '</div>';

    $('#exampleModal #loadModalData').html(deletePermissionBtns);
    $('#exampleModal').modal("show");

})

// hide modal
$('#exampleModal').on('click', '.hideModal', function (e) {
    $("loadModalData").html("");
    $('#exampleModal').modal("hide");
});

$(document).on("submit", "#registerForm", function (e) {
    if ($("#Completed").val().length < 1 || $("#Planned").val().length < 1) {
        alert("Both field are required");
        e.preventDefault();
    }
});



