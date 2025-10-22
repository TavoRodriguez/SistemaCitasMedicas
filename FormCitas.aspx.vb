Public Class FormCitas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarPacientes()
            CargarDoctores()
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

    Protected Sub ddlEstado_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)

    End Sub
End Class