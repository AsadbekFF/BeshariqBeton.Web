// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Wrapper for GET requests
function httpGet(url, data, callback) {
    $.getJSON(url, data, function () {
    }).done(function (result) {
        callback(result.data);
    }).fail(function (result) {
        hideLoader();
        if (result) {
            console.log(result);

            if (result.responseJSON)
                showErrorMessage(result.responseJSON.errorMessage);
            else
                showErrorMessage();
        }
        else
            showErrorMessage();

        window.scrollTo(0, 0);
    });
}

// Wrapper for POST requests
function httpPost(url, data, callback, failedCallback) {
    $.post(url, data, function () { })
        .done(function (result) {
            callback(result.data);
        })
        .fail(function (result) {
            hideLoader();
            console.log(result);
            if (result.responseJSON)
                showErrorMessage(result.responseJSON.errorMessage);
            else
                showErrorMessage();

            window.scrollTo(0, 0);

            if (failedCallback)
                failedCallback();
        });
}

function showLoader() {
    $('#loader').show();
}

function hideLoader() {
    $('#loader').hide();
}

$(function () {
    // Fix for calendar and clock icons
    $('body').on('click', '.pointer', function () {
        $(this).prev().focus();
    });

    // Active links
    $('.sidebar a[href="' + window.location.pathname + '"]').parent('li').addClass('active');
    $('.sidebar a[href="' + window.location.pathname + '"]').addClass('active');

    // Remove parent menu items without children, which are not shown due to permissions
    $('.sidebar .nav-item.dropdown').each(function (idx, el) {
        var dropdownItems = $(el).find(".dropdown-menu > .dropdown-item");
        if (dropdownItems.length === 0) {
            $(el).remove();
        }
    });
});

function initializeFileSelect($selector, allowedFileExtensions) {
    var options = {
        language: 'en',
        browseBtnClass: 'btn btn-secondary'
    };

    if (allowedFileExtensions)
        options.allowedFileExtensions = allowedFileExtensions;

    $selector.fileselect(options);
    $selector.prev('.btn').html('<span class="fas fa-folder-open"></span> Browse');
    $selector.prev('.btn').addClass('btn-browse');
}

function initializeDatepicker($selector) {
    $selector.datepicker({
        format: 'dd/mm/yyyy',
        assumeNearbyYear: true,
        maxViewMode: 'months',
        todayHighlight: true,
        autoclose: true,
        templates: {
            leftArrow: '<span class="fas fa-angle-double-left"></span>',
            rightArrow: '<span class="fas fa-angle-double-right"></span>'
        }
    });

    // Fix for icon
    $('.datepicker + .input-group-append').click(function () {
        $(this).prev().focus();
    });
}

function initializeTimepicker($selector) {
    $selector.timepicker({
        showMeridian: false,
        defaultTime: false,
        icons: {
            up: 'fa fa-chevron-up',
            down: 'fa fa-chevron-down'
        }
    });

    // Fix for icon
    $('.datepicker + .input-group-append').click(function () {
        $(this).prev().focus();
    });
}

function initializeDateTimePicker($selector) {
    //Switching to font-awesome 5 icons
    $.fn.datetimepicker.Constructor.Default = $.extend({}, $.fn.datetimepicker.Constructor.Default, {
        icons: {
            time: 'far fa-clock',
            date: 'far fa-calendar',
            up: 'fas fa-arrow-up',
            down: 'fas fa-arrow-down',
            previous: 'fas fa-chevron-left',
            next: 'fas fa-chevron-right',
            today: 'fas fa-calendar-check',
            clear: 'far fa-trash-alt',
            close: 'far fa-times-circle'
        }
    });
    $selector.datetimepicker({
        format: 'DD/MM/YYYY HH:mm'
    });
}

function initializeRichTextEditor($selector, options) {
    var defaultOptions = {
        height: 300,
        toolbar: [
            ['style', ['style', 'clear']],
            ['formatting', ['bold', 'italic', 'underline', 'strikethrough']],
            ['script', ['superscript', 'subscript']],
            ['font', ['fontname', 'fontsize', 'color']],
            ['paragraph', ['ol', 'ul', 'paragraph']],
            ['insert', ['picture', 'link', 'table', 'hr']],
            ['history', ['undo', 'redo']],
            ['misc', ['codeview', 'fullscreen']]
        ]
    };

    if (options)
        options = $.extend({}, defaultOptions, options);
    else
        options = defaultOptions;

    $selector.summernote(options);

    // Bug fix for summer note
    $('.btn-fullscreen').on('click', function () {
        $('.note-toolbar-wrapper').hide();
    });
}


