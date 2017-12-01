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
    
    // General function for removing stuff in database and DOM.
    // Uses the following html data attributes:
    //    data-user-api:    Database API for removing something
    //    data-user-id:     Id for API
    //    data-user-remove: DOM element to remove on success
    //    data-user-message:Text to show in confirmation box
    //
    $('.js-delete-entity').click(function (e) {
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
                                link.closest( link.attr("data-user-remove") ).fadeOut(function () {
                                    $(this).remove();
                                });
                            })
                            .fail(function (xhr) {
                                alert(xhr.responseText);
                            });

                    }
                }
            }
        });

    });


});