document.addEventListener("DOMContentLoaded", function() {
    // HTML elementinden kimlik doğrulama durumunu alma
    const isAuthenticated = document.body.getAttribute('data-is-authenticated') === 'true';

    function toggleAuthLinks(authenticated) {
        const loginLink = document.querySelector('a[href*="Login"]');
        const registerLink = document.querySelector('a[href*="Register"]');

        if (authenticated) {
            if (loginLink) loginLink.style.display = 'none';
            if (registerLink) registerLink.style.display = 'none';
        } else {
            if (loginLink) loginLink.style.display = 'block';
            if (registerLink) registerLink.style.display = 'block';
        }
    }

    toggleAuthLinks(isAuthenticated);

    // Giriş formunu ele al
    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        loginForm.addEventListener('submit', function(event) {
            event.preventDefault(); // Formun varsayılan gönderimini durdur

            const formData = new FormData(loginForm);
            fetch(loginForm.action, {
                method: 'POST',
                body: formData
            })
            .then(response => {
                if (response.ok) {
                    // Giriş başarılıysa sayfayı yeniden yükle
                    location.reload();
                } else {
                    // Hata durumunda kullanıcıyı bilgilendirin
                    alert('Giriş işlemi başarısız oldu.');
                }
            });
        });
    }
});
