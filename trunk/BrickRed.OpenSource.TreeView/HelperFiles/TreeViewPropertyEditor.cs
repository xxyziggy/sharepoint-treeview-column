using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Utilities;
using System.Collections.Generic;
using System.Data;


namespace Brickred.OpenSource.TreeView
{
    [Guid("4D54B1BE-DA47-4478-B48D-B1EDB7651512")]
    public partial class TreeViewPropertyEditor : UserControl, IFieldEditor
    {
        protected DropDownList ddlListName;
        protected DropDownList ddlDisplayName;
        protected DropDownList ddlParent;                
        protected CheckBox chkExpandCollapse;
       


        private string _ListName = string.Empty;
        private string _ValueColumn = string.Empty;
        private string _ParentLookup = string.Empty;        
        private string _ExpandCollapse = string.Empty;
        
        
        


        public override void Focus()
        {
            EnsureChildControls();
            base.Focus();
        }

        #region Overridden CreateChildControls Method

        protected override void CreateChildControls()
        {
            try
            {
                base.CreateChildControls();
                if (!Page.IsPostBack)
                {
                    LoadListNameColumnsDropDownList();                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Overridden CreateChildControls Method

        #region  LoadDropDown for ListName and Display,LookUp Column 

        public void LoadListNameColumnsDropDownList()
        {
            ArrayList objListCollection = new ArrayList();
            string objSelectedList = string.Empty;
            string sListName = string.Empty;
            try
            {
                using (SPSite objSPsite = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb objWeb = objSPsite.OpenWeb())
                    {
                        SPListCollection collSiteLists = objWeb.Lists;
                        foreach (SPList oList in collSiteLists)
                        {
                            objListCollection.Add(oList);
                        }
                    }
                }

                ddlListName.DataSource = objListCollection;
                ddlListName.DataBind();

                if (!string.IsNullOrEmpty(_ListName))
                {
                    ddlListName.Items.FindByValue(_ListName).Selected = true;
                }
                objSelectedList = ddlListName.SelectedItem.Value;
                BindDropDownLists(objSelectedList);

                if (_ExpandCollapse.Equals("True"))
                {
                    chkExpandCollapse.Checked = true;
                }
                else
                {
                    chkExpandCollapse.Checked = false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

             


        public void BindDropDownLists(string objSelectedList)
        {
            ArrayList objColumnCollection = new ArrayList();
            ArrayList objLookupColumnCollection = new ArrayList();
            
            try
            {
                using (SPSite objSPsite = new SPSite(SPContext.Current.Site.Url))
                {
                    using (SPWeb objWeb = objSPsite.OpenWeb())
                    {
                        objSelectedList = ddlListName.SelectedItem.Value;
                        SPList objList = objWeb.Lists[objSelectedList];

                        SPViewFieldCollection objSPViewFieldCollection = objList.DefaultView.ViewFields;

                        string strTest = objList.Views.SchemaXml;


                        SPFieldCollection objFieldCollection = objList.Fields;
                        DataTable dt = objList.Items.GetDataTable();

                        foreach (SPField objField in objFieldCollection)
                        {
                            if (IsValidField(objField))
                            {
                                if (objField.Type == SPFieldType.Lookup)
                                {
                                    SPFieldLookup lookUpColumn = (SPFieldLookup)objField;
                                    if (!lookUpColumn.AllowMultipleValues)
                                    {
                                        objLookupColumnCollection.Add(objField);
                                    }
                                }
                                else
                                {
                                    objColumnCollection.Add(objField);
                                }
                            }
                        }
                    }
                }

                ddlDisplayName.DataSource = objColumnCollection;
                ddlDisplayName.DataBind();
                ddlParent.DataSource = objLookupColumnCollection;
                ddlParent.DataBind();

                if (!string.IsNullOrEmpty(_ParentLookup))
                {
                    ddlParent.SelectedIndex = ddlParent.Items.IndexOf(((ListItem)ddlParent.Items.FindByText(_ParentLookup)));
                    
                }
                if (!string.IsNullOrEmpty(_ValueColumn))
                {
                    ddlDisplayName.SelectedIndex = ddlDisplayName.Items.IndexOf(((ListItem)ddlDisplayName.Items.FindByText(_ValueColumn)));
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion  LoadDropDown for ListName and Display,LookUp Column

        #region ListName_SelectedIndexChanged Event

        protected void ddlListName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string objSelectedList = string.Empty;
                objSelectedList = ddlListName.SelectedItem.Value;
                BindDropDownLists(objSelectedList);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion ListName_SelectedIndexChanged Event

        private bool IsValidField(SPField field)
        {
            return (field.Id == SPBuiltInFieldId.FileLeafRef || field.Hidden == false && (field.ReadOnlyField == false || field.Id == SPBuiltInFieldId.ID)
                && (
                        field.Type == SPFieldType.Counter
                        || field.Type == SPFieldType.Boolean
                        || field.Type == SPFieldType.Integer
                        || field.Type == SPFieldType.Currency
                        || field.Type == SPFieldType.DateTime
                        || field.Type == SPFieldType.Number
                        || field.Type == SPFieldType.Text
                        || field.Type == SPFieldType.URL
                        || field.Type == SPFieldType.User
                        || field.Type == SPFieldType.Choice
                        || field.Type == SPFieldType.MultiChoice
                        || field.Type == SPFieldType.Lookup
                        || field.Type == SPFieldType.File
                        || (field.Type == SPFieldType.Calculated && ((SPFieldCalculated)field).OutputType == SPFieldType.Text))
                        );
        }

        #region IFieldEditor Members

        public bool DisplayAsNewSection
        {
            get
            {
                return true;
            }
        }

        public void InitializeWithField(SPField field)
        {
            if (field != null)
            {

                TreeViewControl sharePointField = (TreeViewControl)field;
                _ListName = sharePointField.ListName;
                _ValueColumn = sharePointField.ValueColumn;
                _ParentLookup = sharePointField.ParentLookup;              
                _ExpandCollapse = sharePointField.ExpandCollapse;
               


            }
        }

        public void OnSaveChange(SPField field, bool isNewField)
        {
            TreeViewControl sharePointField = (TreeViewControl)field;

            _ListName = ddlListName.SelectedItem.Text;
            _ValueColumn = ddlDisplayName.SelectedValue;
            _ParentLookup = ddlParent.SelectedValue;            
            _ExpandCollapse = chkExpandCollapse.Checked.ToString();
           
            if (isNewField)
            {
                //if the field is new, we will set the global new property cache
                //which helps us avoid all of the conflicts between update states
                //that can occur during postback processing of the control
                sharePointField.SetNewColumnPropertyCacheValue(GlobalConstants.TV_PROPERTY_LISTNAME, _ListName);//DropDownListTreeViewList.SelectedValue);
                sharePointField.SetNewColumnPropertyCacheValue(GlobalConstants.TV_PROPERTY_VALUECOLUMN, _ValueColumn);
                sharePointField.SetNewColumnPropertyCacheValue(GlobalConstants.TV_PROPERTY_PARENTLOOKUP, _ParentLookup);
                sharePointField.SetNewColumnPropertyCacheValue(GlobalConstants.TV_PROPERTY_EXPANDCOLLAPSE, _ExpandCollapse);
                
                
            }
            else
            {
                sharePointField.ListName = ddlListName.SelectedItem.Text;
                sharePointField.ValueColumn = ddlDisplayName.SelectedValue;
                sharePointField.ParentLookup = ddlParent.SelectedValue;                
                sharePointField.ExpandCollapse = chkExpandCollapse.Checked.ToString();
                
            }
        }
        #endregion
    }
}
