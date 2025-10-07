// LocalStorage Helper Functions
window.localStorageHelper = {
    getItem: function (key) {
        try {
            if (typeof Storage !== "undefined") {
                return localStorage.getItem(key);
            }
            return null;
        } catch (e) {
            console.warn('localStorage.getItem failed:', e);
            return null;
        }
    },
    
    setItem: function (key, value) {
        try {
            if (typeof Storage !== "undefined") {
                localStorage.setItem(key, value);
                return true;
            }
            return false;
        } catch (e) {
            console.warn('localStorage.setItem failed:', e);
            return false;
        }
    },
    
    removeItem: function (key) {
        try {
            if (typeof Storage !== "undefined") {
                localStorage.removeItem(key);
                return true;
            }
            return false;
        } catch (e) {
            console.warn('localStorage.removeItem failed:', e);
            return false;
        }
    },
    
    isAvailable: function () {
        try {
            return typeof Storage !== "undefined" && localStorage !== null;
        } catch (e) {
            return false;
        }
    },
    
    // Theme management
    setThemeAttribute: function (theme) {
        try {
            document.documentElement.setAttribute('data-theme', theme);
            return true;
        } catch (e) {
            console.warn('setThemeAttribute failed:', e);
            return false;
        }
    },
    
    initializeTheme: function () {
        try {
            const theme = this.getItem('cipp-theme') || 'light';
            this.setThemeAttribute(theme);
            return theme;
        } catch (e) {
            console.warn('initializeTheme failed:', e);
            this.setThemeAttribute('light');
            return 'light';
        }
    }
};
