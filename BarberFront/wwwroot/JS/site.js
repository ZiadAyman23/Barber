window.sayHello = function () {
    alert("Hello from JavaScript");
}

// Custom SweetAlert2 styling for calm POS theme
const posSwalMixin = {
    customClass: {
        popup: 'pos-swal-popup',
        title: 'pos-swal-title',
        htmlContainer: 'pos-swal-html',
        confirmButton: 'btn btn-primary px-4 me-2',
        cancelButton: 'btn btn-secondary px-4'
    },
    buttonsStyling: false
};

window.showSuccess = function (message) {
    Swal.fire({
        ...posSwalMixin,
        title: 'Success',
        text: message,
        icon: 'success',
        confirmButtonText: 'OK'
    });
}

window.showToast = function (message, icon = 'success') {
    Swal.fire({
        toast: true,
        position: 'top-end',
        icon: icon,
        title: message,
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        background: '#ffffff',
        color: '#1e293b',
        customClass: {
            popup: 'pos-toast-popup'
        }
    });
}

window.showToast2 = function (message, icon = 'error') {
    Swal.fire({
        toast: true,
        position: 'top-end',
        icon: icon,
        title: message,
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        background: '#ffffff',
        color: '#1e293b',
        customClass: {
            popup: 'pos-toast-popup'
        }
    });
}

window.showToast3 = function (message, icon = 'warning') {
    Swal.fire({
        toast: true,
        position: 'top-end',
        icon: icon,
        title: message,
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        background: '#ffffff',
        color: '#1e293b',
        customClass: {
            popup: 'pos-toast-popup'
        }
    });
}

window.showToast4 = function (message, icon = 'info') {
    Swal.fire({
        toast: true,
        position: 'top-end',
        icon: icon,
        title: message,
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        background: '#ffffff',
        color: '#1e293b',
        customClass: {
            popup: 'pos-toast-popup'
        }
    });
}

window.showLoading = function () {
    Swal.fire({
        title: 'Loading...',
        text: 'Please wait',
        allowOutsideClick: false,
        allowEscapeKey: false,
        background: '#ffffff',
        color: '#1e293b',
        customClass: {
            popup: 'pos-swal-popup'
        },
        didOpen: () => {
            Swal.showLoading();
        }
    });
}

window.hideLoading = function () {
    Swal.close();
}

window.storage = {
    setAccessToken: function (token) {
        document.cookie = "AccessToken=" + token + "; path=/; max-age=2592000; samesite=lax";
        try { localStorage.setItem("AccessToken", token); } catch(e){}
    },

    getAccessToken: function () {
        const name = "AccessToken=";
        const decodedCookie = decodeURIComponent(document.cookie);
        const ca = decodedCookie.split(';');
        for(let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        try { return localStorage.getItem("AccessToken") || ""; } catch(e){}
        return "";
    },

    setRefreshToken: function (token) {
        document.cookie = "RefreshToken=" + token + "; path=/; max-age=2592000; samesite=lax";
        try { localStorage.setItem("RefreshToken", token); } catch(e){}
    },

    getRefreshToken: function () {
        const name = "RefreshToken=";
        const decodedCookie = decodeURIComponent(document.cookie);
        const ca = decodedCookie.split(';');
        for(let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        try { return localStorage.getItem("RefreshToken") || ""; } catch(e){}
        return "";
    },

    clear: function () {
        document.cookie = "AccessToken=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC; samesite=lax";
        document.cookie = "RefreshToken=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC; samesite=lax";
        try { localStorage.removeItem("AccessToken"); localStorage.removeItem("RefreshToken"); } catch(e){}
    }
};