Public Class Citas
    Private _idCita As Integer
    Private _idPaciente As Integer
    Private _idDoctor As Integer
    Private _fechaCita As DateTime
    Private _estado As String
    Private _observaciones As String

    ' Constructor vacío
    Public Sub New()
    End Sub

    ' Constructor con parámetros
    Public Sub New(idCita As Integer, idPaciente As Integer, idDoctor As Integer,
                   fechaCita As DateTime, estado As String, observaciones As String)
        Me.IdCita = idCita
        Me.IdPaciente = idPaciente
        Me.IdDoctor = idDoctor
        Me.FechaCita = fechaCita
        Me.Estado = estado
        Me.Observaciones = observaciones
    End Sub

    Public Property IdCita As Integer
        Get
            Return _idCita
        End Get
        Set(value As Integer)
            _idCita = value
        End Set
    End Property

    Public Property IdPaciente As Integer
        Get
            Return _idPaciente
        End Get
        Set(value As Integer)
            _idPaciente = value
        End Set
    End Property

    Public Property IdDoctor As Integer
        Get
            Return _idDoctor
        End Get
        Set(value As Integer)
            _idDoctor = value
        End Set
    End Property

    Public Property FechaCita As DateTime
        Get
            Return _fechaCita
        End Get
        Set(value As DateTime)
            _fechaCita = value
        End Set
    End Property

    Public Property Estado As String
        Get
            Return _estado
        End Get
        Set(value As String)
            _estado = value
        End Set
    End Property

    Public Property Observaciones As String
        Get
            Return _observaciones
        End Get
        Set(value As String)
            _observaciones = value
        End Set
    End Property
End Class
