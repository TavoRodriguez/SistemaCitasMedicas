Imports System.Data.SqlClient

Public Class dbCitas

    Public ReadOnly connectionString As String = ConfigurationManager.ConnectionStrings("CitasMedicasDBConnectionString2").ConnectionString

    Public Function Create(Cita As Citas) As String
        Try
            Dim sql As String = "INSERT INTO Citas (IdPaciente, IdDoctor, FechaCita, Estado, Observaciones) " &
                                "VALUES (@IdPaciente, @IdDoctor, @FechaCita, @Estado, @Observaciones)"

            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@IdPaciente", Cita.IdPaciente),
                New SqlParameter("@IdDoctor", Cita.IdDoctor),
                New SqlParameter("@FechaCita", Cita.FechaCita),
                New SqlParameter("@Estado", Cita.Estado),
                New SqlParameter("@Observaciones", Cita.Observaciones)
            }

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddRange(parametros.ToArray())
                    connection.Open()
                    command.ExecuteNonQuery()
                End Using
            End Using

            Return "Cita creada correctamente"
        Catch ex As Exception
            Return "Error al crear cita: " & ex.Message
        End Try
    End Function

    Public Function Update(Cita As Citas) As String
        Try
            Dim sql As String = "UPDATE Citas SET IdPaciente=@IdPaciente, IdDoctor=@IdDoctor, FechaCita=@FechaCita, " &
                                "Estado=@Estado, Observaciones=@Observaciones " &
                                "WHERE IdCita=@IdCita"

            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@IdPaciente", Cita.IdPaciente),
                New SqlParameter("@IdDoctor", Cita.IdDoctor),
                New SqlParameter("@FechaCita", Cita.FechaCita),
                New SqlParameter("@Estado", Cita.Estado),
                New SqlParameter("@Observaciones", Cita.Observaciones),
                New SqlParameter("@IdCita", Cita.IdCita)
            }

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddRange(parametros.ToArray())
                    connection.Open()
                    command.ExecuteNonQuery()
                End Using
            End Using

            Return "Cita actualizada correctamente"
        Catch ex As Exception
            Return "Error al actualizar cita: " & ex.Message
        End Try
    End Function

    Public Function Delete(IdCita As Integer) As String
        Try
            Dim sql As String = "DELETE FROM Citas WHERE IdCita=@IdCita"

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.AddWithValue("@IdCita", IdCita)
                    connection.Open()
                    command.ExecuteNonQuery()
                End Using
            End Using

            Return "Cita eliminada correctamente"
        Catch ex As Exception
            Return "Error al eliminar cita: " & ex.Message
        End Try
    End Function
    Public Function GetById(IdCita As Integer) As Citas
        Dim cita As New Citas()
        Try
            Dim sql As String = "SELECT IdCita, IdPaciente, IdDoctor, FechaCita, Estado, Observaciones FROM Citas WHERE IdCita = @IdCita"

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    command.Parameters.Add(New SqlParameter("@IdCita", IdCita))
                    connection.Open()
                    Using reader As SqlDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            cita.IdCita = Convert.ToInt32(reader("IdCita"))
                            cita.IdPaciente = Convert.ToInt32(reader("IdPaciente"))
                            cita.IdDoctor = Convert.ToInt32(reader("IdDoctor"))
                            cita.FechaCita = Convert.ToDateTime(reader("FechaCita"))
                            cita.Estado = reader("Estado").ToString()
                            cita.Observaciones = reader("Observaciones").ToString()
                        End If
                    End Using
                End Using
            End Using

            Return cita
        Catch ex As Exception
            Throw New Exception("Error al obtener cita: " & ex.Message)
        End Try
    End Function
    Public Function GetCitas(Optional ByVal estado As String = "") As DataTable
        Dim dt As New DataTable()
        Using con As New SqlConnection(connectionString)
            Dim sql As String = "
                SELECT c.IdCita, 
                       p.Nombre + ' ' + p.Apellido1 AS Paciente,
                       d.Nombre + ' ' + d.Apellido1 AS Doctor,
                       c.FechaCita, c.Estado, c.Observaciones
                FROM Citas c
                INNER JOIN Pacientes p ON c.IdPaciente = p.IdPaciente
                INNER JOIN Doctores d ON c.IdDoctor = d.IdDoctor
            "
            If estado <> "" Then
                sql &= " WHERE c.Estado = @Estado"
            End If

            Using cmd As New SqlCommand(sql, con)
                If estado <> "" Then cmd.Parameters.AddWithValue("@Estado", estado)
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using
        Return dt
    End Function

    Public Function CitasPacientes(IdPaciente As Integer) As Boolean
        Dim sql As String = "SELECT COUNT(*) FROM Citas WHERE IdPaciente=@IdPaciente"
        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(sql, connection)
                command.Parameters.AddWithValue("@IdPaciente", IdPaciente)
                connection.Open()
                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function

    Public Function CitasDoctores(IdDoctor As Integer) As Boolean
        Dim sql As String = "SELECT COUNT(*) FROM Citas WHERE IdDoctor=@IdDoctor"
        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(sql, connection)
                command.Parameters.AddWithValue("@IdDoctor", IdDoctor)
                connection.Open()
                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function


End Class

