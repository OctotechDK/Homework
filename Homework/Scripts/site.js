$(function () {
    $("body").on("click", "#formSubmit",
        function (event) {
            event.preventDefault();
            var submission = { // FormSubmissionViewModel
                "FirstName": $("#formFirstName").val(),
                "SurName": $("#formSurName").val(),
                "Email": $("#formEmail").val(),
                "PhoneNumber": $("#formPhoneNumber").val(),
                "DateOfBirth": $("#formDateOfBirth").val(),
                "ProductSerialNumber": $("#formProductSerialNumber").val()
            }
            $.ajax({
                type: "POST",
                data: JSON.stringify(submission),
                url: "/api/formsubmissionapi/submit",
                contentType: "application/json",
                success: function (data) {
                    $('#form')[0].reset();
                    $("#formSuccess").text(data.Message);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $("#formError").text(errorThrown);
                }
            });
        });
});