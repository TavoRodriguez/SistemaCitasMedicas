Imports System.Security.Cryptography
Imports System.Text

Public Class FormPacientes
    Inherits System.Web.UI.Page

    Public Pacientes As Pacientes
    Protected dbPaciente As New dbPacientes()
    Protected dbUsuario As New dbUsuarios()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarRoles()
        End If
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click

        Dim camposValidos As Boolean = True
        Dim mensajeError As String = ""

        ' Validaciones de los datos del paciente
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            camposValidos = False
            mensajeError &= "Debe ingresar el nombre.<br/>"
        End If

        If String.IsNullOrWhiteSpace(txtApellido1.Text) Then
            camposValidos = False
            mensajeError &= "Debe ingresar el primer apellido.<br/>"
        End If

        If String.IsNullOrWhiteSpace(txtTelefono.Text) Then
            camposValidos = False
            mensajeError &= "Debe ingresar el teléfono.<br/>"
        End If

        If String.IsNullOrWhiteSpace(txtCorreo.Text) Then
            camposValidos = False
            mensajeError &= "Debe ingresar el correo electrónico.<br/>"
        ElseIf Not txtCorreo.Text.Contains("@") OrElse Not txtCorreo.Text.Contains(".") Then
            camposValidos = False
            mensajeError &= "El correo electrónico no es válido.<br/>"
        End If

        ' Validaciones de los datos del usuario 
        If String.IsNullOrEmpty(editando.Value) Then
            If String.IsNullOrWhiteSpace(txtNombreUsuario.Text) Then
                camposValidos = False
                mensajeError &= "Debe ingresar el nombre de usuario.<br/>"
            End If

            If String.IsNullOrWhiteSpace(txtContrasena.Text) Then
                camposValidos = False
                mensajeError &= "Debe ingresar la contraseña.<br/>"
            End If

            If ddlRol.SelectedValue = "0" Then
                camposValidos = False
                mensajeError &= "Debe seleccionar un rol.<br/>"
            End If
        End If

        If Not camposValidos Then
            lblErrorModal.Text = mensajeError
            lblErrorModal.Visible = True
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
            Exit Sub
        End If

        Try
            Dim idUsuario As Integer = 0

            ' Se crea el usuario solo si se está agregando
            If String.IsNullOrEmpty(editando.Value) Then
                Dim usuario As New Usuarios() With {
                .NombreUsuario = txtNombreUsuario.Text.Trim(),
                .Contrasena = EncriptarCon(txtContrasena.Text.Trim()),
                .IdRol = Convert.ToInt32(ddlRol.SelectedValue)
            }
                dbUsuario.Create(usuario)

                ' Guardamos el ID del usuario creado
                Dim usuarioCreado As Usuarios = dbUsuario.GetByIdUsuarioPorNombre(usuario.NombreUsuario)
                idUsuario = usuarioCreado.IdUsuario
            End If

            ' Crear o actualizar pacientes
            Dim paciente As New Pacientes() With {
            .Nombre = txtNombre.Text.Trim(),
            .Apellido1 = txtApellido1.Text.Trim(),
            .Apellido2 = txtApellido2.Text.Trim(),
            .Identificacion = txtIdentificacion.Text.Trim(),
            .FechaNacimiento = Convert.ToDateTime(txtFechaNacimiento.Text.Trim()),
            .Telefono = txtTelefono.Text.Trim(),
            .Correo = txtCorreo.Text.Trim()
        }

            ' Asociar usuario solo si se está agregando
            If String.IsNullOrEmpty(editando.Value) Then
                paciente.IdUsuario = idUsuario
                dbPaciente.Create(paciente)
            Else
                paciente.IdPaciente = Convert.ToInt32(editando.Value)
                dbPaciente.Update(paciente)
            End If

            gvPacientes.DataBind()
            LimpiarCampos()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "cerrarModal", "$('#modalAgregar').modal('hide');", True)

        Catch ex As Exception
            lblErrorModal.Text = "Error al guardar: " & ex.Message
            lblErrorModal.Visible = True
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
        lblErrorModal.Visible = False
        editando.Value = ""
    End Sub

    Protected Sub gvPacientes_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "EditarPaciente" Then
            Dim idPaciente As Integer = Convert.ToInt32(e.CommandArgument)
            Dim paciente As Pacientes = dbPaciente.GetById(idPaciente)

            ' Cargar datos en modal
            txtNombre.Text = paciente.Nombre
            txtApellido1.Text = paciente.Apellido1
            txtApellido2.Text = paciente.Apellido2
            txtIdentificacion.Text = paciente.Identificacion
            txtFechaNacimiento.Text = paciente.FechaNacimiento.ToString("yyyy-MM-dd")
            txtTelefono.Text = paciente.Telefono
            txtCorreo.Text = paciente.Correo

            ' Ocultar campos de usuario al editar
            pnlCuentaUsuario.Visible = False

            editando.Value = idPaciente.ToString()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
        Else
            ' Mostrar campos de usuario
            pnlCuentaUsuario.Visible = True
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

    Private Sub CargarRoles()
        Try
            Dim dbRol As New dbRoles()
            ddlRol.DataSource = dbRol.GetRoles()
            ddlRol.DataTextField = "NombreRol"
            ddlRol.DataValueField = "IdRol"
            ddlRol.DataBind()
            ddlRol.Items.Insert(0, New ListItem("--Seleccione Rol--", "0"))
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub gvPacientes_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Try
            Dim idPaciente As Integer = Convert.ToInt32(gvPacientes.DataKeys(e.RowIndex).Value)
            Dim db As New dbPacientes()
            db.Delete(idPaciente)
            e.Cancel = True
            gvPacientes.DataBind()
        Catch ex As Exception
            lblErrorModal.Text = "Error al eliminar: " & ex.Message
            lblErrorModal.Visible = True
        End Try
    End Sub
End Class

