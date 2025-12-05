Imports SistemaCitasMedicas.Utils
Imports System.Security.Cryptography
Imports System.Text
Imports System.Globalization

Public Class FormUsuarios
    Inherits System.Web.UI.Page

    Public Usuario As Usuarios
    Protected dbUsuario As New dbUsuarios()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarRoles()
            CargarRolesFiltro()
        End If
    End Sub

    Private Sub CargarRoles()
        Try
            Dim dbRol As New dbRoles()
            ddlRol.DataSource = dbRol.GetRolesTodos()
            ddlRol.DataTextField = "NombreRol"
            ddlRol.DataValueField = "IdRol"
            ddlRol.DataBind()
            ddlRol.Items.Insert(0, New ListItem("--Seleccione Rol--", "0"))
        Catch ex As Exception
            ShowSwalMessage(Me, "Error", "Error al cargar los roles.", "error")
        End Try
    End Sub

    Private Sub CargarRolesFiltro()
        Try
            Dim dbRolFiltro As New dbRoles()
            ddlRolFiltro.DataSource = dbRolFiltro.GetRolesTodos()
            ddlRolFiltro.DataTextField = "NombreRol"
            ddlRolFiltro.DataValueField = "IdRol"
            ddlRolFiltro.DataBind()
            ddlRolFiltro.Items.Insert(0, New ListItem("Todos", "0"))
        Catch ex As Exception
            ShowSwalMessage(Me, "Error", "Error al cargar el filtro de roles.", "error")
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' Validaciones
        If txtNombreUsuario.Text = "" Or ddlRol.SelectedValue = "0" Then
            ShowSwalMessage(Me, "Error", "Complete todos los campos requeridos.", "error")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
            Return
        End If

        Try
            Dim idUsuario As Integer = 0
            Dim contrasenaEncriptada As String = ""

            ' Crear usuario
            If editando.Value = "0" Then
                If txtContrasena.Text = "" Then
                    ShowSwalMessage(Me, "Error", "Ingrese una contraseña.", "error")
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
                    Return
                End If

                contrasenaEncriptada = EncriptarCon(txtContrasena.Text.Trim())

                Dim nuevoUsuario As New Usuarios() With {
                    .NombreUsuario = txtNombreUsuario.Text.Trim(),
                    .Contrasena = contrasenaEncriptada,
                    .IdRol = Convert.ToInt32(ddlRol.SelectedValue)
                }

                dbUsuario.Create(nuevoUsuario)
                idUsuario = dbUsuario.GetByIdUsuarioPorNombre(nuevoUsuario.NombreUsuario).IdUsuario

                ShowSwalMessage(Me, "Éxito", "Usuario creado correctamente.", "success")

            Else
                ' Editar usuario existente
                idUsuario = Convert.ToInt32(editando.Value)
                Dim usuarioExistente As Usuarios = dbUsuario.GetByIdUsuarioPorNombre(txtNombreUsuario.Text.Trim())

                ' Si se ingresó nueva contraseña, encriptarla
                If txtContrasena.Text.Trim() <> "" Then
                    contrasenaEncriptada = EncriptarCon(txtContrasena.Text.Trim())
                Else
                    contrasenaEncriptada = usuarioExistente.Contrasena
                End If

                Dim usuarioActualizar As New Usuarios() With {
                    .IdUsuario = idUsuario,
                    .NombreUsuario = txtNombreUsuario.Text.Trim(),
                    .Contrasena = contrasenaEncriptada,
                    .IdRol = Convert.ToInt32(ddlRol.SelectedValue)
                }

                dbUsuario.Update(usuarioActualizar)
                ShowSwalMessage(Me, "Actualizado", "Usuario actualizado correctamente.", "success")
            End If

            gvUsuarios.DataBind()
            LimpiarCampos()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "cerrarModal", "$('#modalAgregar').modal('hide');", True)

        Catch ex As Exception
            ShowSwalMessage(Me, "Error", "Error al guardar: " & ex.Message, "error")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
        End Try
    End Sub

    Private Sub LimpiarCampos()
        txtNombreUsuario.Text = ""
        txtContrasena.Text = ""
        ddlRol.SelectedIndex = 0
        editando.Value = "0"
    End Sub

    Protected Sub gvUsuarios_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "EditarUsuario" Then
            Dim idUsuario As Integer = Convert.ToInt32(e.CommandArgument)
            Dim usuario As Usuarios = dbUsuario.GetById(idUsuario)


            txtNombreUsuario.Text = usuario.NombreUsuario
            ddlRol.SelectedValue = usuario.IdRol
            txtContrasena.Text = ""
            editando.Value = usuario.IdUsuario.ToString()

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "abrirModal", "$('#modalAgregar').modal('show');", True)
        End If
    End Sub

    Protected Sub gvUsuarios_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        e.Cancel = True
        Try
            Dim idUsuario As Integer = Convert.ToInt32(gvUsuarios.DataKeys(e.RowIndex).Value)
            Dim resultado As String = dbUsuario.Delete(idUsuario)

            If resultado.ToLower().Contains("error") Then
                ShowSwalMessage(Me, "Error", resultado, "error")
            Else
                ShowSwalMessage(Me, "Eliminado", "Usuario eliminado correctamente.", "success")
                gvUsuarios.DataBind()
            End If
        Catch ex As Exception
            ShowSwalMessage(Me, "Error", "Error al eliminar el usuario: " & ex.Message, "error")
        End Try
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

    Protected Sub ddlRolFiltro_SelectedIndexChanged(sender As Object, e As EventArgs)
        gvUsuarios.DataBind()
    End Sub

    Protected Sub gvUsuarios_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvUsuarios.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim btnEliminar As LinkButton = CType(e.Row.FindControl("btnEliminar"), LinkButton)

            If btnEliminar IsNot Nothing Then
                ShowSwalConfirmDelete(
                    page:=Me,
                    serverUniqueId:=btnEliminar.UniqueID,
                    clientId:=btnEliminar.ClientID,
                    confirmMessage:="¿Está seguro de eliminar este usuario?"
                )
            End If

        End If
    End Sub

End Class
