<%@ Page Title="Pacientes" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="FormPacientes.aspx.vb" Inherits="SistemaCitasMedicas.FormPacientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="editando" runat="server" Value="0" />

    <div class="container mt-4">
        <div class="d-flex justify-content-between align-items-center mb-3">
            <h2 class="text-primary">Lista de Pacientes 👨‍⚕️</h2>
            <asp:Button ID="btnAgregar" runat="server" Text="Agregar Paciente" CssClass="btn btn-success"
                OnClientClick="new bootstrap.Modal(document.getElementById('modalAgregar')).show(); return false;" />
        </div>

        <!-- Tabla de Pacientes -->
        <div class="table-responsive shadow-sm rounded">
            <asp:GridView ID="gvPacientes" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover align-middle"
                EmptyDataText="No hay pacientes registrados" GridLines="None" DataKeyNames="IdPaciente"
                DataSourceID="SqlDataSourcePacientes"
                OnRowDeleting="gvPacientes_RowDeleting"
                OnRowCommand="gvPacientes_RowCommand"
                OnRowDataBound ="gvPacientes_RowDataBound">

                <Columns>
                    <asp:BoundField DataField="IdPaciente" HeaderText="ID" Visible="False" />

                    <asp:TemplateField HeaderText="Nombre Completo">
                        <ItemTemplate>
                            <%# Eval("Nombre") & " " & Eval("Apellido1") & " " & Eval("Apellido2") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Identificacion" HeaderText="Identificación" />
                    <asp:BoundField DataField="FechaNacimiento" HeaderText="Fecha Nacimiento" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                    <asp:BoundField DataField="Correo" HeaderText="Correo" />

                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server"
                                CssClass="btn btn-sm btn-primary me-1"
                                CommandName="EditarPaciente"
                                CommandArgument='<%# Eval("IdPaciente") %>'>Editar</asp:LinkButton>

                            <asp:LinkButton ID="btnEliminar" runat="server"
                                CssClass="btn btn-sm btn-danger"
                                CommandName="Delete"
                                CommandArgument='<%# Eval("IdPaciente") %>'>
                                Eliminar
                            </asp:LinkButton>

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <asp:SqlDataSource ID="SqlDataSourcePacientes" runat="server"
        ConnectionString="<%$ ConnectionStrings:CitasMedicasDBConnectionString2 %>"
        SelectCommand="SELECT IdPaciente, Nombre, Apellido1, Apellido2, Identificacion, FechaNacimiento, Telefono, Correo FROM Pacientes"></asp:SqlDataSource>

    <!-- modal pacientes -->
    <div class="modal fade" id="modalAgregar" tabindex="-1" aria-labelledby="modalAgregarLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:Panel ID="pnlAgregar" runat="server">
                    <div class="modal-header bg-primary text-white">
                        <h5 class="modal-title" id="modalAgregarLabel">Agregar Paciente 👨‍⚕️</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                    </div>

                    <div class="modal-body">
                        <!-- campos para los datos del paciente -->
                        <div class="mb-3">
                            <label class="form-label">Nombre</label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Apellido 1</label>
                            <asp:TextBox ID="txtApellido1" runat="server" CssClass="form-control" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Apellido 2</label>
                            <asp:TextBox ID="txtApellido2" runat="server" CssClass="form-control" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Identificación</label>
                            <asp:TextBox ID="txtIdentificacion" runat="server" CssClass="form-control" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Fecha de Nacimiento</label>
                            <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control" TextMode="Date" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Teléfono</label>
                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Correo</label>
                            <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" TextMode="Email" />
                            <asp:RegularExpressionValidator ID="revCorreo" runat="server" ControlToValidate="txtCorreo"
                                ErrorMessage="* Formato de correo inválido"
                                ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"
                                CssClass="text-danger" Display="Dynamic" />
                        </div>

                        <asp:Label ID="lblErrorModal" runat="server" CssClass="text-danger fw-bold" Visible="False"></asp:Label>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success" OnClick="btnGuardar_Click" />
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

</asp:Content>

