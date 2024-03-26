$.ajax({
    url: '@Url.Action("DownloadFile", "Magazine")', // Adjust the URL to match your controller and action
    type: 'GET',
    data: { documentId: id }, // Pass the document ID to the controller action
    success: function (response) {
        // Check if the response contains file content or file path
        if (response.success) {
            // Create a hidden link and trigger the download
            var link = document.createElement('a');
            link.href = response.filePath; // Adjust property name based on the response
            link.download = response.fileName; // Adjust property name based on the response
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        } else {
            // Handle error or display message
            console.error('Error downloading file:', response.message);
        }
    },
    error: function (xhr, status, error) {
        console.error('Error:', error);
    }
});
