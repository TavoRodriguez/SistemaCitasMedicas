<%@ Page Title="Citas Médicas" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="FormCitas.aspx.vb" Inherits="SistemaCitasMedicas.FormCitas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="editando" runat="server" />

    <div class="container mt-4">
        <div class="d-flex justify-content-between align-items-center mb-3">
            <h2 class="text-primary">Lista de Citas 📅</h2>
            <asp:Button ID="btnAgregar" runat="server" Text="Agregar Cita" CssClass="btn btn-success"
                OnClientClick="new bootstrap.Modal(document.getElementById('modalAgregar')).show(); return false;" />
        </div>

        <!-- Filtro por Estado -->
        <div class="mb-3">
            <label for="ddlEstado" class="form-label fw-bold">Filtrar por Estado:</label>
            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select w-50" AutoPostBack="True" OnSelectedIndexChanged ="ddlEstado_SelectedIndexChanged">
                <asp:ListItem Text="Todas" Value="" />
                <asp:ListItem Text="Pendiente" Value="Pendiente" />
                <asp:ListItem Text="Confirmada" Value="Confirmada" />
                <asp:ListItem Text="Cancelada" Value="Cancelada" />
            </asp:DropDownList>
        </div>


        <div class="row" id="citasContainer" runat="server">

        </div>
    </div>

    <!-- modal cita -->
    <div class="modal fade" id="modalAgregar" tabindex="-1" aria-labelledby="modalAgregarLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:Panel ID="pnlAgregar" runat="server">
                    <div class="modal-header bg-primary text-white">
                        <h5 class="modal-title" id="modalAgregarLabel">Agregar Cita 📅</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                    </div>

                    <div class="modal-body">
                        <div class="mb-3">
                            <label class="form-label">Paciente</label>
                            <asp:DropDownList ID="ddlPaciente" runat="server" CssClass="form-select" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Doctor</label>
                            <asp:DropDownList ID="ddlDoctor" runat="server" CssClass="form-select" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Fecha y Hora</label>
                            <asp:TextBox ID="txtFechaCita" runat="server" CssClass="form-control" TextMode ="DateTimeLocal" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Estado</label>
                            <asp:DropDownList ID="ddlEstadoModal" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Pendiente" Value="Pendiente" />
                                <asp:ListItem Text="Confirmada" Value="Confirmada" />
                                <asp:ListItem Text="Cancelada" Value="Cancelada" />
                            </asp:DropDownList>
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Observaciones</label>
                            <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                        </div>

                        <asp:Label ID="lblErrorModal" runat="server" CssClass="text-danger fw-bold" Visible="False"></asp:Label>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success" OnClick ="btnGuardar_Click" />
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

</asp:Content>
