Public Class SiteMaster
    Inherits MasterPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("UsuarioRol") IsNot Nothing Then
                Dim rol As Integer = Convert.ToInt32(Session("UsuarioRol"))

                ' todas las opciones ocultas por defecto
                liDoctores.Visible = False
                liPacientes.Visible = False
                liCitas.Visible = False
                liUsuarios.Visible = False

                Select Case rol
                    Case 1 ' Paciente
                        'Solo muestra pagina principal
                    Case 2 ' Administrador
                        liDoctores.Visible = True
                        liPacientes.Visible = True
                        liCitas.Visible = True
                        liUsuarios.Visible = True
                    Case 3 ' Secretaria
                        liPacientes.Visible = True
                        liCitas.Visible = True
                End Select
            Else
                ' No hay sesión activa, redirigir a login
                Response.Redirect("~/FormLogin.aspx")
            End If
        End If
        ' Evitar el uso del botón de retroceso después de cerrar sesión
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1))
        Response.Cache.SetNoStore()
    End Sub

    Protected Sub aCerrar_ServerClick1(sender As Object, e As EventArgs)
        ' Limpiar la sesión
        Session.Clear()
        Session.Abandon()
        Response.Cookies.Clear()
        Response.Redirect("~/FormLogin.aspx")
    End Sub
End Class