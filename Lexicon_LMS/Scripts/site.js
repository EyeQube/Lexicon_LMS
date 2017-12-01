$(document).ready(function () {
    $('.datepickerDate').datepicker({ format: 'yyyy/mm/dd', autoclose: true, locale: 'sw' });

    $('.js-delete-user').click(function (e) {
        var link = $(e.target);

        bootbox.dialog({
            title: 'Confirm',
            message: "Are you sure you want to delete this user?",
            buttons: {
                no: {
                    label: "No",
                    className: 'btn-default',
                    callback: function () {
                        bootbox.hideAll();
                    }
                },
                yes: {
                    label: "Yes",
                    className: 'btn-danger',
                    callback: function () {
                        $.ajax({
                            url: "/api/Account/" + link.attr("data-user-id"),
                            method: "DELETE"
                        })
                            .done(function () {
                                link.parents("tr").fadeOut(function () {
                                    $(this).remove();
                                });
                            })
                            .fail(function () {
                                alert("Something failed!!");
                            });

                    }
                }
            }
        });

    });
});