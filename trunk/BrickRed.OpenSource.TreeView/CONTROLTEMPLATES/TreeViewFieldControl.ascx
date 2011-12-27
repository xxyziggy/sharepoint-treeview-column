<%@ Control Language="C#" %>
<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<SharePoint:RenderingTemplate ID="TreeViewFieldControlTemplate" runat="server">
    <Template>
        <table class="ms-authoringcontrols" border="0" width="100%" cellspacing="0" cellpadding="0">           
            <tr>
                <td align="left" valign="top">
                    <asp:TreeView ID="treeViewBrickred" EnableViewState="true" runat="server">                        
                    </asp:TreeView>
                </td>
                
            </tr>
            <tr>
                <td>
                    <asp:Label ID = "lblExptnMsg" runat = "server" Text = "" ForeColor = "Red"></asp:Label>
                </td>
            </tr>            
        </table>
    </Template>
</SharePoint:RenderingTemplate>
