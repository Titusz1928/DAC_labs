document.getElementById('uploadForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const fileInput = document.getElementById('fileInput');
    const fileNameInput = document.getElementById('fileNameInput'); // Input for file name
    const formData = new FormData();

    if (!fileInput.files.length) {
        alert('Please select a file to upload.');
        return; // Prevent submission if no file is selected
    }

    formData.append('fileName', fileNameInput.value); // Append the file name
    formData.append('file', fileInput.files[0]); // Append the file

    const response = await fetch('/api/files/upload', {
        method: 'POST',
        body: formData,
    });

    if (response.ok) {
        alert('File uploaded successfully!');
        fileInput.value = ''; // Clear the file input
        fileNameInput.value = ''; // Clear the file name input
        loadFiles(); // Refresh the file list
    } else {
        alert('Error uploading file');
    }
});

// Function to load files and populate the table
async function loadFiles() {
    const response = await fetch('/api/files'); // Adjust to your actual API endpoint
    const files = await response.json();

    const tableBody = document.getElementById('fileTableBody');
    tableBody.innerHTML = ''; // Clear existing rows

    files.forEach(file => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${file.name}</td>
            <td>${new Date(file.lastUpdated).toLocaleString()}</td> <!-- Adjust based on your file object structure -->
            <td>
                <button onclick="downloadFile('${file.name}')">Download</button>
                <button onclick="deleteFile('${file.name}')">Delete</button>
            </td>
        `;
        tableBody.appendChild(row);
    });
}

// Function to download a file
async function downloadFile(fileName) {
    const response = await fetch(`/api/files/download/${fileName}`);
    if (response.ok) {
        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.style.display = 'none';
        a.href = url;
        a.download = fileName;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        alert('File downloaded successfully!');
    } else {
        alert('Failed to download file');
    }
}

// Function to delete a file
async function deleteFile(fileName) {
    const response = await fetch(`/api/files/${fileName}`, {
        method: 'DELETE',
    });
    if (response.ok) {
        alert('File deleted successfully!');
        loadFiles(); // Refresh the file list after deletion
    } else {
        alert('Failed to delete file');
    }
}

// Load files on page load
window.onload = loadFiles;
