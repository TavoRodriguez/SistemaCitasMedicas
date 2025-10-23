<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FormLogin.aspx.vb" Inherits="SistemaCitasMedicas.FormLogin" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <title>Login</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        body {
            height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            background: linear-gradient(135deg, #6C63FF, #00C9A7);
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        .login-card {
            background: #fff;
            border-radius: 15px;
            box-shadow: 0 20px 40px rgba(0,0,0,0.2);
            width: 400px;
            padding: 40px;
            position: relative;
            overflow: hidden;
            transition: transform 0.3s;
        }

        .login-card:hover {
            transform: translateY(-5px);
        }

        .login-card h2 {
            color: #6C63FF;
            font-weight: 700;
            margin-bottom: 30px;
            text-align: center;
        }

        .form-control {
            border-radius: 50px;
            padding: 12px 20px;
            margin-bottom: 20px;
            border: 1px solid #ddd;
            transition: all 0.3s;
        }

        .form-control:focus {
            border-color: #6C63FF;
            box-shadow: 0 0 8px rgba(108, 99, 255, 0.3);
        }

        .btn-login {
            border-radius: 50px;
            padding: 12px;
            background: #6C63FF;
            border: none;
            color: #fff;
            font-weight: 600;
            transition: background 0.3s;
        }

        .btn-login:hover {
            background: #574fd6;
        }

        .error-label {
            color: #ff4d4f;
            font-weight: bold;
            margin-bottom: 15px;
            display: block;
            text-align: center;
        }

        .forgot-link {
            display: block;
            text-align: center;
            margin-top: 10px;
            text-decoration: none;
            color: #6C63FF;
            font-weight: 500;
        }

        .forgot-link:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-card">
            <h2>Iniciar Sesión</h2>

            <asp:Label ID="lblError" runat="server" CssClass="error-label" Visible="False"></asp:Label>

            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Usuario" />
            <asp:TextBox ID="txtContrasena" runat="server" CssClass="form-control" TextMode="Password" placeholder="Contraseña" />

            <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-login w-100" Text="Entrar" OnClick="btnLogin_Click" />

            <a href="#" class="forgot-link">¿Olvidaste tu contraseña?</a>
        </div>
    </form>
</body>
</html>
