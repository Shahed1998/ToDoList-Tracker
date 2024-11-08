$(document).ready(function () {
    $("#createModalButton").click(function (e) {

        $('#modalLabel').text("Create");

        $.ajax({
            type: "GET",
            url: $(this).data('url'),
            success: function (response) {
                $('#loadModalData').html(response);
                $.validator.unobtrusive.parse('#registerForm');
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
        $('#loadModalData').html("Loading");

        $.ajax({
            type: "GET",
            url: $(this).attr('href'),
            success: function (response) {
                $('#loadModalData').html(response);
                $.validator.unobtrusive.parse('#registerForm');
            },
            error: function () {
                $('#loadModalData').html("Error Generating View");
            }
        })

        $('#exampleModal').modal("show");
    })

    $('#deleteAll').click(function (e) {
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

    //$(document).on("submit", "#registerForm", function (e) {
    //    if ($("#Completed").val().length < 1 || $("#Planned").val().length < 1) {
    //        alert("Both field are required");
    //        e.preventDefault();
    //    }
    //});

    // add asterisk to all required fields
    $('input').each(function () {
        var req = $(this).attr('data-val-required');
        if (req != undefined) {
            var label = $('label[for="' + $(this).attr('Id') + '"]');
            $(label).addClass('required');
        }
    })

    // remove asterisk from the label of all the checkboxes 
    $('input[type="checkbox"]').each(function () {
        $('label[for="' + $(this).attr("id") + '"]').removeClass('required');
    })

});

// password view
function PasswordToggle(fieldId, passwordIconId) {
    var icon = $(passwordIconId);

    if ($(fieldId).attr('type') == "password") {
        $(fieldId).attr('type', 'text');
        icon.removeClass('fa-regular fa-eye-slash').addClass('fa-regular fa-eye');
    }
    else {
        $(fieldId).attr('type', 'password');
        icon.removeClass('fa-regular fa-eye').addClass('fa-regular fa-eye-slash');
    }
}



