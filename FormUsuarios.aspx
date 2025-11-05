<%@ Page Title="Gestión de Usuarios" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="FormUsuarios.aspx.vb" Inherits="SistemaCitasMedicas.FormUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="editando" runat="server" Value="0" />

    <div class="container mt-4">
        <!-- Encabezado -->
        <div class="d-flex justify-content-between align-items-center mb-3">
            <h2 class="text-primary fw-bold">Lista de Usuarios 👥</h2>
            <asp:Button ID="btnAgregar" runat="server" Text="Agregar Usuario" CssClass="btn btn-success"
                OnClientClick="new bootstrap.Modal(document.getElementById('modalAgregar')).show(); return false;" />
        </div>

        <!-- Filtro por Rol -->
        <div class="mb-3">
            <label for="ddlRolFiltro" class="form-label fw-bold">Filtrar por Rol:</label>
            <asp:DropDownList ID="ddlRolFiltro" runat="server" CssClass="form-select w-50"
                AutoPostBack="True" OnSelectedIndexChanged="ddlRolFiltro_SelectedIndexChanged">
                <asp:ListItem Text="Todos" Value="0" />
                <%-- Los demás items se cargarán dinámicamente desde Page_Load o CargarRolesFiltro() --%>
            </asp:DropDownList>
        </div>


        <!-- Tabla de Usuarios -->
        <div class="table-responsive shadow-sm rounded">
            <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="False"
                CssClass="table table-striped table-hover align-middle"
                EmptyDataText="No hay usuarios registrados"
                DataKeyNames="IdUsuario"
                OnRowCommand= "gvUsuarios_RowCommand"
                OnRowDeleting= "gvUsuarios_RowDeleting"
                DataSourceID="SqlDataSourceUsuarios">

                <Columns>
                    <asp:BoundField DataField="IdUsuario" HeaderText="ID" Visible="False" />
                    <asp:BoundField DataField="NombreUsuario" HeaderText="Nombre de Usuario" />
                    <asp:BoundField DataField="Rol" HeaderText="Rol" />

                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server"
                                CssClass="btn btn-sm btn-primary me-1"
                                CommandName="EditarUsuario"
                                CommandArgument='<%# Eval("IdUsuario") %>'>Editar</asp:LinkButton>

                            <asp:LinkButton ID="btnEliminar" runat="server"
                                CssClass="btn btn-sm btn-danger"
                                CommandName="Delete"
                                CommandArgument='<%# Eval("IdUsuario") %>'
                                OnClientClick="return confirm('¿Está seguro de eliminar este usuario?');">Eliminar</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Fuente de datos -->
    <asp:SqlDataSource ID="SqlDataSourceUsuarios" runat="server"
        ConnectionString="<%$ ConnectionStrings:CitasMedicasDBConnectionString2 %>"
        SelectCommand="
            SELECT U.IdUsuario, 
                   U.NombreUsuario, 
                   R.NombreRol AS Rol
            FROM Usuarios U
            INNER JOIN Roles R ON U.IdRol = R.IdRol
            WHERE (@IdRol = 0 OR U.IdRol = @IdRol)">
        <SelectParameters>
            <asp:ControlParameter Name="IdRol" ControlID="ddlRolFiltro" PropertyName="SelectedValue" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

    <!-- Modal agregar/editar usuario -->
    <div class="modal fade" id="modalAgregar" tabindex="-1" aria-labelledby="modalAgregarLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <asp:Panel ID="pnlAgregar" runat="server">
                    <div class="modal-header bg-primary text-white">
                        <h5 class="modal-title" id="modalAgregarLabel">Agregar Usuario 👤</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                    </div>

                    <div class="modal-body">
                        <div class="mb-3">
                            <label class="form-label">Nombre de Usuario</label>
                            <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Contraseña</label>
                            <asp:TextBox ID="txtContrasena" runat="server" CssClass="form-control" TextMode="Password" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Rol</label>
                            <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select" />
                        </div>

                        <asp:Label ID="lblErrorModal" runat="server" CssClass="text-danger fw-bold" Visible="False"></asp:Label>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success" OnClick= "btnGuardar_Click" />
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
