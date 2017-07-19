(function () {
    $('.user-registration').on('click', function () {
        var $tryLogin = $('.try-login'),
            $infoLabel = $('.info-custom-sm');

        $tryLogin.bind("change keyup input click", function () {
            if (this.value.match(/[^a-z0-9_]/g)) {
                this.value = this.value.replace(/[^a-z0-9_]/g, '');
            }
        });

        $tryLogin.blur(function () {
            $login = $tryLogin.val(),
            status;

            ShowPreloader();

            $.ajax({
                url: '/users/is-login-free',
                type: 'post',
                data: { login: $login }
            })
                .done(function (res, status, xhr) {
                    status = xhr.getResponseHeader('IsLoginFree');
                    if (status === 'true') {
                        $('.registration-button').prop('disabled', false);
                        $('.info-custom-sm').html('');
                        $tryLogin.css({
                            'border': '1px solid #55b852'
                        });
                    }
                    else {
                        $('.registration-button').prop('disabled', true);
                        $('.info-custom-sm').html('Юзер с таким именем уже существует').addClass('error');
                        $tryLogin.css({
                            'border': '1px solid #ff5454'
                        });
                    }

                    HidePreloader();
                })
                .fail(function () {
                    ShowError('Ошибка сервера');
                });
        });
    });

    $('.user-enter-btn').on('click', function (e) {
        var $loginInput = $('.login-input'),
            $passwordInput = $('.password-input'),
            $passwordValue = $passwordInput.val(),
            $loginValue = $loginInput.val(),
            status;

        ShowPreloader();

        $.ajax({
            url: '/users/login',
            type: 'post',
            data: {
                login: $loginValue,
                password: $passwordValue
            }
        })
            .done(function (res, status, xhr) {
                status = xhr.getResponseHeader('IsLoginCorrect');
                if (status === 'true') {
                    location.reload();
                }
                else {
                    $passwordInput.val('');
                    ShowError('Неправильная пара логин/пароль');
                }

                HidePreloader();
            })
            .fail(function () {
                HidePreloader();
                ShowError('Ошибка сервера');
            });
    });

    $(function () {
        var url = window.location.pathname,
            urlRegExp = new RegExp(url.replace(/\/$/, '') + "$");
        $('.menu li a').each(function () {
            if (urlRegExp.test(this.href.replace(/\/$/, ''))) {
                $(this).addClass('active');
            }
        });
    });

    
})();

function ShowPreloader() {
    $('.ajax-status-img').removeClass('hidden');
}

function HidePreloader() {
    $('.ajax-status-img').addClass('hidden');
}

function ShowError(errorMessage) {
    $('#modal-error').modal('show');
    $('#modal-error .modal-title').html(errorMessage);
}