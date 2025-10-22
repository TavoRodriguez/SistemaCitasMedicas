Public Class Doctores
    Private _idDoctor As Integer
    Private _nombre As String
    Private _apellido1 As String
    Private _apellido2 As String
    Private _idEspecialidad As Integer
    Private _telefono As String
    Private _correo As String

    ' Constructor vacío
    Public Sub New()
    End Sub

    ' Constructor con parámetros
    Public Sub New(idDoctor As Integer, nombre As String, apellido1 As String, apellido2 As String,
                   idEspecialidad As Integer, telefono As String, correo As String)
        Me.IdDoctor = idDoctor
        Me.Nombre = nombre
        Me.Apellido1 = apellido1
        Me.Apellido2 = apellido2
        Me.IdEspecialidad = idEspecialidad
        Me.Telefono = telefono
        Me.Correo = correo
    End Sub

    ' Propiedades
    Public Property IdDoctor As Integer
        Get
            Return _idDoctor
        End Get
        Set(value As Integer)
            _idDoctor = value
        End Set
    End Property

    Public Property Nombre As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property Apellido1 As String
        Get
            Return _apellido1
        End Get
        Set(value As String)
            _apellido1 = value
        End Set
    End Property

    Public Property Apellido2 As String
        Get
            Return _apellido2
        End Get
        Set(value As String)
            _apellido2 = value
        End Set
    End Property

    Public Property IdEspecialidad As Integer
        Get
            Return _idEspecialidad
        End Get
        Set(value As Integer)
            _idEspecialidad = value
        End Set
    End Property

    Public Property Telefono As String
        Get
            Return _telefono
        End Get
        Set(value As String)
            _telefono = value
        End Set
    End Property

    Public Property Correo As String
        Get
            Return _correo
        End Get
        Set(value As String)
            _correo = value
        End Set
    End Property
End Class
