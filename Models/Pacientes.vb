Public Class Pacientes

    Private _idPaciente As Integer
    Private _nombre As String
    Private _apellido1 As String
    Private _apellido2 As String
    Private _identificacion As String
    Private _fechaNacimiento As Date
    Private _telefono As String
    Private _correo As String

    ' Constructor vacío
    Public Sub New()
    End Sub

    ' Constructor con parámetros (sin IdUsuario)
    Public Sub New(idPaciente As Integer, nombre As String, apellido1 As String, apellido2 As String,
                   identificacion As String, fechaNacimiento As Date, telefono As String, correo As String)

        Me.IdPaciente = idPaciente
        Me.Nombre = nombre
        Me.Apellido1 = apellido1
        Me.Apellido2 = apellido2
        Me.Identificacion = identificacion
        Me.FechaNacimiento = fechaNacimiento
        Me.Telefono = telefono
        Me.Correo = correo
    End Sub

    ' Propiedades
    Public Property IdPaciente As Integer
        Get
            Return _idPaciente
        End Get
        Set(value As Integer)
            _idPaciente = value
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

    Public Property Identificacion As String
        Get
            Return _identificacion
        End Get
        Set(value As String)
            _identificacion = value
        End Set
    End Property

    Public Property FechaNacimiento As Date
        Get
            Return _fechaNacimiento
        End Get
        Set(value As Date)
            _fechaNacimiento = value
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

