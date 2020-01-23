var requestVerificationToken = $('input[name="__RequestVerificationToken"]').val();

$(document).ready(function () {
    $('.toast').toast({
        animation: true,
        autohide: true,
        delay: 3000
    });
});

function processRules(url, accountId) {
    var data = {
        __RequestVerificationToken: requestVerificationToken
    };

    if (accountId) {
        data.accountId = accountId;
    }

    $.ajax({
        type: "POST",
        url: url,
        data: data,
        success: function(data, status) {
            $('#processRulesSuccess').toast('show');
        },
        error: function (data, status) {
            $('#processRulesError').toast('show');
        }
    });
}