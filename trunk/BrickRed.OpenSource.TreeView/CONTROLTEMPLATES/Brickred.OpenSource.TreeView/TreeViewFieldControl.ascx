<%@ Control Language="C#" %>
<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%--<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreeViewFieldControl.ascx.cs" Inherits="TreeView.CONTROLTEMPLATES.TreeViewFieldControl" %>--%>

<%--<%@ Control Language="C#" Inherits="TreeView.TreeViewFieldControl,TreeView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=520cbe1bb07ceba1"
    AutoEventWireup="false" CompilationMode="Always" %>--%>
<SharePoint:RenderingTemplate ID="TreeViewFieldControlTemplate" runat="server">
    <Template>
        <table class="ms-authoringcontrols" border="0" width="100%" cellspacing="0" cellpadding="0">
        <tr>
          <%-- <td align="left" valign="top">
                <asp:LinkButton runat="server" ID="lnkExpCollAll"></asp:LinkButton>
                </td>--%>
        </tr>
            <tr>
                <td align="left" valign="top">
                    <asp:TreeView ID="treeViewBrickred"  EnableViewState="true" runat="server">
                    </asp:TreeView>
                </td>
            </tr>
        </table>
    </Template>
</SharePoint:RenderingTemplate>