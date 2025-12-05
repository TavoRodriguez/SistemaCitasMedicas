Imports System.Data.SqlClient

Public Class dbUsuarios

    Private ReadOnly dbHelper As New DatabaseHelper()

    Public Function Create(usuario As Usuarios) As String
        Try
            Dim sql As String =
                "INSERT INTO Usuarios (NombreUsuario, Contrasena, IdRol, Correo) " &
                "OUTPUT INSERTED.IdUsuario VALUES (@NombreUsuario, @Contrasena, @IdRol, @Correo)"

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@NombreUsuario", usuario.NombreUsuario),
                dbHelper.CreateParameter("@Contrasena", usuario.Contrasena),
                dbHelper.CreateParameter("@IdRol", usuario.IdRol),
                dbHelper.CreateParameter("@Correo", usuario.Correo)
            }

            Dim idGenerado As Integer = Convert.ToInt32(dbHelper.ExecuteScalar(sql, parametros))
            usuario.IdUsuario = idGenerado

            Return "Usuario creado correctamente"

        Catch ex As Exception
            Return "Error al crear usuario: " & ex.Message
        End Try
    End Function

    Public Function Update(usuario As Usuarios) As String
        Try
            Dim sql As String =
                "UPDATE Usuarios SET NombreUsuario=@NombreUsuario, Contrasena=@Contrasena, IdRol=@IdRol, Correo=@Correo " &
                "WHERE IdUsuario=@IdUsuario"

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@NombreUsuario", usuario.NombreUsuario),
                dbHelper.CreateParameter("@Contrasena", usuario.Contrasena),
                dbHelper.CreateParameter("@IdRol", usuario.IdRol),
                dbHelper.CreateParameter("@Correo", usuario.Correo),
                dbHelper.CreateParameter("@IdUsuario", usuario.IdUsuario)
            }

            dbHelper.ExecuteNonQuery(sql, parametros)

            Return "Usuario actualizado correctamente"

        Catch ex As Exception
            Return "Error al actualizar usuario: " & ex.Message
        End Try
    End Function

    Public Function Delete(idUsuario As Integer) As String
        Try
            Dim sql As String = "DELETE FROM Usuarios WHERE IdUsuario=@IdUsuario"

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@IdUsuario", idUsuario)
            }

            dbHelper.ExecuteNonQuery(sql, parametros)

            Return "Usuario eliminado correctamente"

        Catch ex As Exception
            Return "Error al eliminar usuario: " & ex.Message
        End Try
    End Function

    Public Function GetByIdUsuarioPorNombre(nombreUsuario As String) As Usuarios
        Dim usuario As New Usuarios()

        Try
            Dim sql As String =
                "SELECT IdUsuario, NombreUsuario, Contrasena, IdRol, Correo 
                 FROM Usuarios WHERE NombreUsuario = @NombreUsuario"

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@NombreUsuario", nombreUsuario)
            }

            Dim dt As DataTable = dbHelper.ExecuteQuery(sql, parametros)

            If dt.Rows.Count > 0 Then
                Dim row = dt.Rows(0)
                usuario.IdUsuario = Convert.ToInt32(row("IdUsuario"))
                usuario.NombreUsuario = row("NombreUsuario").ToString()
                usuario.Contrasena = row("Contrasena").ToString()
                usuario.IdRol = Convert.ToInt32(row("IdRol"))
                usuario.Correo = row("Correo").ToString()
            End If

            Return usuario

        Catch ex As Exception
            Throw New Exception("Error al obtener usuario: " & ex.Message)
        End Try
    End Function

    Public Function GetById(idUsuario As Integer) As Usuarios
        Dim usuario As New Usuarios()

        Try
            Dim sql As String =
                "SELECT IdUsuario, NombreUsuario, Contrasena, IdRol, Correo 
                 FROM Usuarios WHERE IdUsuario = @IdUsuario"

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@IdUsuario", idUsuario)
            }

            Dim dt As DataTable = dbHelper.ExecuteQuery(sql, parametros)

            If dt.Rows.Count > 0 Then
                Dim row = dt.Rows(0)
                usuario.IdUsuario = Convert.ToInt32(row("IdUsuario"))
                usuario.NombreUsuario = row("NombreUsuario").ToString()
                usuario.Contrasena = row("Contrasena").ToString()
                usuario.IdRol = Convert.ToInt32(row("IdRol"))
                usuario.Correo = row("Correo").ToString()
            End If

            Return usuario

        Catch ex As Exception
            Throw New Exception("Error al obtener usuario: " & ex.Message)
        End Try
    End Function

    Public Function AutenticarUsuario(nombreUsuario As String, contrasenaEncriptada As String) As Usuarios
        Dim usuario As New Usuarios()

        Try
            Dim sql As String =
                "SELECT IdUsuario, NombreUsuario, Contrasena, IdRol, Correo
                 FROM Usuarios
                 WHERE NombreUsuario=@NombreUsuario AND Contrasena=@Contrasena"

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@NombreUsuario", nombreUsuario),
                dbHelper.CreateParameter("@Contrasena", contrasenaEncriptada)
            }

            Dim dt As DataTable = dbHelper.ExecuteQuery(sql, parametros)

            If dt.Rows.Count > 0 Then
                Dim row = dt.Rows(0)
                usuario.IdUsuario = Convert.ToInt32(row("IdUsuario"))
                usuario.NombreUsuario = row("NombreUsuario").ToString()
                usuario.Contrasena = row("Contrasena").ToString()
                usuario.IdRol = Convert.ToInt32(row("IdRol"))
                usuario.Correo = row("Correo").ToString()
            End If

            Return usuario

        Catch ex As Exception
            Throw New Exception("Error al autenticar usuario: " & ex.Message)
        End Try
    End Function

End Class



