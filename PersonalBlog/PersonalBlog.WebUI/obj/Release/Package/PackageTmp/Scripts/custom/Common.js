(function () {
    $('.users-list').on('click', '.add-role', function (e) {
        var $target = $(e.target),
            $trItem = $target.closest('tr'),
            $userId = $trItem.data('id');

        $('.add-this-role').on('click', function (e) {
            var $target = $(e.target),
                $trItem = $target.closest('tr'),
                $roleId = $trItem.data('id');

            $.ajax({
                url: '/users/add-role-to-user',
                type: 'post',
                data: {
                    userId: $userId,
                    roleId: $roleId
                }
            })
                .done(function () {
                    location.reload();
                })
                .fail(function () {
                    ShowError('Пользователь уже имеет эту роль!');
                });
        });
    });

    $('.users-list').on('click', '.delete-user', function (e) {
        var $target = $(e.target),
            $trItem = $target.closest('tr'),
            $id = $trItem.data('id');

        ShowPreloader();

        $.ajax({
            url: '/users/delete',
            type: 'post',
            data: { id: $id }
        })
            .done(function () {
                HidePreloader();
                $trItem.remove();
            })
            .fail(function () {
                HidePreloader();
                ShowError('Ошибка при удалении пользователя!');
            });
    });

    $('.users-list').on('click', '.delete-current-role', function (e) {
        var $target = $(e.target),
            $liItem = $target.closest('li'),
            $roleTitle = $liItem.data('role-title'),
            $userId = $liItem.data('user-id');

        ShowPreloader();

        $.ajax({
            url: '/users/delete-current-role',
            type: 'post',
            data: {
                title: $roleTitle,
                userId: $userId
            }
        })
            .done(function () {
                HidePreloader();
                $liItem.remove();
            })
            .fail(function () {
                HidePreloader();
                ShowError('Ошибка при удалении роли!');
            });
    });

    $('.posts').on('click', '.delete-post', function (e) {
        var $target = $(e.target),
            $listItem = $target.closest('li'),
            id = $listItem.data('id');

        ShowPreloader();

        $.ajax({
            url: '/articles/delete',
            type: 'post',
            data: { id: id }
        })
            .done(function () {
                HidePreloader();
                $listItem.remove();
            })
            .fail(function () {
                HidePreloader();
                ShowError('Запись не может быть удалена!');
            });
    });

    $('.posts-table').on('click', '.delete-post', function (e) {
        var $target = $(e.target),
            $trItem = $target.closest('tr'),
            $id = $trItem.data('id');

        ShowPreloader();

        $.ajax({
            url: '/articles/delete',
            type: 'post',
            data: { id: $id }
        })
            .done(function () {
                HidePreloader();
                $trItem.remove();
            })
            .fail(function () {
                HidePreloader();
                ShowError('Запись не может быть удалена!');
            });
    });

    $('.posts-table').on('click', '.edit-post', function (e) {
        var $target = $(e.target),
            $trItem = $target.closest('tr'),
            $id = $trItem.data('id'),
            $title = $trItem.data('title'),
            $text = $trItem.data('text'),
            $imgPath = $trItem.data('img');
        nameOnly = $imgPath.split('/').pop();

        $('.post-img').attr('src', '/images/articles/' + $imgPath).attr('alt', $title);
        $('.edit-title').val($title);
        $('.edit-text').val($text);
        $('.title-id').val($id);
        $('.old-user-img').val(nameOnly);
    });

    $('.posts').on('click', '.approve-post', function (e) {
        var $target = $(e.target),
            $listItem = $target.closest('li'),
            id = $listItem.data('id');

        ShowPreloader();

        $.ajax({
            url: '',
            type: 'post',
            data: { id: id }
        })
            .done(function () {
                HidePreloader();
                $listItem.remove();
            })
            .fail(function () {
                HidePreloader();
                ShowError('Ошибка при модерации записи!');
            });
    });

    $('.posts').on('click', '.edit-post', function (e) {
        var $target = $(e.target),
            $listItem = $target.closest('li'),
            $id = $listItem.data('id'),
            $title = $listItem.data('title'),
            $text = $listItem.data('text'),
            $imgPath = $listItem.data('img'),
            nameOnly = $imgPath.split('/').pop();

        $('.post-img').attr('src', '/images/articles/' + $imgPath).attr('alt', $title);
        $('.edit-title').val($title);
        $('.edit-text').val($text);
        $('.title-id').val($id);
        $('.old-user-img').val(nameOnly);
    });

    $('.tags-table').on('click', '.delete-tag', function (e) {
        var $target = $(e.target),
            $trItem = $target.closest('tr'),
            id = $trItem.data('id');

        ShowPreloader();

        $.ajax({
            url: '/tags/delete',
            type: 'post',
            data: { id: id }
        })
            .done(function () {
                HidePreloader();
                $trItem.remove();
            })
            .fail(function () {
                HidePreloader();
                ShowError('Удаление тега невозможно!');
            });
    });

    $('.tags-table').on('click', '.edit-tag', function (e) {
        var $target = $(e.target),
            $trItem = $target.closest('tr'),
            $id = $trItem.data('id'),
            $title = $trItem.data('title'),
            code = '<input type="text" class="form-control input-custom" id="editable" value="' + $title + '" />';

        $trItem.children(':first').empty().append(code);
        $('#editable').focus();
        $('#editable').blur(function () {
            $title = $(this).val();

            $(this).parent().empty().html($title);
            ShowPreloader();

            $.ajax({
                url: '/tags/edit',
                type: 'post',
                data: { id: $id, title: $title }
            })
                .done(function () {
                    HidePreloader();
                    $trItem.data('title', $title);
                })
                .fail(function () {
                    HidePreloader();
                    ShowError('Ошибка при редактировании тега!');
                });
        });
    });

    $('.user-registration').on('click', function () {
        var $tryLogin = $('.try-login'),
            $infoLabel = $('.info-custom-sm');

        $tryLogin.bind("change keyup input click", function () {
            if (this.value.match(/[^a-z0-9_]/g)) {
                this.value = this.value.replace(/[^a-z0-9_]/g, '');
            }
        });

        $tryLogin.blur(function () {
            $login = $tryLogin.val();

            ShowPreloader();

            $.ajax({
                url: '/users/is-login-free',
                type: 'post',
                data: { login: $login }
            })
                .done(function (res, status, xhr) {
                    var status = xhr.getResponseHeader('IsLoginFree');
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

    $('.creating-tag-btn').on('click', function (e) {
        var $target = $(e.target),
            $newTag = $('.creating-tag-input').val();

        $.ajax({
            url: '/tags/add',
            type: 'post',
            data: { title: $newTag }
        })
            .done(function () {
                location.reload();
            })
            .fail(function () {
                ShowError('Тег не может быть добавлен!');
            });
    });

    $('.user-enter-btn').on('click', function (e) {
        var $loginInput = $('.login-input'),
            $passwordInput = $('.password-input'),
            $passwordValue = $passwordInput.val(),
            $loginValue = $loginInput.val();

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
                var status = xhr.getResponseHeader('IsLoginCorrect');
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

    $('.tag-btn').on('click', function () {
        var $title = $(this).data('title');

        $('.tags-all-input').val($title);
    });

    $('.article-btn').click(function () {
        var arr = $('input:checkbox:checked').map(function () { return this.value; }).get();
        $('.tags-input').val(arr);
    });

    $('.comment').on('click', function (e) {
        var $target = $(e.target),
            $liItem = $target.closest('li'),
            $articleId = $liItem.data('id');

        $('.create-comment-btn').on('click', function () {
            var $commentTitle = $('.create-comment-title').val(),
                $commentText = $('.create-comment-text').val();

            ShowPreloader();

            $.ajax({
                url: '/comments/add',
                type: 'post',
                data: {
                    articleId: $articleId,
                    title: $commentTitle,
                    text: $commentText
                }
            })
                .done(function () {
                    $('.add-comment-status').html('Комментарий появится после проверки модератором');
                    $('.create-comment-btn').html('Закрыть').removeClass('create-comment-btn');
                    location.reload();
                })
                .fail(function () {
                    HidePreloader();
                    ShowError('Ошибка при добавлении комментария!');
                });
        });
    });

    $('.comments-list').on('click', '.approve-comment', function (e) {
        var $target = $(e.target),
            $listItem = $target.closest('li'),
            id = $listItem.data('id');

        ShowPreloader();

        $.ajax({
            url: '',
            type: 'post',
            data: { id: id }
        })
            .done(function () {
                HidePreloader();
                $listItem.remove();
            })
            .fail(function () {
                HidePreloader();
                ShowError('Ошибка при модерации комментария');
            });
    });

    $('.comments-list').on('click', '.delete-comment', function (e) {
        var $target = $(e.target),
            $listItem = $target.closest('li'),
            $id = $listItem.data('id');

        ShowPreloader();

        $.ajax({
            url: '/comments/delete',
            type: 'post',
            data: { id: $id }
        })
            .done(function () {
                HidePreloader();
                $listItem.remove();
            })
            .fail(function () {
                HidePreloader();
                ShowError('Запись не может быть удалена!');
            });
    });

    $('#add-comment').on('hidden.bs.modal', function () {
        $('.create-comment-title').val('');
        $('.create-comment-text').val('');
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

    $(function () {
        var checkBoxes = $('input[type="checkbox"]');
        checkBoxes.change(function () {
            $('.article-btn').prop('disabled', checkBoxes.filter(':checked').length < 1);
        });
        $('input[type="checkbox"]').change();
    });

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
})();