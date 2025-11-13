
Public Class FormLogin
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        Try
            Dim dbUsuario As New dbUsuarios()
            Dim contrasenaEncriptada As String = EncriptarCon(txtContrasena.Text.Trim())
            Dim usuario As Usuarios = dbUsuario.AutenticarUsuario(txtUsuario.Text.Trim(), contrasenaEncriptada)

            If usuario IsNot Nothing AndAlso usuario.IdUsuario > 0 Then
                ' guardar datos en sesión
                Session("UsuarioId") = usuario.IdUsuario
                Session("UsuarioNombre") = usuario.NombreUsuario
                Session("UsuarioRol") = usuario.IdRol

                Response.Redirect("~/") ' Redirige a la página principal
            Else
                lblError.Text = "Usuario o contraseña incorrectos."
                lblError.Visible = True
            End If
        Catch ex As Exception
            lblError.Text = "Error al iniciar sesión: " & ex.Message
            lblError.Visible = True
        End Try
    End Sub
    Private Function EncriptarCon(texto As String) As String
        Using sha As System.Security.Cryptography.SHA256 = System.Security.Cryptography.SHA256.Create()
            Dim bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(texto)
            Dim hash As Byte() = sha.ComputeHash(bytes)
            Dim sb As New System.Text.StringBuilder()
            For Each b As Byte In hash
                sb.Append(b.ToString("x2"))
            Next
            Return sb.ToString()
        End Using
    End Function

End Class