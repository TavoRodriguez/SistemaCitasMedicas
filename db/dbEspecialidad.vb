Imports System.Data.SqlClient

Public Class dbEspecialidad

    Private ReadOnly dbHelper As New DatabaseHelper()

    Public Function GetEspecialidades() As DataTable
        Try
            Dim sql As String = "SELECT IdEspecialidad, NombreEspecialidad 
                                 FROM Especialidades 
                                 ORDER BY NombreEspecialidad"
            Return dbHelper.ExecuteQuery(sql)

        Catch ex As Exception
            Throw New Exception("Error al cargar especialidades: " & ex.Message)
        End Try
    End Function

End Class


