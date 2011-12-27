<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%--<%@ Control Language="C#" AutoEventWireup="true" Inherits="TreeView.TreeViewPropertyEditor" %>--%>
<%@ Control Language="C#" Inherits="Brickred.OpenSource.TreeView.TreeViewPropertyEditor,Brickred.OpenSource.TreeView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f3b253169219fb76"
    AutoEventWireup="false" CompilationMode="Always" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/InputFormControl.ascx" %>

<wssuc:InputFormSection ID="InputFormSection1" runat="server"
       Title="Tree View Column Settings"
       Description="List Name specifies the list having the parent Child Relationship. Display Name specifies the node name to be shown in the treeview. Key Value specifies the node value to be saved against the selected node." >
     <template_inputformcontrols>
          <wssuc:InputFormControl runat="server">
              <Template_Control>
              <asp:Label ID="lblListName" Text="List Name" runat="server" Width= "100px"></asp:Label>                
                <asp:DropDownList id="ddlListName" runat="server" AutoPostBack="true"  Title="List Name"  OnSelectedIndexChanged="ddlListName_SelectedIndexChanged"/>
                <asp:RequiredFieldValidator ID="RFVListName" runat="server" ErrorMessage="You must specify a value for this required field!" 
                    ControlToValidate="ddlListName" Display="Dynamic"  InitialValue="--Select List--"></asp:RequiredFieldValidator> 
              </Template_Control>
          </wssuc:InputFormControl>

           <wssuc:InputFormControl runat="server">
                <Template_Control>    
                <asp:Label ID="lblDisplayName" Text="Display Name" runat="server" Visible="false" Width= "100px"></asp:Label>
                    <asp:DropDownList id="ddlDisplayName" runat="server" AutoPostBack="false"  Title="Display Name" Visible="false" />                    
                    </asp:RequiredFieldValidator
                </Template_Control>              

             </wssuc:InputFormControl>

             <wssuc:InputFormControl runat="server">
                <Template_Control>  
                 <asp:Label ID="lblKeyColumn" Text="Value Column" runat="server" Visible="false" Width= "100px"></asp:Label>                 
                    <asp:DropDownList id="ddlKeyColumn" runat="server" AutoPostBack="false"  Title="Value Column"  Visible="false"/>
                    
                </Template_Control>

             </wssuc:InputFormControl>

              <wssuc:InputFormControl runat="server">
                <Template_Control>  
                <asp:Label ID="lblParent" Text="Parent Column" runat="server" Visible="false" Width= "100px"></asp:Label>
                <asp:DropDownList id="ddlParent" runat="server" AutoPostBack="false"  Title="Parent" Visible="false" >
                    
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RFVKeyParent" runat="server" ErrorMessage="You must specify a value for this required field!" 
                    ControlToValidate="ddlParent" Display="Dynamic" InitialValue="--Select Value--"></asp:RequiredFieldValidator>                   
                </Template_Control>
             </wssuc:InputFormControl>
             

              <wssuc:InputFormControl runat="server">
                <Template_Control>  
                <asp:Label ID="lblExpandCollapse" Text="Expand Tree" runat="server" Visible="false" Width= "100px"></asp:Label>
                <asp:CheckBox id="chkExpandCollapse" runat="server"  AutoPostBack="false"  Title="Parent" Visible="false" />                                     
                </Template_Control>
             </wssuc:InputFormControl>            

             <wssuc:InputFormControl runat="server">
                <Template_Control>  
                    <asp:Label ID="lblExptnMsg" Text="Expand Tree" runat="server" ForeColor = "Red"></asp:Label>
                </Template_Control>
             </wssuc:InputFormControl> 
     </template_inputformcontrols>
</wssuc:InputFormSection>



   
