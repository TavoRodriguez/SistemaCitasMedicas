Imports SistemaCitasMedicas.Utils
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
            Dim dbEspecialidad As New dbEspecialidad()
            ddlEspecialidadModal.DataSource = dbEspecialidad.GetEspecialidades()
            ddlEspecialidadModal.DataTextField = "NombreEspecialidad"
            ddlEspecialidadModal.DataValueField = "IdEspecialidad"
            ddlEspecialidadModal.DataBind()
            ddlEspecialidadModal.Items.Insert(0, New ListItem("--Seleccione Especialidad--", "0"))
        Catch ex As Exception
            ShowSwalMessage(Me, "Error", "Error al cargar las especialidades.", "error")
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
            ShowSwalMessage(Me, "Error", "Error al cargar el filtro de especialidades.", "error")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' Validaciones 
        If txtNombre.Text = "" Or txtApellido1.Text = "" Or ddlEspecialidadModal.SelectedValue = "0" Or txtTelefono.Text = "" Or txtCorreo.Text = "" Then
            ShowSwalMessage(Me, "Error", "Por favor, complete todos los campos requeridos.", "error")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
            Return
        End If

        If Not txtCorreo.Text.Contains("@") OrElse Not txtCorreo.Text.Contains(".") Then
            ShowSwalMessage(Me, "Error", "El correo electrónico no es válido.", "error")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
            Return
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
            If editando.Value = "0" Then
                dbDoctor.Create(doctor)
                ShowSwalMessage(Me, "Éxito", "Doctor agregado correctamente.", "success")
            Else
                doctor.IdDoctor = Convert.ToInt32(editando.Value)
                dbDoctor.Update(doctor)
                ShowSwalMessage(Me, "Actualizado", "Datos del doctor actualizados correctamente.", "success")
            End If

            gvDoctores.DataBind()
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
        ddlEspecialidadModal.SelectedIndex = 0
        txtTelefono.Text = ""
        txtCorreo.Text = ""
        editando.Value = "0"
    End Sub



    Protected Sub gvDoctores_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        e.Cancel = True

        Dim idDoctor As Integer = Convert.ToInt32(gvDoctores.DataKeys(e.RowIndex).Value)
        Dim dbCitas As New dbCitas()
        Dim dbDoctores As New dbDoctores()

        'Verificar si el doctor tiene citas
        If dbCitas.CitasDoctores(idDoctor) Then
            ShowSwalMessage(Me, "Atención", "El doctor tiene citas asignadas y no se puede eliminar.", "warning")
            Exit Sub
        End If

        Dim resultado As String = dbDoctores.Delete(idDoctor)
        If resultado.ToLower().Contains("error") Then
            ShowSwalMessage(Me, "Error", resultado, "error")
        Else
            ShowSwalMessage(Me, "Eliminado", "El doctor fue eliminado correctamente.", "success")
        End If

        gvDoctores.DataBind()
    End Sub

    Protected Sub gvDoctores_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "EditarDoctor" Then
            Dim idDoctor As Integer = Convert.ToInt32(e.CommandArgument)
            Dim db As New dbDoctores()
            Dim doctor As Doctores = db.GetById(idDoctor)

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

    Protected Sub gvDoctores_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvDoctores.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim btnEliminar As LinkButton = CType(e.Row.FindControl("btnEliminar"), LinkButton)

            If btnEliminar IsNot Nothing Then
                ShowSwalConfirmDelete(
                    page:=Me,
                    serverUniqueId:=btnEliminar.UniqueID,
                    clientId:=btnEliminar.ClientID,
                    confirmMessage:="¿Está seguro de eliminar este doctor?"
                )
            End If

        End If
    End Sub

End Class
