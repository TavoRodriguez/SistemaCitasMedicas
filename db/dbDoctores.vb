Imports System.Data.SqlClient

Public Class dbDoctores

    Private ReadOnly dbHelper As New DatabaseHelper()

    Public Function Create(doctor As Doctores) As String
        Try
            Dim sql As String = "
                INSERT INTO Doctores (Nombre, Apellido1, Apellido2, IdEspecialidad, Telefono, Correo)
                VALUES (@Nombre, @Apellido1, @Apellido2, @IdEspecialidad, @Telefono, @Correo)
            "

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@Nombre", doctor.Nombre),
                dbHelper.CreateParameter("@Apellido1", doctor.Apellido1),
                dbHelper.CreateParameter("@Apellido2", doctor.Apellido2),
                dbHelper.CreateParameter("@IdEspecialidad", doctor.IdEspecialidad),
                dbHelper.CreateParameter("@Telefono", doctor.Telefono),
                dbHelper.CreateParameter("@Correo", doctor.Correo)
            }

            dbHelper.ExecuteNonQuery(sql, parametros)

            Return "Doctor creado correctamente"

        Catch ex As Exception
            Return "Error al crear doctor: " & ex.Message
        End Try
    End Function

    Public Function Update(doctor As Doctores) As String
        Try
            Dim sql As String = "
                UPDATE Doctores SET
                    Nombre=@Nombre,
                    Apellido1=@Apellido1,
                    Apellido2=@Apellido2,
                    IdEspecialidad=@IdEspecialidad,
                    Telefono=@Telefono,
                    Correo=@Correo
                WHERE IdDoctor=@IdDoctor
            "

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@Nombre", doctor.Nombre),
                dbHelper.CreateParameter("@Apellido1", doctor.Apellido1),
                dbHelper.CreateParameter("@Apellido2", doctor.Apellido2),
                dbHelper.CreateParameter("@IdEspecialidad", doctor.IdEspecialidad),
                dbHelper.CreateParameter("@Telefono", doctor.Telefono),
                dbHelper.CreateParameter("@Correo", doctor.Correo),
                dbHelper.CreateParameter("@IdDoctor", doctor.IdDoctor)
            }

            dbHelper.ExecuteNonQuery(sql, parametros)

            Return "Doctor actualizado correctamente"

        Catch ex As Exception
            Return "Error al actualizar doctor: " & ex.Message
        End Try
    End Function

    Public Function Delete(IdDoctor As Integer) As String
        Try
            Dim sql As String = "DELETE FROM Doctores WHERE IdDoctor=@IdDoctor"

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@IdDoctor", IdDoctor)
            }

            dbHelper.ExecuteNonQuery(sql, parametros)

            Return "Doctor eliminado correctamente"

        Catch ex As Exception
            Return "Error al eliminar doctor: " & ex.Message
        End Try
    End Function

    Public Function GetById(IdDoctor As Integer) As Doctores
        Try
            Dim sql As String = "
                SELECT IdDoctor, Nombre, Apellido1, Apellido2, IdEspecialidad, Telefono, Correo
                FROM Doctores
                WHERE IdDoctor = @IdDoctor
            "

            Dim parametros As New List(Of SqlParameter) From {
                dbHelper.CreateParameter("@IdDoctor", IdDoctor)
            }

            Dim dt As DataTable = dbHelper.ExecuteQuery(sql, parametros)

            If dt.Rows.Count = 0 Then Return Nothing

            Dim row = dt.Rows(0)

            Dim doctor As New Doctores With {
                .IdDoctor = Convert.ToInt32(row("IdDoctor")),
                .Nombre = row("Nombre").ToString(),
                .Apellido1 = row("Apellido1").ToString(),
                .Apellido2 = row("Apellido2").ToString(),
                .IdEspecialidad = Convert.ToInt32(row("IdEspecialidad")),
                .Telefono = row("Telefono").ToString(),
                .Correo = row("Correo").ToString()
            }

            Return doctor

        Catch ex As Exception
            Throw New Exception("Error al obtener doctor: " & ex.Message)
        End Try
    End Function

    Public Function GetDoctores() As DataTable
        Try
            Dim sql As String = "
                SELECT IdDoctor, 
                       Nombre + ' ' + Apellido1 + ' ' + Apellido2 AS NombreCompleto
                FROM Doctores
                ORDER BY Nombre, Apellido1, Apellido2
            "

            Return dbHelper.ExecuteQuery(sql)

        Catch ex As Exception
            Throw New Exception("Error al cargar doctores: " & ex.Message)
        End Try
    End Function

End Class

