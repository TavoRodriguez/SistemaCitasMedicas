Imports System.Data.SqlClient
Imports System.Configuration

Public Class dbEspecialidad
    Private ReadOnly connectionString As String = ConfigurationManager.ConnectionStrings("CitasMedicasDBConnectionString2").ConnectionString

    Public Function GetEspecialidades() As DataTable
        Dim dt As New DataTable()
        Try
            Using con As New SqlConnection(connectionString)
                Using cmd As New SqlCommand("SELECT IdEspecialidad, NombreEspecialidad FROM Especialidades ORDER BY NombreEspecialidad", con)
                    con.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    dt.Load(reader)
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error al cargar especialidades: " & ex.Message)
        End Try
        Return dt
    End Function
End Class

