$(document).ready(function () {
    // Check if the edit button was previously clicked
    var editClicked = localStorage.getItem('editClicked') === 'true';

    // If edit button was clicked, show the edit section
    if (editClicked) {
        $("#editBtn").hide();
        $("#editButtons").show();
        $("#addFileBtn").show();
        $(".delete-document-btn").show();
    }

    // Edit button click event
    $("#editBtn").click(function () {
        // Show Save and Cancel buttons
        $("#editButtons").show();
        // Show add file button
        $("#addFileBtn").show();
        // Show delete buttons
        $(".delete-document-btn").show();
        // Hide edit button
        $(this).hide();
        // Set edit button clicked state in localStorage
        localStorage.setItem('editClicked', 'true');
    });

    // Cancel button click event
    $("#cancelBtn").click(function () {
        // Hide Save and Cancel buttons
        $("#editButtons").hide();
        // Hide add file button
        $("#addFileBtn").hide();
        // Hide delete buttons
        $(".delete-document-btn").hide();
        // Show edit button
        $("#editBtn").show();
        // Remove edit button clicked state from localStorage
        localStorage.removeItem('editClicked');
    });
});
