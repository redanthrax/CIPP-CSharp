// DOM Helper utilities to prevent common DOM manipulation errors
window.domHelper = {
    // Safely remove a child element
    safeRemoveChild: function(parent, child) {
        try {
            if (parent && child && parent.contains(child)) {
                parent.removeChild(child);
                return true;
            }
        } catch (error) {
            console.warn('SafeRemoveChild failed:', error.message);
        }
        return false;
    },

    // Safely append a child element
    safeAppendChild: function(parent, child) {
        try {
            if (parent && child && !parent.contains(child)) {
                parent.appendChild(child);
                return true;
            }
        } catch (error) {
            console.warn('SafeAppendChild failed:', error.message);
        }
        return false;
    },

    // Check if element exists and is in DOM
    isElementValid: function(element) {
        return element && 
               element.parentNode && 
               document.contains(element);
    },

    // Safely get element by selector with retry
    safeGetElement: function(selector, maxRetries = 3, delay = 100) {
        return new Promise((resolve) => {
            let attempts = 0;
            
            function tryGetElement() {
                const element = document.querySelector(selector);
                if (element || attempts >= maxRetries) {
                    resolve(element);
                    return;
                }
                
                attempts++;
                setTimeout(tryGetElement, delay);
            }
            
            tryGetElement();
        });
    },

    // Setup error handling for DOM manipulation
    setupDOMErrorHandling: function() {
        // Override removeChild to add safety checks
        const originalRemoveChild = Node.prototype.removeChild;
        Node.prototype.removeChild = function(child) {
            try {
                if (this.contains(child)) {
                    return originalRemoveChild.call(this, child);
                }
                return child;
            } catch (error) {
                console.warn('DOM removeChild error prevented:', error.message);
                return child;
            }
        };

        // Listen for unhandled DOM errors
        window.addEventListener('error', function(event) {
            if (event.message && event.message.includes('removeChild')) {
                console.warn('DOM removeChild error caught:', event.message);
                event.preventDefault();
                return false;
            }
        });

        // Handle mutation observer errors
        if (window.MutationObserver) {
            const originalObserve = MutationObserver.prototype.observe;
            MutationObserver.prototype.observe = function(target, options) {
                try {
                    if (target && document.contains(target)) {
                        return originalObserve.call(this, target, options);
                    }
                } catch (error) {
                    console.warn('MutationObserver error prevented:', error.message);
                }
            };
        }
    },

    // Initialize on page load
    init: function() {
        this.setupDOMErrorHandling();
        console.log('DOM Helper initialized');
    }
};

// Auto-initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => window.domHelper.init());
} else {
    window.domHelper.init();
}