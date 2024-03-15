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