var originalTableHeights = {};
var defaultDataTableOptions = {
    classes: 'table table-hover table-sm',
    sortName: 'id',
    sortOrder: 'asc',
    iconsPrefix: 'fas',
    icons: {
        paginationSwitchDown: 'fa-collapse-down',
        paginationSwitchUp: 'fa-collapse-up',
        refresh: 'fa-sync-alt',
        toggle: 'fa-list-alt',
        columns: 'fa-th',
        detailOpen: 'fa-plus',
        detailClose: 'fa-minus'
    },
    pagination: true,
    sidePagination: 'server',
    showColumns: true,
    showRefresh: true,
    toolbar: '#toolbar',
    buttonsClass: 'primary',
    undefinedText: '',
    uniqueId: 'id',
    cookie: true,
    cookieExpire: '1y',
    cookiesEnabled: ['bs.table.sortOrder', 'bs.table.sortName', 'bs.table.columns'],
    escape: true
}

$(function () {
    $('#confirmDelete').on('show.bs.modal', function (e) {
        var data = $(e.relatedTarget).data();
        var deleteUrl;
        var index = data.deleteUrl.indexOf('?');
        if (index > -1)
            deleteUrl =
                data.deleteUrl.substring(0, index) + '/' + data.id + data.deleteUrl.substring(index);
        else
            deleteUrl = data.deleteUrl + '/' + data.id;

        $('#deleteForm').attr('action', deleteUrl);
    });
});

function initializeDataTable(selector, containerSelector, options) {
    if (typeof options !== 'undefined' && options) {
        var mergedOptions = $.extend({}, defaultDataTableOptions, options);
        $(selector).bootstrapTable(mergedOptions);
    }
    else
        $(selector).bootstrapTable(defaultDataTableOptions);

    originalTableHeights[selector] = $(selector).height();

    var tableSelector;
    if (containerSelector)
        tableSelector = containerSelector + ' ' + '.bootstrap-table ';
    else
        tableSelector = '.bootstrap-table ';

    $(tableSelector + '.dropdown-toggle').click(function () {
        var isOpenedColumnsList = $(this).closest('.keep-open').hasClass('show');

        if (isOpenedColumnsList)
            $(this).closest('.bootstrap-table').css('min-height', originalTableHeights[selector]);
        else
            $(this).closest('.bootstrap-table').css('min-height', $('.columns .dropdown-menu').height() + 50);
    });
}


// Initialize XML code editor for schemas
function initializeCodeEditor(id) {
    var element = document.getElementById(id);

    if (element) {
        return CodeMirror.fromTextArea(element, {
            lineNumbers: true,
            lineWrapping: true,
            viewportMargin: Infinity,
            matchTags: { bothTags: true },
            autoCloseTags: true
        });
    }

    return null;
}

function initializeNumericInput(selector, options) {
    var defaultOptions = {
        verticalbuttons: true,
        verticalup: '<span class="fas fa-chevron-up"><span>',
        verticaldown: '<span class="fas fa-chevron-down"><span>',
        buttondown_class: 'btn btn-secondary',
        buttonup_class: 'btn btn-secondary'
    };

    if (typeof options !== 'undefined' && options)
        var defaultOptions = $.extend({}, defaultOptions, options);

    $(function () {
        selector.TouchSpin(defaultOptions);
    });
}


