// Choose Avatar Page JavaScript

document.addEventListener('DOMContentLoaded', function() {
    initializeAvatarSelection();
    initializeFileUpload();
});

// Avatar Selection
function initializeAvatarSelection() {
    const avatarOptions = document.querySelectorAll('.avatar-option');
    const selectedAvatarPath = document.getElementById('selectedAvatarPath');
    const selectedInfo = document.getElementById('selectedInfo');
    const selectedAvatarName = document.getElementById('selectedAvatarName');

    avatarOptions.forEach(option => {
        option.addEventListener('click', function() {
            // Remove previous selections
            avatarOptions.forEach(opt => opt.classList.remove('selected'));
            
            // Add selection to clicked option
            this.classList.add('selected');
            
            // Update hidden field
            const avatarName = this.getAttribute('data-avatar');
            selectedAvatarPath.value = 'assets/images/avatars/' + avatarName;
            
            // Show selected info
            selectedAvatarName.textContent = avatarName;
            selectedInfo.style.display = 'block';
            
            // Remove any uploaded file selection
            clearFileUpload();
        });
    });
}

// File Upload
function initializeFileUpload() {
    const uploadArea = document.getElementById('uploadArea');
    const fileInput = document.getElementById('avatarUpload');
    const fileUploadControl = document.querySelector('.file-upload input[type="file"]');
    const uploadPreview = document.getElementById('uploadPreview');
    const previewImage = document.getElementById('previewImage');
    const selectedAvatarPath = document.getElementById('selectedAvatarPath');

    // Click to upload
    uploadArea.addEventListener('click', function() {
        fileInput.click();
    });

    // File input change
    fileInput.addEventListener('change', function(e) {
        handleFileSelect(e.target.files[0]);
    });

    // Drag and drop
    uploadArea.addEventListener('dragover', function(e) {
        e.preventDefault();
        this.classList.add('drag-over');
    });

    uploadArea.addEventListener('dragleave', function(e) {
        e.preventDefault();
        this.classList.remove('drag-over');
    });

    uploadArea.addEventListener('drop', function(e) {
        e.preventDefault();
        this.classList.remove('drag-over');
        const files = e.dataTransfer.files;
        if (files.length > 0) {
            handleFileSelect(files[0]);
        }
    });

    // ASP.NET FileUpload control
    if (fileUploadControl) {
        fileUploadControl.addEventListener('change', function(e) {
            handleFileSelect(e.target.files[0]);
        });
    }

    function handleFileSelect(file) {
        if (!file) return;

        // Validate file type
        if (!file.type.match('image.*')) {
            showError('لطفاً فقط تصویر انتخاب کنید');
            return;
        }

        // Validate file size (max 5MB)
        if (file.size > 5 * 1024 * 1024) {
            showError('حجم تصویر نباید بیشتر از 5 مگابایت باشد');
            return;
        }

        // Create preview
        const reader = new FileReader();
        reader.onload = function(e) {
            previewImage.src = e.target.result;
            uploadPreview.style.display = 'block';
            
            // Clear avatar selections
            clearAvatarSelections();
            
            // Update hidden field with base64 data
            selectedAvatarPath.value = e.target.result;
            
            // Hide upload area
            uploadArea.style.display = 'none';
            
            // Show selected info
            document.getElementById('selectedAvatarName').textContent = file.name;
            document.getElementById('selectedInfo').style.display = 'block';
        };
        reader.readAsDataURL(file);
    }
}

function removeUpload() {
    clearFileUpload();
    
    // Show upload area again
    document.getElementById('uploadArea').style.display = 'block';
    
    // Clear the file input
    const fileInput = document.getElementById('avatarUpload');
    const fileUploadControl = document.querySelector('.file-upload input[type="file"]');
    
    if (fileInput) fileInput.value = '';
    if (fileUploadControl) fileUploadControl.value = '';
}

function clearFileUpload() {
    const uploadPreview = document.getElementById('uploadPreview');
    const previewImage = document.getElementById('previewImage');
    
    uploadPreview.style.display = 'none';
    previewImage.src = '';
}

function clearAvatarSelections() {
    const avatarOptions = document.querySelectorAll('.avatar-option');
    avatarOptions.forEach(opt => opt.classList.remove('selected'));
}

// Error handling
function showError(message) {
    const errorDiv = document.createElement('div');
    errorDiv.className = 'error-message';
    errorDiv.textContent = message;
    errorDiv.style.direction = 'rtl';
    errorDiv.style.textAlign = 'right';
    
    // Remove existing error
    const existingError = document.querySelector('.error-message');
    if (existingError) {
        existingError.remove();
    }
    
    // Insert at the top of the card
    const card = document.querySelector('.avatar-card');
    card.insertBefore(errorDiv, card.firstChild);
    
    // Auto remove after 5 seconds
    setTimeout(() => {
        errorDiv.remove();
    }, 5000);
}

// Form submission
function validateForm() {
    const selectedAvatarPath = document.getElementById('selectedAvatarPath');
    
    if (!selectedAvatarPath.value) {
        showError('لطفاً یک تصویر انتخاب کنید یا آپلود کنید');
        return false;
    }
    
    return true;
}

// Loading state
function setLoading(isLoading) {
    const completeButton = document.getElementById('completeButton');
    const skipButton = document.getElementById('skipButton');
    
    if (isLoading) {
        completeButton.classList.add('loading');
        completeButton.disabled = true;
        skipButton.disabled = true;
    } else {
        completeButton.classList.remove('loading');
        completeButton.disabled = false;
        skipButton.disabled = false;
    }
}

// Add CSS for drag over state
const style = document.createElement('style');
style.textContent = `
    .drag-over {
        border-color: #667eea !important;
        background: #f0f4ff !important;
        transform: scale(1.02);
    }
    
    .upload-area.drag-over .upload-icon {
        animation: bounce 0.5s infinite alternate;
    }
    
    @keyframes bounce {
        from { transform: translateY(0px); }
        to { transform: translateY(-10px); }
    }
`;
document.head.appendChild(style);