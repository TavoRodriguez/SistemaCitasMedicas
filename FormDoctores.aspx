<%@ Page Title="Doctores" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="FormDoctores.aspx.vb" Inherits="SistemaCitasMedicas.FormDoctores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="editando" runat="server" />
    <div class="container mt-4">
        <div class="d-flex justify-content-between align-items-center mb-3">
            <h2 class="text-primary">Lista de Doctores 🩺</h2>
            <asp:Button ID="btnAgregar" runat="server" Text="Agregar Doctor" CssClass="btn btn-success" OnClientClick="new bootstrap.Modal(document.getElementById('modalAgregar')).show(); return false;" />

        </div>

        <!-- Filtrado por Especialidad -->
        <div class="mb-3">
            <label for="ddlEspecialidad" class="form-label fw-bold">Filtrar por Especialidad:</label>
            <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select w-50" AutoPostBack="True" OnSelectedIndexChanged="ddlEspecialidad_SelectedIndexChanged">
                <asp:ListItem Text="Todas" Value="0" />
            </asp:DropDownList>
        </div>

        <!-- Tabla Doctores -->
        <div class="table-responsive shadow-sm rounded">
            <asp:GridView ID="gvDoctores" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover align-middle"
                EmptyDataText="No hay doctores registrados" GridLines="None" DataKeyNames="IdDoctor"
                DataSourceID="SqlDataSourceDoctores" 
                OnRowDeleting="gvDoctores_RowDeleting"
                OnRowCommand ="gvDoctores_RowCommand">
               
                
                <Columns>
                    <asp:BoundField DataField="IdDoctor" HeaderText="ID" Visible="False" />

                    <asp:TemplateField HeaderText="Nombre Completo">
                        <ItemTemplate>
                            <%# Eval("Nombre") & " " & Eval("Apellido1") & " " & Eval("Apellido2") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="NombreEspecialidad" HeaderText="Especialidad" />
                    <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                    <asp:BoundField DataField="Correo" HeaderText="Correo" />

                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server"
                                CssClass="btn btn-sm btn-primary me-1"
                                CommandName="EditarDoctor"
                                CommandArgument='<%# Eval("IdDoctor") %>'>Editar</asp:LinkButton>

                            <asp:LinkButton ID="btnEliminar" runat="server"
                                CssClass="btn btn-sm btn-danger"
                                CommandName="Delete"
                                CommandArgument='<%# Eval("IdDoctor") %>'
                                OnClientClick="return confirm('¿Está seguro de eliminar este doctor?');">Eliminar</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>


                </Columns>
            </asp:GridView>
        </div>
    </div>

    <asp:SqlDataSource ID="SqlDataSourceDoctores" runat="server"
        ConnectionString="<%$ ConnectionStrings:CitasMedicasDBConnectionString2 %>"
        SelectCommand="SELECT D.IdDoctor, D.Nombre, D.Apellido1, D.Apellido2, E.NombreEspecialidad, D.Telefono, D.Correo
                       FROM Doctores D
                       INNER JOIN Especialidades E ON D.IdEspecialidad = E.IdEspecialidad
                       WHERE (@IdEspecialidad = 0 OR D.IdEspecialidad = @IdEspecialidad)">
        <SelectParameters>
            <asp:ControlParameter Name="IdEspecialidad" ControlID="ddlEspecialidad" PropertyName="SelectedValue" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

    <!-- modal agregar doctor -->
    <div class="modal fade" id="modalAgregar" tabindex="-1" aria-labelledby="modalAgregarLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:Panel ID="pnlAgregar" runat="server">
                    <div class="modal-header bg-primary text-white">
                        <h5 class="modal-title" id="modalAgregarLabel">Agregar Doctor 🩺</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                    </div>
                    <div class="modal-body">

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
                            <label class="form-label">Especialidad</label>
                            <asp:DropDownList ID="ddlEspecialidadModal" runat="server" CssClass="form-select" />
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



