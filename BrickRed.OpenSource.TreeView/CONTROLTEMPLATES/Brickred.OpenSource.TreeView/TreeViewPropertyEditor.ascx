<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%--<%@ Control Language="C#" AutoEventWireup="true" Inherits="TreeView.TreeViewPropertyEditor" %>--%>
<%@ Control Language="C#" Inherits="BrickRed.OpenSource.TreeView.TreeViewPropertyEditor,BrickRed.OpenSource.TreeView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f3b253169219fb76"
    AutoEventWireup="false" CompilationMode="Always" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/InputFormControl.ascx" %>

<wssuc:InputFormSection ID="InputFormSection1" runat="server"
       Title="Tree View Column Settings"
       Description="&nbsp;&nbsp;Specify detailed options for the type of information you selected." >
     <template_inputformcontrols>
          <wssuc:InputFormControl runat="server" LabelText="&nbsp;&nbsp;&nbsp;List Name ">
              <Template_Control>                   
                <asp:DropDownList id="ddlListName" runat="server" AutoPostBack="true"  Title="List Name"  OnSelectedIndexChanged="ddlListName_SelectedIndexChanged"/>
              </Template_Control>
          </wssuc:InputFormControl>

           <wssuc:InputFormControl runat="server" LabelText="&nbsp;&nbsp;&nbsp;Display Name" 
                    LabelAssociatedControlId="ddlDisplayName">
                <Template_Control>                   
                    <asp:DropDownList id="ddlDisplayName" runat="server" AutoPostBack="false"  Title="Display Name" />
                </Template_Control>
             </wssuc:InputFormControl>

              <wssuc:InputFormControl runat="server" LabelText="&nbsp;&nbsp;&nbsp;Parent" 
                    LabelAssociatedControlId="ddlParent">
                <Template_Control>  
                <asp:DropDownList id="ddlParent" runat="server" AutoPostBack="false"  Title="Parent" />                                     
                </Template_Control>
             </wssuc:InputFormControl>

     </template_inputformcontrols>
</wssuc:InputFormSection>



   
