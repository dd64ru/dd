(function () {
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
            code = $('<input>').attr({
                type: 'text',
                class: 'form-control input-custom',
                id: 'editable',
                value: $title
            });


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
})();