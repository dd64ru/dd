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
})();