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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Brickred.OpenSource.TreeView
{
    /// <summary>
    /// TreeViewControl class helps in calling methods add, update, delete of the control.
    /// </summary>
    public class TreeViewControl : SPFieldMultiColumn
    {
        internal const string NO_CONTEXT = GlobalConstants.MESSAGE_NO_SP_CONTEXT;
        SPFieldCollection _fields;
        private string _ListName = string.Empty;
        private string _ValueColumn = string.Empty;
        private string _ParentLookup = string.Empty;
        private string _Expand = string.Empty;
        private string _KeyColumn = string.Empty;
        private string _ExpandCollapse = string.Empty;
        private string _TextBox = string.Empty;

        string _fieldName;
        string _displayName;
        string _typeName;

        private static System.Collections.Specialized.StringDictionary _NewColumnProperties = new System.Collections.Specialized.StringDictionary();
        private string _ContextID = NO_CONTEXT;

        #region TreeViewControl Constructors

        public TreeViewControl(SPFieldCollection fields,
                                   string fieldName)
            : base(fields, fieldName)
        {
            try
            {
                _fields = fields;
                _fieldName = fieldName;
                _ContextID = SPContext.Current.GetHashCode().ToString();
            }
            catch (Exception Ex)
            {
                _ContextID = NO_CONTEXT;
                throw new Exception(Convert.ToString(Ex.InnerException));
            }
            ReadCustomProperties();
        }

        public TreeViewControl(SPFieldCollection fields,
                                   string typeName,
                                   string displayName)
            : base(fields, typeName, displayName)
        {
            try
            {
                _fields = fields;
                _typeName = typeName;
                _displayName = displayName;
                _ContextID = SPContext.Current.GetHashCode().ToString();
            }
            catch (Exception Ex)
            {
                _ContextID = NO_CONTEXT;
                throw new Exception(Convert.ToString(Ex.InnerException));
            }
            ReadCustomProperties();
        }

        #endregion TreeViewControl Constructors

        /// <summary>
        /// Gets the field type control that is used to render the field in Display,
        /// Edit, and New forms, as well as in data form Web Parts or pages that use
        /// field controls.
        /// </summary>
        public override BaseFieldControl FieldRenderingControl
        {
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                BaseFieldControl ctr = new TreeViewFieldControl(this);
                ctr.FieldName = this.InternalName;
                return ctr;
            }
        }

        /// <summary>
        /// Occurs when a field is being deleted.
        /// </summary>
        public override void OnDeleting()
        {
            string relatedFieldName = this.RelatedField;
            if (!string.IsNullOrEmpty(relatedFieldName))
            {
                try
                {
                    this.ParentList.Fields.Delete(relatedFieldName);
                }
                catch (Exception ex)
                {
                    //Ok to ignore if the user removed the field
                    throw new SPException(ex.Message);
                }

            }
            base.OnDeleting();
        }

        /// <summary>
        /// Occurs after a field is added.
        /// </summary>
        /// <param name="op">
        /// An Microsoft.SharePoint.SPAddFieldOptions value that specifies an option
        /// that is implemented after the field is created.
        /// </param>
        public override void OnAdded(SPAddFieldOptions op)
        {
            Update();
            base.OnAdded(op);
        }

        /// <summary>
        /// Updates the database with changes that are made to the field.
        /// </summary>
        public override void Update()
        {
            WriteCustomProperties();
            base.Update();
        }

        #region Get Set Properties

        public string ListName
        {
            get
            {
                lock (_NewColumnProperties)
                {
                    string contextKey = _ContextID + GlobalConstants.TREEVIEW_PROPERTY_LISTNAME;
                    if (_NewColumnProperties.ContainsKey(contextKey))
                    {
                        return _NewColumnProperties[contextKey];
                    }
                    else
                    {
                        return _ListName;
                    }
                }
            }
            set
            {
                _ListName = value.Trim();
            }
        }

        public string ValueColumn
        {

            get
            {
                lock (_NewColumnProperties)
                {
                    string contextKey = _ContextID + GlobalConstants.TREEVIEW_PROPERTY_VALUECOLUMN;
                    if (_NewColumnProperties.ContainsKey(contextKey))
                    {
                        return _NewColumnProperties[contextKey];
                    }
                    else
                    {
                        return _ValueColumn;
                    }
                }
            }
            set
            {
                _ValueColumn = value.Trim();
            }
        }

        public string ParentLookup
        {

            get
            {
                lock (_NewColumnProperties)
                {
                    string contextKey = _ContextID + GlobalConstants.TREEVIEW_PROPERTY_PARENTLOOKUP;
                    if (_NewColumnProperties.ContainsKey(contextKey))
                    {
                        return _NewColumnProperties[contextKey];
                    }
                    else
                    {
                        return _ParentLookup;
                    }
                }
            }
            set
            {
                _ParentLookup = value.Trim();
            }
        }

        public string ExpandCollapse
        {

            get
            {
                lock (_NewColumnProperties)
                {
                    string contextKey = _ContextID + GlobalConstants.TREEVIEW_PROPERTY_EXPANDCOLLAPSE;
                    if (_NewColumnProperties.ContainsKey(contextKey))
                    {
                        return _NewColumnProperties[contextKey];
                    }
                    else
                    {
                        return _ExpandCollapse;
                    }
                }
            }
            set
            {
                _ExpandCollapse = value.Trim();
            }
        }

        public string KeyColumn
        {

            get
            {
                lock (_NewColumnProperties)
                {
                    string contextKey = _ContextID + GlobalConstants.TREEVIEW_PROPERTY_KEYCOLUMN;
                    if (_NewColumnProperties.ContainsKey(contextKey))
                    {
                        return _NewColumnProperties[contextKey];
                    }
                    else
                    {
                        return _KeyColumn;
                    }
                }
            }
            set
            {
                _KeyColumn = value.Trim();
            }
        }

        #endregion Get Set Properties

        #region Read & Write CustomProperties

        /// <summary>
        /// Read custom property e.g.: ListName, ValueColumn, ParentLookup, ExpandCollapse, KeyColumn.
        /// </summary>
        private void ReadCustomProperties()
        {
            _ListName = WrappedGetCustomProperty(GlobalConstants.TREEVIEW_PROPERTY_LISTNAME, string.Empty);
            _ValueColumn = WrappedGetCustomProperty(GlobalConstants.TREEVIEW_PROPERTY_VALUECOLUMN, string.Empty);
            _ParentLookup = WrappedGetCustomProperty(GlobalConstants.TREEVIEW_PROPERTY_PARENTLOOKUP, string.Empty);
            _ExpandCollapse = WrappedGetCustomProperty(GlobalConstants.TREEVIEW_PROPERTY_EXPANDCOLLAPSE, string.Empty);
            _KeyColumn = WrappedGetCustomProperty(GlobalConstants.TREEVIEW_PROPERTY_KEYCOLUMN, string.Empty);

        }

        /// <summary>
        /// Write custom property e.g.: ListName, ValueColumn, ParentLookup, ExpandCollapse, KeyColumn.
        /// </summary>
        private void WriteCustomProperties()
        {
            this.SetCustomProperty(GlobalConstants.TREEVIEW_PROPERTY_LISTNAME, this.ListName);
            this.SetCustomProperty(GlobalConstants.TREEVIEW_PROPERTY_VALUECOLUMN, this.ValueColumn);
            this.SetCustomProperty(GlobalConstants.TREEVIEW_PROPERTY_PARENTLOOKUP, this.ParentLookup);
            this.SetCustomProperty(GlobalConstants.TREEVIEW_PROPERTY_EXPANDCOLLAPSE, this.ExpandCollapse);
            this.SetCustomProperty(GlobalConstants.TREEVIEW_PROPERTY_KEYCOLUMN, this.KeyColumn);

            //Clear the new column property flags
            lock (_NewColumnProperties)
            {
                string contextKey = string.Empty;
                contextKey = _ContextID + GlobalConstants.TREEVIEW_PROPERTY_LISTNAME;
                if (_NewColumnProperties.ContainsKey(contextKey)) _NewColumnProperties.Remove(contextKey);

                contextKey = _ContextID + GlobalConstants.TREEVIEW_PROPERTY_VALUECOLUMN;
                if (_NewColumnProperties.ContainsKey(contextKey)) _NewColumnProperties.Remove(contextKey);

                contextKey = _ContextID + GlobalConstants.TREEVIEW_PROPERTY_PARENTLOOKUP;
                if (_NewColumnProperties.ContainsKey(contextKey)) _NewColumnProperties.Remove(contextKey);

                contextKey = _ContextID + GlobalConstants.TREEVIEW_PROPERTY_EXPANDCOLLAPSE;
                if (_NewColumnProperties.ContainsKey(contextKey)) _NewColumnProperties.Remove(contextKey);

                contextKey = _ContextID + GlobalConstants.TREEVIEW_PROPERTY_KEYCOLUMN;
                if (_NewColumnProperties.ContainsKey(contextKey)) _NewColumnProperties.Remove(contextKey);
            }
        }

        /// <summary>
        /// Get custom property
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private string WrappedGetCustomProperty(string propertyName, string defaultValue)
        {
            //Since we have the static collection of pending updates
            //we need to check it first , if its not there we will
            //try to read it as a custom property
            string returnValue = string.Empty;
            lock (_NewColumnProperties)
            {
                string contextKey = _ContextID + propertyName;
                if (_NewColumnProperties.ContainsKey(contextKey))
                {
                    returnValue = _NewColumnProperties[contextKey];
                }
            }
            if (string.IsNullOrEmpty(returnValue))
            {
                try
                {
                    object sharePointCastValue = this.GetCustomProperty(propertyName);
                    returnValue = this.GetCustomProperty(propertyName) as string;
                    if (string.IsNullOrEmpty(returnValue))
                    {
                        if (null != sharePointCastValue)
                        {
                            returnValue = sharePointCastValue.ToString();
                        }
                    }
                }
                catch (Exception Ex)
                {
                    returnValue = defaultValue;
                    throw new Exception(Convert.ToString(Ex.InnerException));
                }
            }
            if (string.IsNullOrEmpty(returnValue)) returnValue = defaultValue;
            return returnValue;
        }

        /// <summary>
        /// Set new column property cache values
        /// </summary>
        /// <param name="propertyKey"></param>
        /// <param name="propertyValue"></param>
        public void SetNewColumnPropertyCacheValue(string propertyKey, string propertyValue)
        {
            //Add a new string value to the static new property cache
            lock (_NewColumnProperties)
            {
                string contextKey = _ContextID + propertyKey;
                _NewColumnProperties[contextKey] = propertyValue.Trim();
            }
        }

        #endregion Read & Write CustomProperties

        /// <summary>
        /// Converts the specified value into a field type value object when the field
        /// type requires a complex data type that is different from the parent field
        /// type.
        /// </summary>
        /// <param name="value">A string to convert into a field type value object.</param>
        /// <returns>An object that repesents the field type value object.</returns>
        public override object GetFieldValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return new TreeViewFieldValue(value);
        }
    }

    #region Class TreeViewFieldControl

    /// <summary>
    /// TreeViewFieldControl class helps in rendring the stored values. 
    /// </summary>
    public partial class TreeViewFieldControl : BaseFieldControl
    {
        SPListItemCollection objItems;
        private TreeViewControl objTreeViewControlField;
        System.Web.UI.WebControls.TreeView objTreeView;
        Label lblExptnMsg;
        HiddenField treeViewSelectedValues;

        private string DisplayColumn;
        private string objExpandValue = string.Empty;

        #region Constructor

        public TreeViewFieldControl(TreeViewControl treeViewField)
        {
            objTreeViewControlField = treeViewField;
        }

        public TreeViewFieldControl()
        {
        }

        #endregion Constructor

        /// <summary>
        /// Gets or sets the name of the template that can be used to control the rendering
        /// of the Microsoft.SharePoint.WebControls.BaseFieldControl object in display mode; 
        /// that is, when it is not on a New or Edit form.
        /// </summary>
        protected override string DefaultTemplateName
        {
            get { return GlobalConstants.TREEVIEW_TEMPLATE_NAME_TREEVIEWFIELDCONTROLTEMPLATE; }
        }

        /// <summary>
        /// Ggets or sets the value of the field in the UI.
        /// </summary>
        public override object Value
        {
            get
            {
                EnsureChildControls();
                return GetValue();
            }
            set
            {
                EnsureChildControls();
                Microsoft.SharePoint.SPFieldMultiColumnValue fieldValue = value as Microsoft.SharePoint.SPFieldMultiColumnValue;
            }
        }

        /// <summary>
        /// Gets value from control
        /// </summary>
        /// <returns></returns>
        private string GetValue()
        {
            StringBuilder objStringBuilder = new StringBuilder();

            foreach (TreeNode node in objTreeView.CheckedNodes)
            {
                objStringBuilder.Append(node.Value + GlobalConstants.VALUE_SEPARATOR);
            }
            return objStringBuilder.ToString();
        }

        #region Overridden OnPreRender Method for New and Edit Mode

        /// <summary>
        /// Represents the method that handles the System.Web.UI.Control.PreRender event
        /// of a Microsoft.SharePoint.WebControls.FieldMetadata object.
        /// </summary>
        /// <param name="e">An System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SPList TasksList;
            List<SPListItem> objNodeCollectionForDisplay = new List<SPListItem>();
            SPList objTaskList;
            string objLookupColumn = string.Empty;
            string Title = string.Empty; string objNodeTitle = string.Empty;
            SPQuery objSPQuery;
            StringBuilder Query = new StringBuilder();
            string[] valueArray = null;
            SPField spField;

            if (!Page.IsPostBack)
            {
                try
                {
                    lblExptnMsg.Text = string.Empty;;
                    if ((SPContext.Current.FormContext.FormMode == SPControlMode.New) || (SPContext.Current.FormContext.FormMode == SPControlMode.Edit))
                    {
                        Microsoft.SharePoint.SPFieldMultiColumnValue objMultiColumnValues = (Microsoft.SharePoint.SPFieldMultiColumnValue)this.ListItemFieldValue;
                        if (objMultiColumnValues != null)
                            valueArray = objMultiColumnValues.ToString().Split(new string[] { GlobalConstants.VALUE_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                        TasksList = SPContext.Current.Web.Lists[objTreeViewControlField.ListName];

                        spField = SPContext.Current.Web.Lists[objTreeViewControlField.ListName].Fields[objTreeViewControlField.ParentLookup];
                        if (!string.IsNullOrEmpty(objTreeViewControlField.ListName))
                        {
                            objSPQuery = new SPQuery();
                            Query.Append(String.Format(GlobalConstants.DYNAMIC_CAML_QUERY, spField.InternalName));
                            objSPQuery.Query = Query.ToString();
                            objItems = TasksList.GetItems(objSPQuery);
                            if (objItems != null && objItems.Count > 0)
                            {
                                foreach (SPListItem objItem in objItems)
                                {
                                    DisplayColumn = Convert.ToString(objItem[objTreeViewControlField.ValueColumn]);
                                    Title = Convert.ToString(objItem[GlobalConstants.LIST_COLUMN_TITLE]);                                    
                                    CreateTree(Title, valueArray, null, DisplayColumn, objItem[objTreeViewControlField.KeyColumn].ToString());
                                }
                            }
                        }
                    }
                    else
                    {
                        objLookupColumn = objTreeViewControlField.ParentLookup;
                        if (this.ListItemFieldValue != null)
                        {
                            objTaskList = SPContext.Current.Web.Lists[objTreeViewControlField.ListName];
                            if (objTaskList != null)
                            {
                                valueArray = this.ListItemFieldValue.ToString().Split(new string[] { GlobalConstants.VALUE_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                                if (this.ControlMode == SPControlMode.Display)
                                {
                                    objNodeCollectionForDisplay = CreateNodeCollectionForDisplay(valueArray);
                                }
                                List<SPListItem> results = (from SPListItem item in objNodeCollectionForDisplay
                                                            where (new SPFieldLookupValue(Convert.ToString(item[objTreeViewControlField.ParentLookup])).LookupValue == null)
                                                            orderby item.ID
                                                            select item).ToList();

                                foreach (SPListItem objLstItem in results)
                                {
                                    CreateTree(objLstItem[GlobalConstants.LIST_COLUMN_TITLE].ToString(), null, objNodeCollectionForDisplay, objLstItem[objTreeViewControlField.ValueColumn].ToString(), objLstItem[objTreeViewControlField.KeyColumn].ToString());
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(ex.InnerException)))
                    {
                        lblExptnMsg.Text = ex.Message;
                    }
                    else
                    {
                        lblExptnMsg.Text = Convert.ToString(ex.InnerException);
                    }
                }
            }
        }

        /// <summary>
        /// Outputs server control content to a provided System.Web.UI.HtmlTextWriter
        /// object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The System.Web.UI.HTmlTextWriter object that receives the control content.</param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            // Outputs the content of a server control's children to a provided System.Web.UI.HtmlTextWriter
            // object, which writes the content to be rendered on the client.
            RenderChildren(writer);
        }

        #endregion Overridden OnPreRender Method

        #region Overridden CreateChildControls Method

        /// <summary>
        /// Creates any child controls necessary to render the field
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            treeViewSelectedValues = new HiddenField();
            treeViewSelectedValues.EnableViewState = true;
            this.Controls.Add(treeViewSelectedValues);

            if (this.ControlMode != SPControlMode.Display)
            {
                objTreeView = (System.Web.UI.WebControls.TreeView)TemplateContainer.FindControl(GlobalConstants.TREEVIEW_CONTROL_NAME_TREEVIEWBRICKRED);
                objTreeView.Enabled = true;
                lblExptnMsg = (Label)TemplateContainer.FindControl("lblExptnMsg");
                this.Page.ClientScript.RegisterClientScriptInclude("CheckChild", GlobalConstants.TREEVIEW_JS_PATH);
                objTreeView.Attributes.Add("onclick", "OnTreeClick(event);");
            }
            else
            {
                objTreeView = new System.Web.UI.WebControls.TreeView();
                objTreeView.Enabled = true;
                this.Controls.Add(objTreeView);
                lblExptnMsg = new Label();
                lblExptnMsg.Text = string.Empty;;
                lblExptnMsg.ForeColor = System.Drawing.Color.Red;
                this.Controls.Add(lblExptnMsg);
            }
        }

        #endregion Overridden CreateChildControls Method

        #region CreateTree Method

        /// <summary>
        /// Create Tree method in real create the tree based on the parameters.
        /// </summary>
        /// <param name="RootNode"></param>
        /// <param name="valueArray"></param>
        /// <param name="objNodeCollection"></param>
        /// <param name="DisplayValue"></param>
        /// <param name="KeyValue"></param>
        private void CreateTree(string RootNode, string[] valueArray, List<SPListItem> objNodeCollection, string DisplayValue, string KeyValue)
        {
            TreeNode objTreeNode;
            TreeNodeCollection objChildNodeColn; 
            try
            {
                objTreeView.ShowLines = true;
                objTreeNode = new TreeNode(DisplayValue, KeyValue);
                objTreeView.Nodes.Add(objTreeNode);
                objTreeNode.SelectAction = TreeNodeSelectAction.None;

                //Check for parent node if no child exists Show Checkbox
                //Start
                if (this.ControlMode != SPControlMode.Display)
                {
                    if (valueArray != null && this.ControlMode == SPControlMode.Edit && valueArray.Contains(objTreeNode.Value))
                    {
                        objTreeNode.ShowCheckBox = true;
                        objTreeNode.Checked = true;
                    }
                    else
                    {
                        objTreeNode.ShowCheckBox = true;
                    }
                }
                //End

                objChildNodeColn = GetChildNode(RootNode, valueArray, objNodeCollection);
                foreach (TreeNode childnode in objChildNodeColn)
                {
                    objTreeNode.ChildNodes.Add(childnode);
                    childnode.SelectAction = TreeNodeSelectAction.None;
                    if (valueArray != null && this.ControlMode == SPControlMode.Edit && valueArray.Contains(objTreeNode.Value))
                    {
                        objTreeNode.ShowCheckBox = true;
                        objTreeNode.Checked = true;
                    }
                    else if (this.ControlMode == SPControlMode.Display)
                    {
                        objTreeNode.ShowCheckBox = false;
                    }
                    else
                    {
                        objTreeNode.ShowCheckBox = true;
                    }
                }

                objExpandValue = objTreeViewControlField.ExpandCollapse.ToString();
                if (objExpandValue.Equals(GlobalConstants.VALUE_TRUE))
                {
                    objTreeView.ExpandAll();
                }
                else
                {
                    objTreeView.CollapseAll();
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(Convert.ToString(ex.InnerException)))
                {
                    throw new Exception(GlobalConstants.ERROR_MESSAGE_TREE_CREATION_FAILED + GlobalConstants.HTML_BR + ex.Message);
                }
                else
                {
                    throw new Exception(Convert.ToString(ex.InnerException));
                }
            }
        }

        private TreeNodeCollection GetChildNode(string RootNode, string[] valueArray, List<SPListItem> objListItemColn)
        {
            TreeNodeCollection childtreenodes = new TreeNodeCollection();
            SPQuery objSPQuery;
            SPListItemCollection objItems = null;
            List<SPListItem> objNodeListItems = new List<SPListItem>();
            SiteMapNodeCollection objNode = new SiteMapNodeCollection();
            objSPQuery = new SPQuery();
            string objNodeTitle = string.Empty;
            string objLookupColumn = string.Empty;
            StringBuilder Query = new StringBuilder();
            SPList objTaskList;
            SPField spField;
            string objKeyColumn;

            try
            {
                objTaskList = SPContext.Current.Web.Lists[objTreeViewControlField.ListName];
                objLookupColumn = objTreeViewControlField.ParentLookup;

                spField = SPContext.Current.Web.Lists[objTreeViewControlField.ListName].Fields[objTreeViewControlField.ParentLookup];
                if (this.ControlMode == SPControlMode.Display)
                {
                    objNodeListItems = (from SPListItem Item in objListItemColn
                                        where (new SPFieldLookupValue(Convert.ToString(Item[objTreeViewControlField.ParentLookup])).LookupValue == RootNode)
                                        orderby Item.ID
                                        select Item).ToList();

                }
                else
                {
                    Query.Append(String.Format(GlobalConstants.DYNAMIC_CAML_QUERY_GET_CHILD_NODE, spField.InternalName, RootNode));
                    objSPQuery.Query = Query.ToString();


                    objItems = objTaskList.GetItems(objSPQuery);
                    foreach (SPListItem objItem in objItems)
                    {
                        objNodeListItems.Add(objItem);
                    }

                }
                if (objNodeListItems != null && objNodeListItems.Count > 0)
                {
                    foreach (SPListItem objItem in objNodeListItems)
                    {
                        RootNode = Convert.ToString(objItem[GlobalConstants.LIST_COLUMN_TITLE]);
                        objKeyColumn = Convert.ToString(objItem[objTreeViewControlField.KeyColumn]);

                        objNodeTitle = Convert.ToString(objItem[objTreeViewControlField.ValueColumn]);
                        if (!String.IsNullOrEmpty(objNodeTitle))
                        {
                            TreeNode childNode = new TreeNode();
                            childNode.Text = objNodeTitle;
                            childNode.Value = objKeyColumn;
                            childNode.ExpandAll();

                            if (valueArray != null && this.ControlMode == SPControlMode.Edit && valueArray.Contains(childNode.Value))
                            {
                                childNode.ShowCheckBox = true;
                                childNode.Checked = true;
                            }
                            else if (this.ControlMode == SPControlMode.Display)
                            {
                                childNode.ShowCheckBox = false;
                            }
                            else
                            {
                                childNode.ShowCheckBox = true;
                            }

                            foreach (TreeNode cnode in GetChildNode(RootNode, valueArray, objListItemColn))
                            {
                                childNode.ChildNodes.Add(cnode);
                                cnode.SelectAction = TreeNodeSelectAction.None;
                                if (valueArray != null && this.ControlMode == SPControlMode.Edit && valueArray.Contains(cnode.Value))
                                {
                                    cnode.ShowCheckBox = true;
                                    cnode.Checked = true;
                                }
                                else if (this.ControlMode == SPControlMode.Display)
                                {
                                    cnode.ShowCheckBox = false;
                                }
                                else
                                {
                                    cnode.ShowCheckBox = true;
                                }
                            }
                            childtreenodes.Add(childNode);
                        }
                    }
                }
                return childtreenodes;
            }
            catch (Exception ex)
            {
                throw new Exception(GlobalConstants.ERROR_MESSAGE_CHILDNODE_CREATION_FAILED + GlobalConstants.HTML_BR + ex.Message);
            }
            // Call method again (recursion) to get the child items
        }

        #endregion CreateTree Method

        /// <summary>
        /// Create nod collection for display purpose.
        /// </summary>
        /// <param name="valueArray">Inputs collection of values</param>
        /// <returns>List of SPList item</returns>
        private List<SPListItem> CreateNodeCollectionForDisplay(string[] valueArray)
        {
            SPList objRelationShipList;
            SPListItemCollection objRelationShipListItems;
            List<SPListItem> objTreeNodeCollection = new List<SPListItem>();
            objRelationShipList = SPContext.Current.Web.Lists[objTreeViewControlField.ListName];
            objRelationShipListItems = objRelationShipList.Items;

            List<SPListItem> results = (from SPListItem item in objRelationShipListItems                                        
                                        where (valueArray.Contains(item[objTreeViewControlField.KeyColumn].ToString()))
                                        orderby item.ID
                                        select item).ToList();
            objTreeNodeCollection = results;

            if (results.Count > 0)
            {
                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i] != null)
                    {
                        SPListItem item = results[i];
                        string value = Convert.ToString(item[objTreeViewControlField.ParentLookup]);
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = value.Substring(value.IndexOf(GlobalConstants.HASH_SEPARATOR) + 1, value.Length - value.IndexOf(GlobalConstants.HASH_SEPARATOR) - 1);
                            List<SPListItem> objParentItemResult = (from SPListItem ParentItem in results
                                                                    where (ParentItem[GlobalConstants.LIST_COLUMN_TITLE].ToString().Equals(value))                                                                   
                                                                    orderby ParentItem.ID
                                                                    select ParentItem).ToList();
                            if (objParentItemResult.Count == 0)
                            {
                                value = item[objTreeViewControlField.ParentLookup].ToString();
                                value = value.Substring(value.IndexOf(GlobalConstants.HASH_SEPARATOR) + 1, value.Length - value.IndexOf(GlobalConstants.HASH_SEPARATOR) - 1);
                                objParentItemResult = (from SPListItem ParentItem in objRelationShipListItems
                                                       where (ParentItem[GlobalConstants.LIST_COLUMN_TITLE].ToString().Equals(value))                                                       
                                                       orderby ParentItem.ID
                                                       select ParentItem).ToList();
                                if (objParentItemResult.Count != 0)
                                {
                                    objTreeNodeCollection.Add(objParentItemResult[0]);
                                }

                            }
                        }
                    }
                }
            }
            return objTreeNodeCollection;
        }
    }

    #endregion Class TreeViewFieldControl
}
