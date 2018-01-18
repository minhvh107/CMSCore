var loginController = function () {
    var login = function (user, pass) {
        $.ajax({
            type: 'POST',
            data: {
                UserName: user,
                Password: pass
            },
            dateType: 'json',
            url: '/admin/login/authen',
            success: function (res) {
                if (res.Success) {
                    window.location.href = "/Admin/Home/Index";
                } else {
                    cms.notify('Login Failed', 'error');
                }
            }
        });
    }

    var registerEvents = function () {
        //validate
        $("#frmLogin").validate({
            errorClass: "red",
            ignore: [],
            rules: {
                userName: {
                    required: true
                },
                password: {
                    required: true
                }
            }
        });

        // Login
        $('#btnLogin').on('click', function (e) {
            if ($('#frmLogin').valid()) {
                e.preventDefault();
                var user = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                login(user, password);
            }
        });
    }
    this.initialize = function () {
        registerEvents();
    }
}