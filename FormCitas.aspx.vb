Imports SistemaCitasMedicas.Utils
Imports System.Globalization

Public Class FormCitas
    Inherits System.Web.UI.Page

    Public Cita As Citas
    Protected dbCita As New dbCitas()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarPacientes()
            CargarDoctores()
        End If
    End Sub

    Protected Sub ddlEstado_SelectedIndexChanged(sender As Object, e As EventArgs)
        gvCitas.DataBind()
    End Sub

    Private Sub CargarPacientes()
        Try
            Dim dbPac As New dbPacientes()
            ddlPaciente.DataSource = dbPac.GetPacientes()
            ddlPaciente.DataTextField = "NombreCompleto"
            ddlPaciente.DataValueField = "IdPaciente"
            ddlPaciente.DataBind()
            ddlPaciente.Items.Insert(0, New ListItem("--Seleccione Paciente--", "0"))
        Catch ex As Exception
            ShowSwalMessage(Me, "Error", "Error al cargar los pacientes.", "error")
        End Try
    End Sub

    Private Sub CargarDoctores()
        Try
            Dim dbDoc As New dbDoctores()
            ddlDoctor.DataSource = dbDoc.GetDoctores()
            ddlDoctor.DataTextField = "NombreCompleto"
            ddlDoctor.DataValueField = "IdDoctor"
            ddlDoctor.DataBind()
            ddlDoctor.Items.Insert(0, New ListItem("--Seleccione Doctor--", "0"))
        Catch ex As Exception
            ShowSwalMessage(Me, "Error", "Error al cargar los doctores.", "error")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' Validaciones
        If ddlPaciente.SelectedValue = "0" Or ddlDoctor.SelectedValue = "0" Or txtFechaCita.Text = "" Or ddlEstadoModal.SelectedValue = "" Then
            ShowSwalMessage(Me, "Error", "Por favor, complete todos los campos requeridos.", "error")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
            Return
        End If

        ' Validar formato de fecha
        Dim fecha As DateTime
        Dim formatos() As String = {"yyyy-MM-ddTHH:mm", "yyyy-MM-ddTHH:mm:ss"}
        If Not DateTime.TryParseExact(txtFechaCita.Text.Trim(), formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, fecha) Then
            ShowSwalMessage(Me, "Error", "Formato de fecha no válido. Use el selector de fecha y hora.", "error")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
            Return
        End If

        Dim cita As New Citas() With {
            .IdPaciente = Convert.ToInt32(ddlPaciente.SelectedValue),
            .IdDoctor = Convert.ToInt32(ddlDoctor.SelectedValue),
            .FechaCita = fecha,
            .Estado = ddlEstadoModal.SelectedValue,
            .Observaciones = txtObservaciones.Text.Trim()
        }

        Try
            If editando.Value = "0" Then
                dbCita.Create(cita)
                ShowSwalMessage(Me, "Éxito", "Cita agregada correctamente.", "success")
            Else
                cita.IdCita = Convert.ToInt32(editando.Value)
                dbCita.Update(cita)
                ShowSwalMessage(Me, "Actualizado", "Datos de la cita actualizados correctamente.", "success")
            End If

            gvCitas.DataBind()
            LimpiarCampos()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "cerrarModal", "$('#modalAgregar').modal('hide');", True)

        Catch ex As Exception
            ShowSwalMessage(Me, "Error", "Error al guardar la cita: " & ex.Message, "error")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
        End Try
    End Sub

    Private Sub LimpiarCampos()
        ddlPaciente.SelectedIndex = 0
        ddlDoctor.SelectedIndex = 0
        txtFechaCita.Text = ""
        ddlEstadoModal.SelectedValue = "Pendiente"
        txtObservaciones.Text = ""
        editando.Value = "0"
    End Sub

    Protected Sub gvCitas_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        e.Cancel = True
        Dim idCita As Integer = Convert.ToInt32(e.Keys("IdCita"))

        Try
            Dim resultado As String = dbCita.Delete(idCita)
            If resultado.ToLower().Contains("error") Then
                ShowSwalMessage(Me, "Error", resultado, "error")
            Else
                ShowSwalMessage(Me, "Eliminado", "La cita fue eliminada correctamente.", "success")
            End If

            gvCitas.DataBind()

        Catch ex As Exception
            ShowSwalMessage(Me, "Error", "No se pudo eliminar la cita: " & ex.Message, "error")
        End Try
    End Sub

    Protected Sub gvCitas_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "EditarCita" Then
            Dim idCita As Integer = Convert.ToInt32(e.CommandArgument)
            Dim db As New dbCitas()
            Dim cita As Citas = db.GetById(idCita)

            ddlPaciente.SelectedValue = cita.IdPaciente.ToString()
            ddlDoctor.SelectedValue = cita.IdDoctor.ToString()
            txtFechaCita.Text = cita.FechaCita.ToString("yyyy-MM-ddTHH:mm")
            ddlEstadoModal.SelectedValue = cita.Estado
            txtObservaciones.Text = cita.Observaciones
            editando.Value = idCita.ToString()

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
        End If
    End Sub

    Protected Sub gvCitas_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim btnEliminar As LinkButton = CType(e.Row.FindControl("btnEliminar"), LinkButton)

            If btnEliminar IsNot Nothing Then
                ShowSwalConfirmDelete(
                    page:=Me,
                    serverUniqueId:=btnEliminar.UniqueID,
                    clientId:=btnEliminar.ClientID,
                    confirmMessage:="¿Está seguro de eliminar esta cita?"
                )
            End If

        End If
    End Sub




End Class
