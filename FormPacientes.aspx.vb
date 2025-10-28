Imports SistemaCitasMedicas.Utils
Imports System.Security.Cryptography
Imports System.Text

Public Class FormPacientes
    Inherits System.Web.UI.Page

    Public Paciente As Pacientes
    Protected dbPaciente As New dbPacientes()
    Protected dbUsuario As New dbUsuarios()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarRoles()
        End If
    End Sub

    Private Sub CargarRoles()
        Try
            Dim dbRol As New dbRoles()
            ddlRol.DataSource = dbRol.GetRoles()
            ddlRol.DataTextField = "NombreRol"
            ddlRol.DataValueField = "IdRol"
            ddlRol.DataBind()
            ddlRol.Items.Insert(0, New ListItem("--Seleccione Rol--", "0"))
        Catch ex As Exception
            ShowSwalMessage(Me, "Error", "Error al cargar los roles.", "error")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' Validaciones
        If txtNombre.Text = "" Or txtApellido1.Text = "" Or txtTelefono.Text = "" Or txtCorreo.Text = "" Then
            ShowSwalMessage(Me, "Error", "Por favor, complete todos los campos requeridos.", "error")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
            Return
        End If

        If Not txtCorreo.Text.Contains("@") OrElse Not txtCorreo.Text.Contains(".") Then
            ShowSwalMessage(Me, "Error", "El correo electrónico no es válido.", "error")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
            Return
        End If

        ' Validar usuario solo si estamos agregando
        If editando.Value = "0" Then
            If txtNombreUsuario.Text = "" Or txtContrasena.Text = "" Or ddlRol.SelectedValue = "0" Then
                ShowSwalMessage(Me, "Error", "Por favor, complete todos los campos de usuario.", "error")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
                Return
            End If
        End If

        Try
            Dim idUsuario As Integer = 0

            ' Crear usuario solo si editando.Value = "0"
            If editando.Value = "0" Then
                Dim usuario As New Usuarios() With {
                    .NombreUsuario = txtNombreUsuario.Text.Trim(),
                    .Contrasena = EncriptarCon(txtContrasena.Text.Trim()),
                    .IdRol = Convert.ToInt32(ddlRol.SelectedValue)
                }
                dbUsuario.Create(usuario)
                Dim usuarioCreado As Usuarios = dbUsuario.GetByIdUsuarioPorNombre(usuario.NombreUsuario)
                idUsuario = usuarioCreado.IdUsuario
            End If

            ' Crear o actualizar paciente
            Dim paciente As New Pacientes() With {
                .Nombre = txtNombre.Text.Trim(),
                .Apellido1 = txtApellido1.Text.Trim(),
                .Apellido2 = txtApellido2.Text.Trim(),
                .Identificacion = txtIdentificacion.Text.Trim(),
                .FechaNacimiento = Convert.ToDateTime(txtFechaNacimiento.Text.Trim()),
                .Telefono = txtTelefono.Text.Trim(),
                .Correo = txtCorreo.Text.Trim()
            }

            If editando.Value = "0" Then
                paciente.IdUsuario = idUsuario
                dbPaciente.Create(paciente)
                ShowSwalMessage(Me, "Éxito", "Paciente agregado correctamente.", "success")
            Else
                paciente.IdPaciente = Convert.ToInt32(editando.Value)
                dbPaciente.Update(paciente)
                ShowSwalMessage(Me, "Actualizado", "Datos del paciente actualizados correctamente.", "success")
            End If

            gvPacientes.DataBind()
            LimpiarCampos()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "cerrarModal", "$('#modalAgregar').modal('hide');", True)

        Catch ex As Exception
            ShowSwalMessage(Me, "Error", "Error al guardar: " & ex.Message, "error")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
        End Try
    End Sub

    Private Sub LimpiarCampos()
        txtNombre.Text = ""
        txtApellido1.Text = ""
        txtApellido2.Text = ""
        txtIdentificacion.Text = ""
        txtFechaNacimiento.Text = ""
        txtTelefono.Text = ""
        txtCorreo.Text = ""
        txtNombreUsuario.Text = ""
        txtContrasena.Text = ""
        ddlRol.SelectedIndex = 0
        editando.Value = "0"
        pnlCuentaUsuario.Visible = True
    End Sub

    Protected Sub gvPacientes_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "EditarPaciente" Then
            Dim idPaciente As Integer = Convert.ToInt32(e.CommandArgument)
            Dim paciente As Pacientes = dbPaciente.GetById(idPaciente)

            txtNombre.Text = paciente.Nombre
            txtApellido1.Text = paciente.Apellido1
            txtApellido2.Text = paciente.Apellido2
            txtIdentificacion.Text = paciente.Identificacion
            txtFechaNacimiento.Text = paciente.FechaNacimiento.ToString("yyyy-MM-dd")
            txtTelefono.Text = paciente.Telefono
            txtCorreo.Text = paciente.Correo

            pnlCuentaUsuario.Visible = False
            editando.Value = idPaciente.ToString()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
        Else
            pnlCuentaUsuario.Visible = True
        End If
    End Sub

    Protected Sub gvPacientes_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        e.Cancel = True
        Dim idPaciente As Integer = Convert.ToInt32(gvPacientes.DataKeys(e.RowIndex).Value)
        Dim dbCitas As New dbCitas()
        Dim dbPacientes As New dbPacientes()

        If dbCitas.CitasPacientes(idPaciente) Then
            ShowSwalMessage(Me, "Atención", "El paciente tiene citas asignadas y no se puede eliminar.", "warning")
            Exit Sub
        End If

        Dim resultado As String = dbPacientes.Delete(idPaciente)
        If resultado.ToLower().Contains("error") Then
            ShowSwalMessage(Me, "Error", resultado, "error")
        Else
            ShowSwalMessage(Me, "Eliminado", "El paciente fue eliminado correctamente.", "success")
            gvPacientes.DataBind()
        End If
    End Sub

    Private Function EncriptarCon(texto As String) As String
        Using sha As SHA256 = SHA256.Create()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(texto)
            Dim hash As Byte() = sha.ComputeHash(bytes)
            Dim sb As New StringBuilder()
            For Each b As Byte In hash
                sb.Append(b.ToString("x2"))
            Next
            Return sb.ToString()
        End Using
    End Function

End Class