// Formatters for Bootstrap-table
function getManageColumnTemplate(entity, template, tableElement, editText, deleteText) {

    if (!editText)
        editText = 'Edit';
    if (!deleteText)
        deleteText = 'Delete';

    var options;
    // From the parameter
    if (tableElement)
        options = tableElement.bootstrapTable('getOptions');
    // dataTableOptions variable
    else if (typeof dataTableOptions !== 'undefined' && dataTableOptions)
        options = $.extend({}, defaultDataTableOptions, dataTableOptions);
    // Default
    else
        options = defaultDataTableOptions;

    var moreText = 'More';
    if (options.moreButtonText) {
        moreText = options.moreButtonText;
    }

    if (options.moreButtonText) {
        editText = options.editButtonText;
    }

    if (options.moreButtonText) {
        deleteText = options.deleteButtonText;
    }

    var editUrl;
    var index = options.editUrl.indexOf('?');
    var id = options.uniqueId ? entity[options.uniqueId] : entity.Id;

    if (index > -1)
        editUrl =
            options.editUrl.substring(0, index) + '/' + id + options.editUrl.substring(index);
    else
        editUrl = options.editUrl + '/' + id;

    var result = '<div class="btn-group">';

    if (template)
        result +=
            '<div class="dropdown" id="moreButton"><button type="button" class="btn btn-sm btn-info dropdown-toggle" data-bs-toggle="dropdown"><span class="fas fa-ellipsis-v"></span> ' + moreText + '</button><div class="dropdown-menu">' + template + '</div></div>';

    result += '<a href="' + editUrl + '" class="btn btn-sm btn-secondary"><span class="fas fa-edit"></span> ' + editText + '</a> <button type="button" class="btn btn-sm btn-danger" data-id="' + entity[options.uniqueId] + '" data-delete-url="' + options.deleteUrl + '" data-toggle="modal" data-target="#confirmDelete"><span class="fas fa-trash"></span> ' + deleteText + '</button>';


    result += '</div>';

    return result;
}



function manageColumnFormatter(value, entity) {
    return getManageColumnTemplate(entity);
}

function booleanFormatter(value) {
    if (value)
        return '<span class="fas fa-fw fa-check-circle text-success"></span>';

    return '<span class="fas fa-fw fa-times-circle text-danger"></span>';
}

function fileSizeFormatter(value) {
    if (value === 0)
        return '0.00 B';

    if (!value)
        return value;

    var thresh = 1024;

    if (Math.abs(value) < thresh)
        return value + '.00 B';

    var units = ['KB', 'MB', 'GB'];
    var u = -1;

    do {
        value /= thresh;
        ++u;
    } while (Math.abs(value) >= thresh && u < units.length - 1);

    return value.toFixed(2) + ' ' + units[u];
}

function inputFormatter(value, entity) {
    var $result = $('<input />');
    $result.addClass('form-control-sm form-control');
    if (entity.hasError)
        $result.addClass('is-invalid');
    $result.attr('name', entity.fullPath);
    $result.attr('value', value);
    return $result.get(0).outerHTML;
}

function errorFormatter(value) {
    if (value > 0)
        return {
            classes: 'bg-danger'
        };

    return {
        classes: 'bg-success'
    };
}

function showSuccessMessage() {
    var $container = $('#success-message');
    $container.empty();
    $('#error-message').empty();
    $container.html('<div class="alert alert-success alert-dismissable"><button type="button" class="close" data-dismiss="alert"><span>&times;</span></button><p>Successful.</p></div>');
}

function showErrorMessage(message) {
    if (!message)
        message = 'Error happened.';

    var $container = $('#error-message');
    $container.empty();
    $('#success-message').empty();
    $container.html('<div class="alert alert-danger alert-dismissable"><button type="button" class="close" data-dismiss="alert"><span>&times;</span></button><p><strong>Error!</strong></p><p>' + message + '</p></div>');
}

function clearErrorMessage() {
    $('#error-message').empty();
}

function generateGuid() { // Public Domain/MIT
    var d = new Date().getTime();//Timestamp
    var d2 = ((typeof performance !== 'undefined') && performance.now && (performance.now() * 1000)) || 0;//Time in microseconds since page-load or 0 if unsupported
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16;//random number between 0 and 16
        if (d > 0) {//Use timestamp until depleted
            r = (d + r) % 16 | 0;
            d = Math.floor(d / 16);
        } else {//Use microseconds since page-load if supported
            r = (d2 + r) % 16 | 0;
            d2 = Math.floor(d2 / 16);
        }
        return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
}

jQuery.extend(jQuery.fn, {
    hasParent: function (p) {
        return this.filter(function () {
            return $(p).find(this).length;
        });
    }
});

$(function () {
    $('#sidebarToggle').click(function () {
        if ($('body').hasClass('sidebar-toggled') && $('.sidebar').hasClass('toggled')) {
            var currentDate = new Date();
            var expireDate = new Date(currentDate.getFullYear() + 1, currentDate.getMonth() + 1);

            document.cookie = ['isSiteBarToggled', '=', encodeURIComponent(true), '; expires=', expireDate.toGMTString(), '; path=/'].join('');
        }
        else {
            document.cookie = ['isSiteBarToggled', '=', '; expires=Thu, 01 Jan 1970 00:00:00 GMT', '; path=/'].join('');
        }
    });
});