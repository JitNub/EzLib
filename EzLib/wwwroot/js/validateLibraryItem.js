$(document).ready(function () {
    toggleFields();

    $("#Type").change(toggleFields);
});

function toggleFields() {
    var itemType = $("#Type").val();

    if (itemType === "Book") {
        $("#Author, #Pages, #Title, #Category").prop("disabled", false);
        $("#RunTimeMinutes").prop("disabled", true);
        $('#IsBorrowable').prop({ disabled: true, checked: true });
    } else if (itemType === "DVD") {
        $("#Author, #Pages").prop("disabled", true);
        $("#RunTimeMinutes").prop("disabled", false);
        $('#IsBorrowable').prop({ disabled: true, checked: true });
    } else if (itemType === "Audio Book") {
        $("#Author, #Pages").prop("disabled", true);
        $("#Title, #RunTimeMinutes, #Category").prop("disabled", false);
        $('#IsBorrowable').prop({ disabled: true, checked: true });
    } else if (itemType === "Reference Book") {
        $("#Author, #Pages, #Title, #Category").prop("disabled", false);
        $("#RunTimeMinutes").prop("disabled", true);
        $('#IsBorrowable').prop({ disabled: true, checked: false });
    }
}
