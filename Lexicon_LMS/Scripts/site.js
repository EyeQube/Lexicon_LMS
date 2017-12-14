function site_init() {
    $('.datepickerDate').datepicker({
        format: 'yyyy-mm-dd',
        autoclose: true
    });

    $('.datetimepicker').datetimepicker({
        sideBySide: true,
        format: 'YYYY-MM-DD HH:mm'
        
    });
}

$(document).ajaxComplete(function () {
    site_init();
});

$(document).ready(function () {
    site_init();

    // General function for removing stuff in database and DOM.
    // Uses the following html data attributes:
    //    data-user-api:    Database API for removing something
    //    data-user-id:     Id for API (ie. /controller/action/{id} )
    //    data-user-remove: DOM element to remove on success (ie. a css class descriptor)
    //    data-user-message:Text to show in confirmation box
    //
    $(document).on('click', '.js-delete-entity', function (e) {
        var link = $(e.target);

        bootbox.dialog({
            title: 'Confirm Delete',
            message: link.attr("data-user-message"),
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
                            url: link.attr("data-user-api") + link.attr("data-user-id"),
                            method: "DELETE"
                        })
                            .done(function () {
                                link.closest(link.attr("data-user-remove")).fadeOut(function () {
                                    $(this).remove();
                                });
                                $(link.attr("data-user-removeAjax")).children().fadeOut(function () {
                                    $(this).remove();
                                });
                            })
                            .fail(function (xhr) {
                                bootbox.alert(xhr.responseText);
                            });
                    }
                }
            }
        });

    });

});