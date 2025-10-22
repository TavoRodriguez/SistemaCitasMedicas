Imports System.Data.SqlClient

Public Class dbPacientes

    Public ReadOnly connectionString As String = ConfigurationManager.ConnectionStrings("CitasMedicasDBConnectionString2").ConnectionString

    Public Function Create(Paciente As Pacientes) As String
        Try
            Dim sql As String = "INSERT INTO Pacientes (Nombre, Apellido1, Apellido2, Identificacion, FechaNacimiento, Telefono, Correo, IdUsuario) " &
                                "VALUES (@Nombre, @Apellido1, @Apellido2, @Identificacion, @FechaNacimiento, @Telefono, @Correo, @IdUsuario)"

            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@Nombre", Paciente.Nombre),
                New SqlParameter("@Apellido1", Paciente.Apellido1),
                New SqlParameter("@Apellido2", Paciente.Apellido2),
                New SqlParameter("@Identificacion", Paciente.Identificacion),
                New SqlParameter("@FechaNacimiento", Paciente.FechaNacimiento),
                New SqlParameter("@Telefono", Paciente.Telefono),
                New SqlParameter("@Correo", Paciente.Correo),
                New SqlParameter("@IdUsuario", Paciente.IdUsuario)
            }

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddRange(parametros.ToArray())
                    connection.Open()
                    command.ExecuteNonQuery()
                End Using
            End Using

            Return "Paciente creado correctamente"
        Catch ex As Exception
            Return "Error al crear paciente: " & ex.Message
        End Try
    End Function

    Public Function Update(Paciente As Pacientes) As String
        Try
            Dim sql As String = "UPDATE Pacientes SET Nombre=@Nombre, Apellido1=@Apellido1, Apellido2=@Apellido2, " &
                                "Identificacion=@Identificacion, FechaNacimiento=@FechaNacimiento, Telefono=@Telefono, Correo=@Correo, IdUsuario=@IdUsuario " &
                                "WHERE IdPaciente=@IdPaciente"

            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@Nombre", Paciente.Nombre),
                New SqlParameter("@Apellido1", Paciente.Apellido1),
                New SqlParameter("@Apellido2", Paciente.Apellido2),
                New SqlParameter("@Identificacion", Paciente.Identificacion),
                New SqlParameter("@FechaNacimiento", Paciente.FechaNacimiento),
                New SqlParameter("@Telefono", Paciente.Telefono),
                New SqlParameter("@Correo", Paciente.Correo),
                New SqlParameter("@IdUsuario", Paciente.IdUsuario),
                New SqlParameter("@IdPaciente", Paciente.IdPaciente)
            }

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddRange(parametros.ToArray())
                    connection.Open()
                    command.ExecuteNonQuery()
                End Using
            End Using

            Return "Paciente actualizado correctamente"
        Catch ex As Exception
            Return "Error al actualizar paciente: " & ex.Message
        End Try
    End Function

    Public Function Delete(IdPaciente As Integer) As String
        Try
            Dim sql As String = "DELETE FROM Pacientes WHERE IdPaciente=@IdPaciente"

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddWithValue("@IdPaciente", IdPaciente)
                    connection.Open()
                    command.ExecuteNonQuery()
                End Using
            End Using

            Return "Paciente eliminado correctamente"
        Catch ex As Exception
            Return "Error al eliminar paciente: " & ex.Message
        End Try
    End Function

    Public Function GetById(IdPaciente As Integer) As Pacientes
        Dim paciente As New Pacientes()
        Try
            Dim sql As String = "SELECT IdPaciente, Nombre, Apellido1, Apellido2, Identificacion, FechaNacimiento, Telefono, Correo, IdUsuario " &
                                "FROM Pacientes WHERE IdPaciente = @IdPaciente"

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.Add(New SqlParameter("@IdPaciente", IdPaciente))
                    connection.Open()
                    Using reader As SqlDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            paciente.IdPaciente = Convert.ToInt32(reader("IdPaciente"))
                            paciente.Nombre = reader("Nombre").ToString()
                            paciente.Apellido1 = reader("Apellido1").ToString()
                            paciente.Apellido2 = reader("Apellido2").ToString()
                            paciente.Identificacion = reader("Identificacion").ToString()
                            paciente.FechaNacimiento = Convert.ToDateTime(reader("FechaNacimiento"))
                            paciente.Telefono = reader("Telefono").ToString()
                            paciente.Correo = reader("Correo").ToString()
                            paciente.IdUsuario = Convert.ToInt32(reader("IdUsuario"))
                        End If
                    End Using
                End Using
            End Using

            Return paciente
        Catch ex As Exception
            Throw New Exception("Error al obtener paciente: " & ex.Message)
        End Try
    End Function

    Public Function GetPacientes() As DataTable
        Dim dt As New DataTable()
        Try
            Dim sql As String = "SELECT IdPaciente, Nombre + ' ' + Apellido1 + ' ' + Apellido2 AS NombreCompleto " &
                                "FROM Pacientes ORDER BY Nombre, Apellido1, Apellido2"

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
            Throw New Exception("Error al cargar pacientes: " & ex.Message)
        End Try
    End Function

End Class
