Imports System.Data.SqlClient

Public Class dbCitas

    Private ReadOnly dbHelper As New DatabaseHelper()

    Public Function Create(cita As Citas) As String
        Try
            Dim sql As String = "
                INSERT INTO Citas (IdPaciente, IdDoctor, FechaCita, Estado, Observaciones)
                VALUES (@IdPaciente, @IdDoctor, @FechaCita, @Estado, @Observaciones)
            "

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@IdPaciente", cita.IdPaciente),
                dbHelper.CreateParameter("@IdDoctor", cita.IdDoctor),
                dbHelper.CreateParameter("@FechaCita", cita.FechaCita),
                dbHelper.CreateParameter("@Estado", cita.Estado),
                dbHelper.CreateParameter("@Observaciones", cita.Observaciones)
            }

            dbHelper.ExecuteNonQuery(sql, parametros)

            Return "Cita creada correctamente"

        Catch ex As Exception
            Return "Error al crear cita: " & ex.Message
        End Try
    End Function

    Public Function Update(cita As Citas) As String
        Try
            Dim sql As String = "
                UPDATE Citas SET 
                    IdPaciente=@IdPaciente, 
                    IdDoctor=@IdDoctor, 
                    FechaCita=@FechaCita, 
                    Estado=@Estado, 
                    Observaciones=@Observaciones
                WHERE IdCita=@IdCita
            "

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@IdPaciente", cita.IdPaciente),
                dbHelper.CreateParameter("@IdDoctor", cita.IdDoctor),
                dbHelper.CreateParameter("@FechaCita", cita.FechaCita),
                dbHelper.CreateParameter("@Estado", cita.Estado),
                dbHelper.CreateParameter("@Observaciones", cita.Observaciones),
                dbHelper.CreateParameter("@IdCita", cita.IdCita)
            }

            dbHelper.ExecuteNonQuery(sql, parametros)

            Return "Cita actualizada correctamente"

        Catch ex As Exception
            Return "Error al actualizar cita: " & ex.Message
        End Try
    End Function

    Public Function Delete(IdCita As Integer) As String
        Try
            Dim sql As String = "DELETE FROM Citas WHERE IdCita=@IdCita"

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@IdCita", IdCita)
            }

            dbHelper.ExecuteNonQuery(sql, parametros)

            Return "Cita eliminada correctamente"

        Catch ex As Exception
            Return "Error al eliminar cita: " & ex.Message
        End Try
    End Function

    Public Function GetById(IdCita As Integer) As Citas
        Try
            Dim sql As String = "
                SELECT IdCita, IdPaciente, IdDoctor, FechaCita, Estado, Observaciones
                FROM Citas 
                WHERE IdCita = @IdCita
            "

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@IdCita", IdCita)
            }

            Dim dt As DataTable = dbHelper.ExecuteQuery(sql, parametros)

            If dt.Rows.Count = 0 Then Return Nothing

            Dim row = dt.Rows(0)
            Dim cita As New Citas With {
                .IdCita = Convert.ToInt32(row("IdCita")),
                .IdPaciente = Convert.ToInt32(row("IdPaciente")),
                .IdDoctor = Convert.ToInt32(row("IdDoctor")),
                .FechaCita = Convert.ToDateTime(row("FechaCita")),
                .Estado = row("Estado").ToString(),
                .Observaciones = row("Observaciones").ToString()
            }

            Return cita

        Catch ex As Exception
            Throw New Exception("Error al obtener cita: " & ex.Message)
        End Try
    End Function

    Public Function GetCitas(Optional estado As String = "") As DataTable
        Dim sql As String = "
            SELECT c.IdCita,
                   p.Nombre + ' ' + p.Apellido1 AS Paciente,
                   d.Nombre + ' ' + d.Apellido1 AS Doctor,
                   c.FechaCita, c.Estado, c.Observaciones
            FROM Citas c
            INNER JOIN Pacientes p ON c.IdPaciente = p.IdPaciente
            INNER JOIN Doctores d ON c.IdDoctor = d.IdDoctor
        "

        Dim parametros As New List(Of SqlParameter)

        If estado <> "" Then
            sql &= " WHERE c.Estado = @Estado"
            parametros.Add(dbHelper.CreateParameter("@Estado", estado))
        End If

        Return dbHelper.ExecuteQuery(sql, parametros)
    End Function

    Public Function CitasPacientes(IdPaciente As Integer) As Boolean
        Dim sql As String = "SELECT COUNT(*) AS Total FROM Citas WHERE IdPaciente=@IdPaciente"

        Dim parametros As New List(Of SqlParameter) From {
            dbHelper.CreateParameter("@IdPaciente", IdPaciente)
        }

        Dim dt As DataTable = dbHelper.ExecuteQuery(sql, parametros)

        Return Convert.ToInt32(dt.Rows(0)("Total")) > 0
    End Function

    Public Function CitasDoctores(IdDoctor As Integer) As Boolean
        Dim sql As String = "SELECT COUNT(*) AS Total FROM Citas WHERE IdDoctor=@IdDoctor"

        Dim parametros As New List(Of SqlParameter) From {
            dbHelper.CreateParameter("@IdDoctor", IdDoctor)
        }

        Dim dt As DataTable = dbHelper.ExecuteQuery(sql, parametros)

        Return Convert.ToInt32(dt.Rows(0)("Total")) > 0
    End Function

End Class


