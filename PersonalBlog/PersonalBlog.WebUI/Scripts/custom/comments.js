(function () {
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
})();