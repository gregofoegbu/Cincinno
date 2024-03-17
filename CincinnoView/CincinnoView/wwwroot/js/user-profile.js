document.addEventListener('DOMContentLoaded', function () {
    var originalThreshold = document.getElementById('thresholdRange').value;
    var thresholdRange = document.getElementById('thresholdRange');
    var thresholdValue = document.getElementById('thresholdValue');

    thresholdRange.addEventListener('input', function () {
        thresholdValue.textContent = thresholdRange.value+'%';
    });

    var editThresholdBtn = document.getElementById('editThresholdBtn');
    var thresholdSlider = document.getElementById('thresholdSlider');
    var saveThresholdBtn = document.getElementById('saveThresholdBtn');

    editThresholdBtn.addEventListener('click', function () {
        editThresholdBtn.style.display = 'none';
        thresholdSlider.style.display = 'block';
    });

    saveThresholdBtn.addEventListener('click', function () {
        var thresholdValue = document.getElementById('thresholdRange').value;
        var updateThresholdUrl = thresholdSlider.dataset.updateUrl;

        updateThresholdUrl += '?thresholdValue=' + thresholdValue;

        window.location.href = updateThresholdUrl;
    });

    cancelThresholdBtn.addEventListener('click', function () {
        thresholdRange.value = originalThreshold;
        thresholdValue.textContent = originalThreshold + '%';
        thresholdSlider.style.display = 'none';
        editThresholdBtn.style.display = 'block'; 
    });
});

function toggleAddPhotoForm() {
    var form = document.getElementById('addPhotoForm');
    form.style.display = form.style.display === 'none' ? 'block' : 'none';
}

function toggleAddUserForm() {
    var form = document.getElementById('addUserForm');
    form.style.display = form.style.display === 'none' ? 'block' : 'none';
}

function validateUserSelection() {
    var selectedUser = document.getElementById('memberName').value;
    var uploadButton = document.getElementById('uploadButton');
    uploadButton.disabled = selectedUser === '';
}

function validateUserInput() {
    var userInput = document.getElementById('userNameInput').value;
    var addUserButton = document.getElementById('addUserButton');
    addUserButton.disabled = userInput.trim() === '';
}

document.addEventListener("DOMContentLoaded", function () {
    var deleteLinks = document.querySelectorAll(".delete-member");
    deleteLinks.forEach(function (link) {
        link.addEventListener("click", function (event) {
            event.preventDefault();
            var memberName = link.getAttribute("data-member");
            var url = link.getAttribute("data-delete-url")
            if (confirm("Are you sure you want to delete member " + memberName + "?")) {
                // If user confirms, redirect to delete action
                window.location.href = url + '?memberName=' + encodeURIComponent(memberName);
            }
        });
    });
});