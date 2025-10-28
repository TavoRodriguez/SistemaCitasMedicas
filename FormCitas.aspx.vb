Imports SistemaCitasMedicas.Utils
Imports System.Text
Imports System.Data

Public Class FormCitas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarPacientes()
            CargarDoctores()
            CargarCitas()
        End If
    End Sub

    Private Sub CargarPacientes()
        Dim Paciente As New dbPacientes()
        ddlPaciente.DataSource = Paciente.GetPacientes()
        ddlPaciente.DataTextField = "NombreCompleto"
        ddlPaciente.DataValueField = "IdPaciente"
        ddlPaciente.DataBind()
        ddlPaciente.Items.Insert(0, New ListItem("--Seleccione Paciente--", "0"))
    End Sub

    Private Sub CargarDoctores()
        Dim Doctor As New dbDoctores()
        ddlDoctor.DataSource = Doctor.GetDoctores()
        ddlDoctor.DataTextField = "NombreCompleto"
        ddlDoctor.DataValueField = "IdDoctor"
        ddlDoctor.DataBind()
        ddlDoctor.Items.Insert(0, New ListItem("--Seleccione Doctor--", "0"))
    End Sub

    Private Sub CargarCitas(Optional ByVal estado As String = "")
        Dim dbCitas As New dbCitas()
        Dim citas As DataTable = dbCitas.GetCitas(estado)

        Dim html As New StringBuilder()

        For Each fila As DataRow In citas.Rows
            html.Append("<div class='col-md-4 mb-3'>")
            html.Append("  <div class='card shadow-sm border-0 rounded-3'>")
            html.Append("    <div class='card-body'>")
            html.Append("      <h5 class='card-title text-primary mb-2'>" & fila("Paciente") & "</h5>")
            html.Append("      <p class='card-text mb-1'><strong>Doctor:</strong> " & fila("Doctor") & "</p>")
            html.Append("      <p class='card-text mb-1'><strong>Fecha:</strong> " & CDate(fila("FechaCita")).ToString("dd/MM/yyyy HH:mm") & "</p>")
            html.Append("      <p class='card-text mb-1'><strong>Estado:</strong> <span class='badge bg-info'>" & fila("Estado") & "</span></p>")
            html.Append("      <p class='card-text'><strong>Observaciones:</strong> " & fila("Observaciones") & "</p>")
            html.Append("      <div class='d-flex justify-content-end gap-2 mt-2'>")
            html.Append("         <button class='btn btn-sm btn-warning'>Editar</button>")
            html.Append("         <button class='btn btn-sm btn-danger'>Eliminar</button>")
            html.Append("      </div>")
            html.Append("    </div>")
            html.Append("  </div>")
            html.Append("</div>")
        Next

        citasContainer.InnerHtml = html.ToString()
    End Sub

    Protected Sub ddlEstado_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim estadoSeleccionado As String = ddlEstado.SelectedValue
        CargarCitas(estadoSeleccionado)
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' Validaciones
        If ddlPaciente.SelectedValue = "0" Or ddlDoctor.SelectedValue = "0" Or String.IsNullOrWhiteSpace(txtFechaCita.Text) Or ddlEstadoModal.SelectedValue = "" Then
            ShowSwalMessage(Me, "Error", "Por favor, complete todos los campos requeridos.", "error")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
            Return
        End If

        Dim cita As New Citas() With {
            .IdPaciente = Convert.ToInt32(ddlPaciente.SelectedValue),
            .IdDoctor = Convert.ToInt32(ddlDoctor.SelectedValue),
            .FechaCita = Convert.ToDateTime(txtFechaCita.Text),
            .Estado = ddlEstadoModal.SelectedValue,
            .Observaciones = txtObservaciones.Text.Trim()
        }

        Try
            Dim dbCita As New dbCitas()

            If editando.Value = "0" Then
                dbCita.Create(cita)
                ShowSwalMessage(Me, "Éxito", "Cita agregada correctamente.", "success")
            Else
                cita.IdCita = Convert.ToInt32(editando.Value)
                dbCita.Update(cita)
                ShowSwalMessage(Me, "Actualizado", "Cita actualizada correctamente.", "success")
            End If

            CargarCitas()
            LimpiarCampos()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "cerrarModal", "$('#modalAgregar').modal('hide');", True)

        Catch ex As Exception
            ShowSwalMessage(Me, "Error", "Error al guardar la cita: " & ex.Message, "error")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
        End Try
    End Sub

    Private Sub LimpiarCampos()
        ddlPaciente.SelectedValue = "0"
        ddlDoctor.SelectedValue = "0"
        txtFechaCita.Text = ""
        ddlEstadoModal.SelectedValue = "Pendiente"
        txtObservaciones.Text = ""
        editando.Value = "0"
    End Sub

End Class
