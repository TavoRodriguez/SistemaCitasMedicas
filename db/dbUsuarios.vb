Imports System.Data.SqlClient

Public Class dbUsuarios

    Public ReadOnly connectionString As String = ConfigurationManager.ConnectionStrings("CitasMedicasDBConnectionString2").ConnectionString

    Public Function Create(Usuario As Usuarios) As String
        Try
            Dim sql As String = "INSERT INTO Usuarios (NombreUsuario, Contrasena, IdRol) " &
                            "OUTPUT INSERTED.IdUsuario VALUES (@NombreUsuario, @Contrasena, @IdRol)"

            Dim parametros As New List(Of SqlParameter) From {
            New SqlParameter("@NombreUsuario", Usuario.NombreUsuario),
            New SqlParameter("@Contrasena", Usuario.Contrasena),
            New SqlParameter("@IdRol", Usuario.IdRol)
        }

            Dim idGenerado As Integer = 0

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddRange(parametros.ToArray())
                    connection.Open()
                    idGenerado = Convert.ToInt32(command.ExecuteScalar())
                End Using
            End Using

            ' Guardamos el Id generado dentro del objeto que recibimos
            Usuario.IdUsuario = idGenerado

            Return "Usuario creado correctamente"
        Catch ex As Exception
            Return "Error al crear usuario: " & ex.Message
        End Try
    End Function


    Public Function Update(Usuario As Usuarios) As String
        Try
            Dim sql As String = "UPDATE Usuarios SET NombreUsuario=@NombreUsuario, Contrasena=@Contrasena, IdRol=@IdRol " &
                                "WHERE IdUsuario=@IdUsuario"

            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@NombreUsuario", Usuario.NombreUsuario),
                New SqlParameter("@Contrasena", Usuario.Contrasena),
                New SqlParameter("@IdRol", Usuario.IdRol),
                New SqlParameter("@IdUsuario", Usuario.IdUsuario)
            }

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddRange(parametros.ToArray())
                    connection.Open()
                    command.ExecuteNonQuery()
                End Using
            End Using

            Return "Usuario actualizado correctamente"
        Catch ex As Exception
            Return "Error al actualizar usuario: " & ex.Message
        End Try
    End Function

    Public Function Delete(IdUsuario As Integer) As String
        Try
            Dim sql As String = "DELETE FROM Usuarios WHERE IdUsuario=@IdUsuario"

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddWithValue("@IdUsuario", IdUsuario)
                    connection.Open()
                    command.ExecuteNonQuery()
                End Using
            End Using

            Return "Usuario eliminado correctamente"
        Catch ex As Exception
            Return "Error al eliminar usuario: " & ex.Message
        End Try
    End Function

    Public Function GetByIdUsuarioPorNombre(NombreUsuario As String) As Usuarios
        Dim usuario As New Usuarios()
        Try
            Dim sql As String = "SELECT IdUsuario, NombreUsuario, Contrasena, IdRol FROM Usuarios WHERE NombreUsuario = @NombreUsuario"

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.Add(New SqlParameter("@NombreUsuario", NombreUsuario))
                    connection.Open()
                    Using reader As SqlDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            usuario.IdUsuario = Convert.ToInt32(reader("IdUsuario"))
                            usuario.NombreUsuario = reader("NombreUsuario").ToString()
                            usuario.Contrasena = reader("Contrasena").ToString()
                            usuario.IdRol = Convert.ToInt32(reader("IdRol"))
                        End If
                    End Using
                End Using
            End Using

            Return usuario
        Catch ex As Exception
            Throw New Exception("Error al obtener usuario: " & ex.Message)
        End Try
    End Function


    Public Function GetUsuarios() As DataTable
        Dim dt As New DataTable()
        Try
            Dim sql As String = "SELECT IdUsuario, NombreUsuario FROM Usuarios ORDER BY NombreUsuario"

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    connection.Open()
                    Using reader As SqlDataReader = command.ExecuteReader()
                        dt.Load(reader)
                    End Using
                End Using
            End Using

            Return dt
        Catch ex As Exception
            Throw New Exception("Error al cargar usuarios: " & ex.Message)
        End Try
    End Function

End Class

