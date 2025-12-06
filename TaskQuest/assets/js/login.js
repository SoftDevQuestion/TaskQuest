
class MaterialLoginForm {
    constructor() {
        this.form = document.getElementById('loginForm');
        this.emailInput = document.getElementById('email');
        this.passwordInput = document.getElementById('password');
        this.usernameInput = document.getElementById('username');
        this.confirmPasswordInput = document.getElementById('confirmPassword');
        this.passwordToggle = document.getElementById('passwordToggle');
        this.submitButton = this.form.querySelector('.material-btn');
        this.successMessage = document.getElementById('successMessage');
        this.socialButtons = document.querySelectorAll('.social-btn');
        
        this.init();
    }
    
    init() {
        this.bindEvents();
        this.setupPasswordToggle();
        this.setupSocialButtons();
        this.setupRippleEffects();
    }
    
    bindEvents() {
        this.form.addEventListener('submit', (e) => this.handleSubmit(e));
        
        if (this.emailInput) {
            this.emailInput.addEventListener('blur', () => this.validateEmail());
            this.emailInput.addEventListener('input', () => this.clearError('email'));
            this.emailInput.addEventListener('focus', (e) => this.handleInputFocus(e));
            this.emailInput.addEventListener('blur', (e) => this.handleInputBlur(e));
        }

        if (this.passwordInput) {
            this.passwordInput.addEventListener('blur', () => this.validatePassword());
            this.passwordInput.addEventListener('input', () => this.clearError('password'));
            this.passwordInput.addEventListener('focus', (e) => this.handleInputFocus(e));
            this.passwordInput.addEventListener('blur', (e) => this.handleInputBlur(e));
        }

        if (this.usernameInput) {
            this.usernameInput.addEventListener('blur', () => this.validateUsername());
            this.usernameInput.addEventListener('input', () => this.clearError('username'));
            this.usernameInput.addEventListener('focus', (e) => this.handleInputFocus(e));
            this.usernameInput.addEventListener('blur', (e) => this.handleInputBlur(e));
        }

        if (this.confirmPasswordInput) {
            this.confirmPasswordInput.addEventListener('blur', () => this.validateConfirmPassword());
            this.confirmPasswordInput.addEventListener('input', () => this.clearError('confirmPassword'));
            this.confirmPasswordInput.addEventListener('focus', (e) => this.handleInputFocus(e));
            this.confirmPasswordInput.addEventListener('blur', (e) => this.handleInputBlur(e));
        }
    }
    
    setupPasswordToggle() {
        this.passwordToggle.addEventListener('click', (e) => {
            this.createRipple(e, this.passwordToggle.querySelector('.toggle-ripple'));
            
            const type = this.passwordInput.type === 'password' ? 'text' : 'password';
            this.passwordInput.type = type;
            
            const icon = this.passwordToggle.querySelector('.toggle-icon');
            icon.classList.toggle('show-password', type === 'text');
        });
    }
    
    setupSocialButtons() {
        this.socialButtons.forEach(button => {
            button.addEventListener('click', (e) => {
                const provider = button.classList.contains('google-material') ? 'Google' : 'Facebook';
                this.createRipple(e, button.querySelector('.social-ripple'));
                this.handleSocialLogin(provider, button);
            });
        });
    }
    
    setupRippleEffects() {
        // Setup ripples for inputs
        const inputs = [this.emailInput, this.passwordInput, this.usernameInput, this.confirmPasswordInput];
        inputs.forEach(input => {
            if (input) {
                input.addEventListener('focus', (e) => {
                    const rippleContainer = input.parentNode.querySelector('.ripple-container');
                    if (rippleContainer) {
                        this.createRipple(e, rippleContainer);
                    }
                });
            }
        });
        
        // Setup ripple for main button
        if (this.submitButton) {
            this.submitButton.addEventListener('click', (e) => {
                const ripple = this.submitButton.querySelector('.btn-ripple');
                if (ripple) this.createRipple(e, ripple);
            });
        }
        
        // Setup checkbox ripple
        const checkbox = document.querySelector('.checkbox-wrapper');
        if (checkbox) {
            checkbox.addEventListener('click', (e) => {
                const rippleContainer = checkbox.querySelector('.checkbox-ripple');
                if (rippleContainer) this.createRipple(e, rippleContainer);
            });
        }
    }
    
