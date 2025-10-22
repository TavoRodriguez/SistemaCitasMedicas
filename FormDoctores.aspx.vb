Public Class FormDoctores
    Inherits System.Web.UI.Page
    Public Doctor As Doctores
    Protected dbDoctor As New dbDoctores()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarEspecialidades()
            CargarEspecialidadesFiltro()
        End If
    End Sub

    Protected Sub ddlEspecialidad_SelectedIndexChanged(sender As Object, e As EventArgs)
        gvDoctores.DataBind()
    End Sub

    Private Sub CargarEspecialidades()
        Try
            Dim dbEsp As New dbEspecialidad()
            ddlEspecialidadModal.DataSource = dbEsp.GetEspecialidades()
            ddlEspecialidadModal.DataTextField = "NombreEspecialidad"
            ddlEspecialidadModal.DataValueField = "IdEspecialidad"
            ddlEspecialidadModal.DataBind()
            ddlEspecialidadModal.Items.Insert(0, New ListItem("--Seleccione Especialidad--", "0"))
        Catch ex As Exception

        End Try
    End Sub
    Private Sub CargarEspecialidadesFiltro()
        Try
            Dim dbEspFiltro As New dbEspecialidad()
            ddlEspecialidad.DataSource = dbEspFiltro.GetEspecialidades()
            ddlEspecialidad.DataTextField = "NombreEspecialidad"
            ddlEspecialidad.DataValueField = "IdEspecialidad"
            ddlEspecialidad.DataBind()
            ddlEspecialidad.Items.Insert(0, New ListItem("Todas", "0"))
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click

        Dim camposValidos As Boolean = True
        Dim mensajeError As String = ""

        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            camposValidos = False
            mensajeError &= "Debe ingresar el nombre.<br/>"
        End If

        If String.IsNullOrWhiteSpace(txtApellido1.Text) Then
            camposValidos = False
            mensajeError &= "Debe ingresar el primer apellido.<br/>"
        End If

        If ddlEspecialidadModal.SelectedValue = "0" Then
            camposValidos = False
            mensajeError &= "Debe seleccionar una especialidad.<br/>"
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

        If Not camposValidos Then
            lblErrorModal.Text = mensajeError
            lblErrorModal.Visible = True
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
            Exit Sub
        End If

        Dim doctor As New Doctores() With {
        .Nombre = txtNombre.Text.Trim(),
        .Apellido1 = txtApellido1.Text.Trim(),
        .Apellido2 = txtApellido2.Text.Trim(),
        .IdEspecialidad = Convert.ToInt32(ddlEspecialidadModal.SelectedValue),
        .Telefono = txtTelefono.Text.Trim(),
        .Correo = txtCorreo.Text.Trim()
    }

        Try
            If String.IsNullOrEmpty(editando.Value) Then
                dbDoctor.Create(doctor)
            Else
                doctor.IdDoctor = Convert.ToInt32(editando.Value)
                dbDoctor.Update(doctor)
            End If

            gvDoctores.DataBind()
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
        ddlEspecialidadModal.SelectedIndex = 0
        txtTelefono.Text = ""
        txtCorreo.Text = ""
    End Sub

    Protected Sub gvDoctores_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Try
            Dim idDoctor As Integer = Convert.ToInt32(gvDoctores.DataKeys(e.RowIndex).Value)
            Dim db As New dbDoctores()
            db.Delete(idDoctor)
            e.Cancel = True
            gvDoctores.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvDoctores_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "EditarDoctor" Then
            Dim idDoctor As Integer = Convert.ToInt32(e.CommandArgument)
            Dim db As New dbDoctores()
            Dim doctor As Doctores = db.GetById(idDoctor)

            ' Cargar datos en modal
            txtNombre.Text = doctor.Nombre
            txtApellido1.Text = doctor.Apellido1
            txtApellido2.Text = doctor.Apellido2
            ddlEspecialidadModal.SelectedValue = doctor.IdEspecialidad.ToString()
            txtTelefono.Text = doctor.Telefono
            txtCorreo.Text = doctor.Correo
            editando.Value = idDoctor.ToString()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
        End If
    End Sub

End Class