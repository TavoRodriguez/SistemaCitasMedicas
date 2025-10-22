Public Class Usuarios

    Private _idUsuario As Integer
    Private _nombreUsuario As String
    Private _contrasena As String
    Private _idRol As Integer

    ' Constructor vacío
    Public Sub New()
    End Sub

    ' Constructor con parámetros
    Public Sub New(idUsuario As Integer, nombreUsuario As String, contrasena As String, idRol As Integer)
        Me.IdUsuario = idUsuario
        Me.NombreUsuario = nombreUsuario
        Me.Contrasena = contrasena
        Me.IdRol = idRol
    End Sub

    ' Propiedades
    Public Property IdUsuario As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property NombreUsuario As String
        Get
            Return _nombreUsuario
        End Get
        Set(value As String)
            _nombreUsuario = value
        End Set
    End Property

    Public Property Contrasena As String
        Get
            Return _contrasena
        End Get
        Set(value As String)
            _contrasena = value
        End Set
    End Property

    Public Property IdRol As Integer
        Get
            Return _idRol
        End Get
        Set(value As Integer)
            _idRol = value
        End Set
    End Property
End Class