    createRipple(event, container) {
        const rect = container.getBoundingClientRect();
        const size = Math.max(rect.width, rect.height);
        const x = event.clientX - rect.left - size / 2;
        const y = event.clientY - rect.top - size / 2;
        
        const ripple = document.createElement('div');
        ripple.className = 'ripple';
        ripple.style.width = ripple.style.height = size + 'px';
        ripple.style.left = x + 'px';
        ripple.style.top = y + 'px';
        
        container.appendChild(ripple);
        
        // Remove ripple after animation
        setTimeout(() => {
            ripple.remove();
        }, 600);
    }
    
    handleInputFocus(e) {
        const inputWrapper = e.target.closest('.input-wrapper');
        inputWrapper.classList.add('focused');
    }
    
    handleInputBlur(e) {
        const inputWrapper = e.target.closest('.input-wrapper');
        inputWrapper.classList.remove('focused');
    }
    
    validateUsername() {
        if (!this.usernameInput) return true;

        const username = this.usernameInput.value.trim();
        if (!username) {
            this.showError('username', 'Username is required');
            return false;
        }
        
        if (username.length < 3) {
            this.showError('username', 'Username must be at least 3 characters');
            return false;
        }

        this.clearError('username');
        return true;
    }

    validateConfirmPassword() {
        if (!this.confirmPasswordInput) return true;

        const confirmPassword = this.confirmPasswordInput.value;
        const password = this.passwordInput.value;

        if (!confirmPassword) {
            this.showError('confirmPassword', 'Please confirm your password');
            return false;
        }

        if (confirmPassword !== password) {
            this.showError('confirmPassword', 'Passwords do not match');
            return false;
        }

        this.clearError('confirmPassword');
        return true;
    }

    validateEmail() {
        const email = this.emailInput.value.trim();
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        
        if (!email) {
            this.showError('email', 'Email is required');
            return false;
        }
        
        if (!emailRegex.test(email)) {
            this.showError('email', 'Enter a valid email address');
            return false;
        }
        
        this.clearError('email');
        return true;
    }
    
    validatePassword() {
        const password = this.passwordInput.value;
        
        if (!password) {
            this.showError('password', 'Password is required');
            return false;
        }
        
        if (password.length < 6) {
            this.showError('password', 'Password must be at least 6 characters');
            return false;
        }
        
        this.clearError('password');
        return true;
    }
    
    showError(field, message) {
        const formGroup = document.getElementById(field).closest('.form-group');
        const errorElement = document.getElementById(`${field}Error`);
        
        formGroup.classList.add('error');
        errorElement.textContent = message;
        errorElement.classList.add('show');
        
        // Add Material Design shake animation
        const input = document.getElementById(field);
        input.style.animation = 'materialShake 0.4s ease-in-out';
        setTimeout(() => {
            input.style.animation = '';
        }, 400);
    }
    
    clearError(field) {
        const formGroup = document.getElementById(field).closest('.form-group');
        const errorElement = document.getElementById(`${field}Error`);
        
        formGroup.classList.remove('error');
        errorElement.classList.remove('show');
        setTimeout(() => {
            errorElement.textContent = '';
        }, 200);
    }
    
    async handleSubmit(e) {
        
        let isValid = true;

        if (this.emailInput && !this.validateEmail()) isValid = false;
        if (this.passwordInput && !this.validatePassword()) isValid = false;
        if (this.usernameInput && !this.validateUsername()) isValid = false;
        if (this.confirmPasswordInput && !this.validateConfirmPassword()) isValid = false;
        
        if (!isValid) {
            e.preventDefault();
            // Add material feedback for invalid form
            if (this.submitButton) {
                this.submitButton.style.animation = 'materialPulse 0.3s ease';
                setTimeout(() => {
                    this.submitButton.style.animation = '';
                }, 300);
            }
            return;
        }
        
        // Form is valid, allow default submission (PostBack)
        this.setLoading(true);
    }
    
