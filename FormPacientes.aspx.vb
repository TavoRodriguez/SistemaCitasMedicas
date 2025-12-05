Imports SistemaCitasMedicas.Utils

Public Class FormPacientes
    Inherits System.Web.UI.Page

    Public Paciente As Pacientes
    Protected dbPaciente As New dbPacientes()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

        ' Validar fecha de nacimiento
        Dim fechaNacimiento As Date
        If Not Date.TryParse(txtFechaNacimiento.Text.Trim(), fechaNacimiento) Then
            ShowSwalMessage(Me, "Error", "La fecha de nacimiento es inválida.", "error")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
            Return
        End If

        Dim paciente As New Pacientes() With {
        .Nombre = txtNombre.Text.Trim(),
        .Apellido1 = txtApellido1.Text.Trim(),
        .Apellido2 = txtApellido2.Text.Trim(),
        .Identificacion = txtIdentificacion.Text.Trim(),
        .FechaNacimiento = fechaNacimiento,
        .Telefono = txtTelefono.Text.Trim(),
        .Correo = txtCorreo.Text.Trim()
    }

        Try
            If editando.Value = "0" Then
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
        editando.Value = "0"
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
            editando.Value = idPaciente.ToString()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
        End If
    End Sub

    Protected Sub gvPacientes_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        e.Cancel = True
        Dim idPaciente As Integer = Convert.ToInt32(gvPacientes.DataKeys(e.RowIndex).Value)
        Dim dbCitas As New dbCitas()

        If dbCitas.CitasPacientes(idPaciente) Then
            ShowSwalMessage(Me, "Atención", "El paciente tiene citas asignadas y no se puede eliminar.", "warning")
            Exit Sub
        End If

        Dim resultado As String = dbPaciente.Delete(idPaciente)
        If resultado.ToLower().Contains("error") Then
            ShowSwalMessage(Me, "Error", resultado, "error")
        Else
            ShowSwalMessage(Me, "Eliminado", "El paciente fue eliminado correctamente.", "success")
            gvPacientes.DataBind()
        End If
    End Sub

    Protected Sub gvPacientes_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPacientes.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim btnEliminar As LinkButton = CType(e.Row.FindControl("btnEliminar"), LinkButton)
            If btnEliminar IsNot Nothing Then
                ShowSwalConfirmDelete(
                    page:=Me,
                    serverUniqueId:=btnEliminar.UniqueID,
                    clientId:=btnEliminar.ClientID,
                    confirmMessage:="¿Está seguro de eliminar este paciente?"
                )
            End If
        End If
    End Sub

End Class


