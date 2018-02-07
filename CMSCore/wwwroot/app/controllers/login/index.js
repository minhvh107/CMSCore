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
    var loginSubmit = function () {
        if ($('#frmLogin').valid()) {
            var user = $('#txtUserName').val();
            var password = $('#txtPassword').val();
            login(user, password);
        }
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
        $("body").on("keypress", "#txtPassword,#txtUserName", function (e) {
            if (e.which === 13) {
                loginSubmit();
            }
        });

        // Login
        $('#btnLogin').on('click', function (e) {
            e.preventDefault();
            loginSubmit();
        });
    }
    this.initialize = function () {
        registerEvents();
    }
}