    async handleSocialLogin(provider, button) {
        console.log(`Initiating ${provider} sign-in...`);
        
        // Add Material loading state
        button.style.pointerEvents = 'none';
        button.style.opacity = '0.7';
        
        try {
            if (provider === 'Google') {
                // Use Google Identity Services API
                if (window.google && window.google.accounts) {
                    // Show Google sign-in popup
                    window.google.accounts.id.initialize({
                        client_id: 'YOUR_GOOGLE_CLIENT_ID.apps.googleusercontent.com',
                        callback: this.handleGoogleCallback.bind(this),
                        auto_select: false,
                        cancel_on_tap_outside: true
                    });
                    
                    // Show the Google sign-in prompt
                    window.google.accounts.id.prompt();
                } else {
                    console.error('Google Identity Services not loaded');
                    alert('Google login service is not available. Please try again later.');
                }
            }
        } catch (error) {
            console.error(`${provider} authentication failed: ${error.message}`);
            alert('Login with Google failed. Please try again.');
        } finally {
            button.style.pointerEvents = 'auto';
            button.style.opacity = '1';
        }
    }
    
    handleGoogleCallback(response) {
        console.log('Google login callback received');
        
        if (response.credential) {
            // Create a hidden form to submit the Google credential to the server
            const form = document.createElement('form');
            form.method = 'POST';
            form.action = 'Login.aspx';
            
            // Add credential field
            const credentialInput = document.createElement('input');
            credentialInput.type = 'hidden';
            credentialInput.name = 'credential';
            credentialInput.value = response.credential;
            form.appendChild(credentialInput);
            
            // Add submit button to trigger postback
            const submitButton = document.createElement('input');
            submitButton.type = 'hidden';
            submitButton.name = '__EVENTTARGET';
            submitButton.value = '';
            form.appendChild(submitButton);
            
            document.body.appendChild(form);
            form.submit();
        } else {
            console.error('No credential received from Google');
            alert('Google login failed. Please try again.');
        }
    }
    
    setLoading(loading) {
        this.submitButton.classList.toggle('loading', loading);
        this.submitButton.disabled = loading;
        
        // Disable social buttons during login
        this.socialButtons.forEach(button => {
            button.style.pointerEvents = loading ? 'none' : 'auto';
            button.style.opacity = loading ? '0.6' : '1';
        });
    }
    
    showMaterialSuccess() {
        // Hide form with Material motion
        this.form.style.transform = 'translateY(-16px) scale(0.95)';
        this.form.style.opacity = '0';
        
        setTimeout(() => {
            this.form.style.display = 'none';
            document.querySelector('.social-login').style.display = 'none';
            document.querySelector('.signup-link').style.display = 'none';
            
            // Show success with Material elevation
            this.successMessage.classList.add('show');
            
            // Add Material success animation
            const successIcon = this.successMessage.querySelector('.success-icon');
            successIcon.style.animation = 'materialSuccessScale 0.5s cubic-bezier(0.25, 0.8, 0.25, 1)';
            
        }, 300);
        
        // Simulate redirect with Material timing
        setTimeout(() => {
            console.log('Redirecting to dashboard...');
            // window.location.href = '/dashboard';
        }, 2500);
    }
}

// Add Material Design specific animations
if (!document.querySelector('#material-keyframes')) {
    const style = document.createElement('style');
    style.id = 'material-keyframes';
    style.textContent = `
        @keyframes materialShake {
            0%, 100% { transform: translateX(0); }
            25% { transform: translateX(-4px); }
            75% { transform: translateX(4px); }
        }
        
        @keyframes materialPulse {
            0% { transform: scale(1); }
            50% { transform: scale(1.02); }
            100% { transform: scale(1); }
        }
        
        @keyframes materialSuccessScale {
            0% { transform: scale(0); }
            50% { transform: scale(1.1); }
            100% { transform: scale(1); }
        }
    `;
    document.head.appendChild(style);
}

// Initialize the form when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    new MaterialLoginForm();
});