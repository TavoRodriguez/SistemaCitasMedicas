Imports System.Data.SqlClient
Public Class dbPacientes
    Public ReadOnly connectionString As String = ConfigurationManager.ConnectionStrings("CitasMedicasDBConnectionString2").ConnectionString
    Public Function GetPacientes() As DataTable
        Dim dt As New DataTable()
        Try
            Dim sql As String = "SELECT IdPaciente, Nombre + ' ' + Apellido1 + ' ' + Apellido2 AS NombreCompleto " &
                                "FROM Pacientes ORDER BY Nombre, Apellido1, Apellido2"

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(sql, connection)
                    connection.Open()
                    Dim reader As SqlDataReader = command.ExecuteReader()
                    dt.Load(reader)
                End Using
            End Using

            Return dt
        Catch ex As Exception
            Throw New Exception("Error al cargar pacientes: " & ex.Message)
        End Try
    End Function

End Class

