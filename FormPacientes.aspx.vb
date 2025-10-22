Public Class FormPacientes
    Inherits System.Web.UI.Page

    Public Pacientes As Pacientes
    Protected dbPaciente As New dbPacientes()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click

    End Sub

    Private Sub LimpiarCampos()
        txtNombre.Text = ""
        txtApellido1.Text = ""
        txtApellido2.Text = ""
        txtIdentificacion.Text = ""
        txtFechaNacimiento.Text = ""
        txtTelefono.Text = ""
        txtCorreo.Text = ""
    End Sub

    Protected Sub gvPacientes_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

    End Sub

    Protected Sub gvPacientes_RowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub
End Class
