Imports System.Data.SqlClient

Public Class dbPacientes

    Private ReadOnly dbHelper As New DatabaseHelper()

    Public Function Create(paciente As Pacientes) As String
        Try
            Dim sql As String = "
                INSERT INTO Pacientes (Nombre, Apellido1, Apellido2, Identificacion, FechaNacimiento, Telefono, Correo)
                VALUES (@Nombre, @Apellido1, @Apellido2, @Identificacion, @FechaNacimiento, @Telefono, @Correo)
            "

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@Nombre", paciente.Nombre),
                dbHelper.CreateParameter("@Apellido1", paciente.Apellido1),
                dbHelper.CreateParameter("@Apellido2", paciente.Apellido2),
                dbHelper.CreateParameter("@Identificacion", paciente.Identificacion),
                dbHelper.CreateParameter("@FechaNacimiento", paciente.FechaNacimiento),
                dbHelper.CreateParameter("@Telefono", paciente.Telefono),
                dbHelper.CreateParameter("@Correo", paciente.Correo)
            }

            dbHelper.ExecuteNonQuery(sql, parametros)

            Return "Paciente creado correctamente"

        Catch ex As Exception
            Return "Error al crear paciente: " & ex.Message
        End Try
    End Function

    Public Function Update(paciente As Pacientes) As String
        Try
            Dim sql As String = "
                UPDATE Pacientes SET
                    Nombre=@Nombre,
                    Apellido1=@Apellido1,
                    Apellido2=@Apellido2,
                    Identificacion=@Identificacion,
                    FechaNacimiento=@FechaNacimiento,
                    Telefono=@Telefono,
                    Correo=@Correo
                WHERE IdPaciente=@IdPaciente
            "

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@Nombre", paciente.Nombre),
                dbHelper.CreateParameter("@Apellido1", paciente.Apellido1),
                dbHelper.CreateParameter("@Apellido2", paciente.Apellido2),
                dbHelper.CreateParameter("@Identificacion", paciente.Identificacion),
                dbHelper.CreateParameter("@FechaNacimiento", paciente.FechaNacimiento),
                dbHelper.CreateParameter("@Telefono", paciente.Telefono),
                dbHelper.CreateParameter("@Correo", paciente.Correo),
                dbHelper.CreateParameter("@IdPaciente", paciente.IdPaciente)
            }

            dbHelper.ExecuteNonQuery(sql, parametros)

            Return "Paciente actualizado correctamente"

        Catch ex As Exception
            Return "Error al actualizar paciente: " & ex.Message
        End Try
    End Function

    Public Function Delete(IdPaciente As Integer) As String
        Try
            Dim sql As String = "DELETE FROM Pacientes WHERE IdPaciente=@IdPaciente"

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@IdPaciente", IdPaciente)
            }

            dbHelper.ExecuteNonQuery(sql, parametros)

            Return "Paciente eliminado correctamente"

        Catch ex As Exception
            Return "Error al eliminar paciente: " & ex.Message
        End Try
    End Function

    Public Function GetById(IdPaciente As Integer) As Pacientes
        Try
            Dim sql As String = "
                SELECT IdPaciente, Nombre, Apellido1, Apellido2, Identificacion, FechaNacimiento, Telefono, Correo
                FROM Pacientes
                WHERE IdPaciente = @IdPaciente
            "

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@IdPaciente", IdPaciente)
            }

            Dim dt As DataTable = dbHelper.ExecuteQuery(sql, parametros)
            If dt.Rows.Count = 0 Then Return Nothing

            Dim row = dt.Rows(0)

            Dim paciente As New Pacientes With {
                .IdPaciente = Convert.ToInt32(row("IdPaciente")),
                .Nombre = row("Nombre").ToString(),
                .Apellido1 = row("Apellido1").ToString(),
                .Apellido2 = row("Apellido2").ToString(),
                .Identificacion = row("Identificacion").ToString(),
                .FechaNacimiento = Convert.ToDateTime(row("FechaNacimiento")),
                .Telefono = row("Telefono").ToString(),
                .Correo = row("Correo").ToString()
            }

            Return paciente

        Catch ex As Exception
            Throw New Exception("Error al obtener paciente: " & ex.Message)
        End Try
    End Function

    Public Function GetPacientes() As DataTable
        Try
            Dim sql As String = "
                SELECT IdPaciente, 
                       Nombre + ' ' + Apellido1 + ' ' + Apellido2 AS NombreCompleto
                FROM Pacientes
                ORDER BY Nombre, Apellido1, Apellido2
            "

            Return dbHelper.ExecuteQuery(sql)

        Catch ex As Exception
            Throw New Exception("Error al cargar pacientes: " & ex.Message)
        End Try
    End Function

End Class


