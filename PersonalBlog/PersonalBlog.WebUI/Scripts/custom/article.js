(function () {
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

    $('.tag-btn').on('click', function () {
        var $title = $(this).data('title');

        $('.tags-all-input').val($title);
    });

    $('.article-btn').click(function () {
        var arr = $('input:checkbox:checked').map(function () { return this.value; }).get();
        $('.tags-input').val(arr);
    });

    $(function () {
        var checkBoxes = $('input[type="checkbox"]');
        checkBoxes.change(function () {
            $('.article-btn').prop('disabled', checkBoxes.filter(':checked').length < 1);
        });
        $('input[type="checkbox"]').change();
    });
})();