Imports System.Data.SqlClient

Public Class dbDoctores

    Public ReadOnly connectionString As String = ConfigurationManager.ConnectionStrings("CitasMedicasDBConnectionString2").ConnectionString


    Public Function Create(Doctor As Doctores) As String
        Try
            Dim sql As String = "INSERT INTO Doctores (Nombre, Apellido1, Apellido2, IdEspecialidad, Telefono, Correo) " &
                                "VALUES (@Nombre, @Apellido1, @Apellido2, @IdEspecialidad, @Telefono, @Correo)"

            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@Nombre", Doctor.Nombre),
                New SqlParameter("@Apellido1", Doctor.Apellido1),
                New SqlParameter("@Apellido2", Doctor.Apellido2),
                New SqlParameter("@IdEspecialidad", Doctor.IdEspecialidad),
                New SqlParameter("@Telefono", Doctor.Telefono),
                New SqlParameter("@Correo", Doctor.Correo)
            }

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddRange(parametros.ToArray())
                    connection.Open()
                    command.ExecuteNonQuery()
                End Using
            End Using

            Return "Doctor creado correctamente"
        Catch ex As Exception
            Return "Error al crear doctor: " & ex.Message
        End Try
    End Function

    Public Function Update(Doctor As Doctores) As String
        Try
            Dim sql As String = "UPDATE Doctores SET Nombre=@Nombre, Apellido1=@Apellido1, Apellido2=@Apellido2, " &
                                "IdEspecialidad=@IdEspecialidad, Telefono=@Telefono, Correo=@Correo " &
                                "WHERE IdDoctor=@IdDoctor"

            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@Nombre", Doctor.Nombre),
                New SqlParameter("@Apellido1", Doctor.Apellido1),
                New SqlParameter("@Apellido2", Doctor.Apellido2),
                New SqlParameter("@IdEspecialidad", Doctor.IdEspecialidad),
                New SqlParameter("@Telefono", Doctor.Telefono),
                New SqlParameter("@Correo", Doctor.Correo),
                New SqlParameter("@IdDoctor", Doctor.IdDoctor)
            }

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddRange(parametros.ToArray())
                    connection.Open()
                    command.ExecuteNonQuery()
                End Using
            End Using

            Return "Doctor actualizado correctamente"
        Catch ex As Exception
            Return "Error al actualizar doctor: " & ex.Message
        End Try
    End Function

    Public Function Delete(IdDoctor As Integer) As String
        Try
            Dim sql As String = "DELETE FROM Doctores WHERE IdDoctor=@IdDoctor"

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddWithValue("@IdDoctor", IdDoctor)
                    connection.Open()
                    command.ExecuteNonQuery()
                End Using
            End Using

            Return "Doctor eliminado correctamente"
        Catch ex As Exception
            Return "Error al eliminar doctor: " & ex.Message
        End Try
    End Function

    Public Function GetById(IdDoctor As Integer) As Doctores
        Dim doctor As New Doctores()
        Try
            Dim sql As String = "SELECT IdDoctor, Nombre, Apellido1, Apellido2, IdEspecialidad, Telefono, Correo " &
                                "FROM Doctores WHERE IdDoctor = @IdDoctor"

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.Add(New SqlParameter("@IdDoctor", IdDoctor))
                    connection.Open()
                    Using reader As SqlDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            doctor.IdDoctor = Convert.ToInt32(reader("IdDoctor"))
                            doctor.Nombre = reader("Nombre").ToString()
                            doctor.Apellido1 = reader("Apellido1").ToString()
                            doctor.Apellido2 = reader("Apellido2").ToString()
                            doctor.IdEspecialidad = Convert.ToInt32(reader("IdEspecialidad"))
                            doctor.Telefono = reader("Telefono").ToString()
                            doctor.Correo = reader("Correo").ToString()
                        End If
                    End Using
                End Using
            End Using

            Return doctor
        Catch ex As Exception
            Throw New Exception("Error al obtener doctor: " & ex.Message)
        End Try
    End Function

    Public Function GetDoctores() As DataTable
        Dim dt As New DataTable()
        Try
            Dim sql As String = "SELECT IdDoctor, Nombre + ' ' + Apellido1 + ' ' + Apellido2 AS NombreCompleto " &
                                "FROM Doctores ORDER BY Nombre, Apellido1, Apellido2"

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
            Throw New Exception("Error al cargar doctores: " & ex.Message)
        End Try
    End Function

End Class

