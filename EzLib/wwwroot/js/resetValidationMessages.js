$(function () {
    // Get the select element
    var select = $('#Type');

    // Store the original validation summary content
    var originalValidationSummary = $('div.validation-summary-valid').html();

    // Handle change event of the select element
    select.change(function () {
        // Reset validation messages
        $('.field-validation-error').html('');
        $('div.validation-summary-errors').html(originalValidationSummary);
    });
});
