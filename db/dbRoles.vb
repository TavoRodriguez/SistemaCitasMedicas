Imports System.Data.SqlClient

Public Class dbRoles

    Private ReadOnly dbHelper As New DatabaseHelper()
    Public Function GetRoles() As DataTable
        Try
            Dim sql As String =
                "SELECT IdRol, NombreRol 
                 FROM Roles 
                 WHERE IdRol = 1"

            Return dbHelper.ExecuteQuery(sql)

        Catch ex As Exception
            Throw New Exception("Error al cargar los roles: " & ex.Message)
        End Try
    End Function

    Public Function GetRolesTodos() As DataTable
        Try
            Dim sql As String =
                "SELECT IdRol, NombreRol 
                 FROM Roles"

            Return dbHelper.ExecuteQuery(sql)

        Catch ex As Exception
            Throw New Exception("Error al cargar los roles: " & ex.Message)
        End Try
    End Function

End Class

