Namespace Utils
    Module SweetUtils

        Public Sub ShowSwalMessage(page As System.Web.UI.Page, title As String, message As String, icon As String)
            ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), ShowSwalScript(title, message, icon), True)
        End Sub

        Public Function ShowSwalScript(title As String, message As String, icon As String) As String
            Return $"Swal.fire('{title}', '{message}', '{icon}');"
        End Function

        Public Sub ShowSwalError(page As System.Web.UI.Page, message As String)
            ShowSwalMessage(page, "Error", message, "error")
        End Sub

        Public Sub ShowSwal(page As System.Web.UI.Page, title As String, message As String, Optional icon As String = "success")
            ShowSwalMessage(page, title, message, icon)
        End Sub

        Public Sub ShowSwalConfirmDelete(page As System.Web.UI.Page, linkButtonId As String, confirmMessage As String)
            Dim script As String = $"
                document.getElementById('{linkButtonId}').addEventListener('click', function(event) {{
                    event.preventDefault();
                    Swal.fire({{
                        title: '¿Está seguro?',
                        text: '{confirmMessage}',
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#d33',
                        cancelButtonColor: '#3085d6',
                        confirmButtonText: 'Sí, eliminar',
                        cancelButtonText: 'Cancelar'
                    }}).then((result) => {{
                        if (result.isConfirmed) {{
                            __doPostBack('{linkButtonId}', '');
                        }}
                    }});
                }});"
            ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), script, True)
        End Sub
    End Module
End Namespace