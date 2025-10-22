Imports System.Data.SqlClient

Public Class dbRoles
    Private ReadOnly connectionString As String = ConfigurationManager.ConnectionStrings("CitasMedicasDBConnectionString2").ConnectionString

    Public Function GetRoles() As DataTable
        Dim dt As New DataTable()
        Try
            Using con As New SqlConnection(connectionString)
                Using cmd As New SqlCommand("SELECT IdRol, NombreRol FROM Roles WHERE IdRol = 1", con)
                    con.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    dt.Load(reader)
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Error al cargar los roles: " & ex.Message)
        End Try
        Return dt
    End Function
End Class
