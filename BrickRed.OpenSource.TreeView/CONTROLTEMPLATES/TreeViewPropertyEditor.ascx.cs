/*
===========================================================================
Copyright (c) 2011 BrickRed Technologies Limited

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sub-license, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
===========================================================================
*/

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
        protected DropDownList ddlKeyColumn;
        protected CheckBox chkExpandCollapse;
        protected Label lblDisplayName;
        protected Label lblKeyColumn;
        protected Label lblParent;
        protected Label lblExpandCollapse;
        protected Label lblExptnMsg;

        private string _ListName = string.Empty;
        private string _ValueColumn = string.Empty;
        private string _ParentLookup = string.Empty;
        private string _KeyColumn = string.Empty;
        private string _ExpandCollapse = string.Empty;

        /// <summary>
        /// Sets input focus to a control.
        /// </summary>
        public override void Focus()
        {
            EnsureChildControls();
            base.Focus();
        }

        #region Overridden CreateChildControls Method

        /// <summary>
        /// Creates any child controls necessary to render the field
        /// </summary>
        protected override void CreateChildControls()
        {
            try
            {
                base.CreateChildControls();
                lblExptnMsg.Text = string.Empty;
                if (!Page.IsPostBack)
                {
                    LoadListNameColumnsDropDownList();
                }
            }
            catch (Exception ex)
            {
                lblExptnMsg.Text = ex.Message;
            }
        }

        #endregion Overridden CreateChildControls Method

        #region  LoadListNameColumnsDropDownList

        /// <summary>
        /// Load listname dropdown and display lookUp column
        /// </summary>
        public void LoadListNameColumnsDropDownList()
        {
            ArrayList objListCollection = new ArrayList();
            string objSelectedList = string.Empty;
            string sListName = string.Empty;
            SPListCollection collSiteLists;

            try
            {
                lblExptnMsg.Text = string.Empty;
                collSiteLists = SPContext.Current.Web.Lists;
                foreach (SPList oList in collSiteLists)
                {
                    objListCollection.Add(oList);
                }

                ddlListName.DataSource = objListCollection;
                ddlListName.DataBind();

                ddlListName.Items.Insert(0, new ListItem(GlobalConstants.TEXT_SELECT_LIST, GlobalConstants.TEXT_SELECT_LIST));
                if (!string.IsNullOrEmpty(_ListName))
                {
                    ddlListName.Items.FindByValue(_ListName).Selected = true;
                }
                
                if (ddlListName.SelectedItem.Value != GlobalConstants.TEXT_SELECT_LIST)
                {
                    ShowHideControls(true);
                    BindDropDownLists(ddlListName.SelectedItem.Value);
                }
                else
                {
                    ShowHideControls(false);
                }
                if (_ExpandCollapse.Equals(GlobalConstants.VALUE_TRUE))
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
                lblExptnMsg.Text = ex.Message;
            }
        }

        /// <summary>
        /// Conditional hide/show dropdown controls.
        /// </summary>
        /// <param name="boolShowStatus"></param>
        private void ShowHideControls(bool boolShowStatus)
        {
            if (boolShowStatus)
            {
                ddlDisplayName.Visible = true;
                ddlKeyColumn.Visible = true;
                lblDisplayName.Visible = true;
                lblKeyColumn.Visible = true;
                lblExpandCollapse.Visible = true;
                chkExpandCollapse.Visible = true;
                lblParent.Visible = true;
                ddlParent.Visible = true;
            }
            else
            {
                ddlDisplayName.Visible = false;
                ddlKeyColumn.Visible = false;
                lblDisplayName.Visible = false;
                lblKeyColumn.Visible = false;
                lblExpandCollapse.Visible = false;
                chkExpandCollapse.Visible = false;
                lblParent.Visible = false;
                ddlParent.Visible = false;
            }
        }

        /// <summary>
        /// Bind different dropdown lists e.g.: DisplayName, Parent, KeyColumn
        /// </summary>
        /// <param name="objSelectedList"></param>
        public void BindDropDownLists(string objSelectedList)
        {
            ArrayList objColumnCollection = new ArrayList();
            ArrayList objLookupColumnCollection = new ArrayList();
            SPList objList;
            SPFieldCollection objFieldCollection;
            string strTest = string.Empty;

            try
            {
                lblExptnMsg.Text = string.Empty;
                objSelectedList = ddlListName.SelectedItem.Value;
                objList = SPContext.Current.Web.Lists[objSelectedList];

                strTest = objList.Views.SchemaXml;
                objFieldCollection = objList.Fields;

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

                ddlDisplayName.DataSource = objColumnCollection;
                ddlDisplayName.DataBind();
                ddlKeyColumn.DataSource = objColumnCollection;
                ddlKeyColumn.DataBind();
                ddlParent.DataSource = objLookupColumnCollection;
                ddlParent.DataBind();

                ddlParent.Items.Insert(0, new ListItem(GlobalConstants.TEXT_SELECT_VALUE, GlobalConstants.TEXT_SELECT_VALUE));
                if (!string.IsNullOrEmpty(_ParentLookup))
                {
                    if (ddlParent.Items.FindByValue(_ParentLookup) != null)
                        ddlParent.Items.FindByValue(_ParentLookup).Selected = true;
                }
                if (!string.IsNullOrEmpty(_ValueColumn))
                {
                    ddlDisplayName.SelectedIndex = ddlDisplayName.Items.IndexOf(((ListItem)ddlDisplayName.Items.FindByText(_ValueColumn)));
                }
                else
                {
                    ddlDisplayName.SelectedIndex = ddlDisplayName.Items.IndexOf(((ListItem)ddlDisplayName.Items.FindByText("Title")));
                }

                if (!string.IsNullOrEmpty(_KeyColumn))
                {
                    ddlKeyColumn.SelectedIndex = ddlKeyColumn.Items.IndexOf(((ListItem)ddlKeyColumn.Items.FindByText(_KeyColumn)));
                }
                else
                {
                    ddlKeyColumn.SelectedIndex = ddlKeyColumn.Items.IndexOf(((ListItem)ddlKeyColumn.Items.FindByText("ID")));
                }
                if (!string.IsNullOrEmpty(_ParentLookup))
                {
                    ddlParent.SelectedIndex = ddlParent.Items.IndexOf(((ListItem)ddlParent.Items.FindByText(_ParentLookup)));
                }
            }
            catch (Exception ex)
            {
                lblExptnMsg.Text = ex.Message;
            }
        }

        #endregion  LoadDropDown for ListName and Display,LookUp Column

        #region ListName_SelectedIndexChanged Event

        /// <summary>
        /// Method called when listnamed dropdown selected index changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlListName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string objSelectedList = string.Empty;
                lblExptnMsg.Text = string.Empty;

                if (ddlListName.SelectedItem.Value != GlobalConstants.TEXT_SELECT_LIST)
                {
                    ShowHideControls(true);
                    BindDropDownLists(ddlListName.SelectedItem.Value);
                }
                else
                {
                    ShowHideControls(false);
                }
            }
            catch (Exception ex)
            {
                lblExptnMsg.Text = ex.Message;
            }
        }

        #endregion ListName_SelectedIndexChanged Event

        /// <summary>
        /// Check field is valid or not
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Display as new section
        /// </summary>
        public bool DisplayAsNewSection
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Initializes the field property editor when the page loads
        /// </summary>
        /// <param name="field">
        /// An object that instantiates a custom field (column) class that derives from
        /// the Microsoft.SharePoint.SPField class.
        /// </param>
        public void InitializeWithField(SPField field)
        {
            if (field != null)
            {
                TreeViewControl sharePointField = (TreeViewControl)field;
                _ListName = sharePointField.ListName;
                _ValueColumn = sharePointField.ValueColumn;
                _ParentLookup = sharePointField.ParentLookup;
                _ExpandCollapse = sharePointField.ExpandCollapse;
                _KeyColumn = sharePointField.KeyColumn;
            }
        }

        /// <summary>
        /// Validates and saves the changes the user has made in the field property editor control.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="isNewField"></param>
        public void OnSaveChange(SPField field, bool isNewField)
        {
            try
            {
                lblExptnMsg.Text = string.Empty;
                TreeViewControl sharePointField = (TreeViewControl)field;
                _ListName = ddlListName.SelectedItem.Text;
                _ValueColumn = ddlDisplayName.SelectedValue;
                _ParentLookup = ddlParent.SelectedValue;
                _KeyColumn = ddlKeyColumn.SelectedValue;
                _ExpandCollapse = chkExpandCollapse.Checked.ToString();

                if (isNewField)
                {
                    //if the field is new, we will set the global new property cache
                    //which helps us avoid all of the conflicts between update states
                    //that can occur during postback processing of the control
                    sharePointField.SetNewColumnPropertyCacheValue(GlobalConstants.TREEVIEW_PROPERTY_LISTNAME, _ListName);
                    sharePointField.SetNewColumnPropertyCacheValue(GlobalConstants.TREEVIEW_PROPERTY_VALUECOLUMN, _ValueColumn);
                    sharePointField.SetNewColumnPropertyCacheValue(GlobalConstants.TREEVIEW_PROPERTY_PARENTLOOKUP, _ParentLookup);
                    sharePointField.SetNewColumnPropertyCacheValue(GlobalConstants.TREEVIEW_PROPERTY_EXPANDCOLLAPSE, _ExpandCollapse);
                    sharePointField.SetNewColumnPropertyCacheValue(GlobalConstants.TREEVIEW_PROPERTY_KEYCOLUMN, _KeyColumn);
                }
                else
                {
                    sharePointField.ListName = ddlListName.SelectedItem.Text;
                    sharePointField.ValueColumn = ddlDisplayName.SelectedValue;
                    sharePointField.ParentLookup = ddlParent.SelectedValue;
                    sharePointField.KeyColumn = ddlKeyColumn.SelectedValue;
                    sharePointField.ExpandCollapse = chkExpandCollapse.Checked.ToString();
                }
            }
            catch (Exception ex)
            {
                lblExptnMsg.Text = ex.Message;
            }
        }
        #endregion
    }
}
