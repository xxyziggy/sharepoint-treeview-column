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


    public class TreeViewControl : SPFieldMultiColumn
    {
        internal const string NO_CONTEXT = "NO***SP***CONTEXT";
        SPFieldCollection _fields;
        private string _ListName = string.Empty;
        private string _ValueColumn = string.Empty;
        private string _ParentLookup = string.Empty;
        private string _Expand = string.Empty;
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
            }
            ReadCustomProperties();
        }

        #endregion TreeViewControl Constructors

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
                    throw ex;

                }

            }
            base.OnDeleting();
        }

        public override void OnAdded(SPAddFieldOptions op)
        {
            Update();
            base.OnAdded(op);
        }

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
                    string contextKey = _ContextID + GlobalConstants.TV_PROPERTY_LISTNAME;
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
                    string contextKey = _ContextID + GlobalConstants.TV_PROPERTY_VALUECOLUMN;
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
                    string contextKey = _ContextID + GlobalConstants.TV_PROPERTY_PARENTLOOKUP;
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
                    string contextKey = _ContextID + GlobalConstants.TV_PROPERTY_EXPANDCOLLAPSE;
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

        #endregion Get Set Properties

        #region Read & Write CustomProperties

        private void ReadCustomProperties()
        {
            _ListName = WrappedGetCustomProperty(GlobalConstants.TV_PROPERTY_LISTNAME, string.Empty);
            _ValueColumn = WrappedGetCustomProperty(GlobalConstants.TV_PROPERTY_VALUECOLUMN, string.Empty);
            _ParentLookup = WrappedGetCustomProperty(GlobalConstants.TV_PROPERTY_PARENTLOOKUP, string.Empty);           
            _ExpandCollapse = WrappedGetCustomProperty(GlobalConstants.TV_PROPERTY_EXPANDCOLLAPSE, string.Empty);
           

        }

        private void WriteCustomProperties()
        {
            this.SetCustomProperty(GlobalConstants.TV_PROPERTY_LISTNAME, this.ListName);
            this.SetCustomProperty(GlobalConstants.TV_PROPERTY_VALUECOLUMN, this.ValueColumn);
            this.SetCustomProperty(GlobalConstants.TV_PROPERTY_PARENTLOOKUP, this.ParentLookup);           
            this.SetCustomProperty(GlobalConstants.TV_PROPERTY_EXPANDCOLLAPSE, this.ExpandCollapse);
            


            //Clear the new column property flags
            lock (_NewColumnProperties)
            {
                string contextKey = string.Empty;

                contextKey = _ContextID + GlobalConstants.TV_PROPERTY_LISTNAME;
                if (_NewColumnProperties.ContainsKey(contextKey)) _NewColumnProperties.Remove(contextKey);

                contextKey = _ContextID + GlobalConstants.TV_PROPERTY_VALUECOLUMN;
                if (_NewColumnProperties.ContainsKey(contextKey)) _NewColumnProperties.Remove(contextKey);

                contextKey = _ContextID + GlobalConstants.TV_PROPERTY_PARENTLOOKUP;
                if (_NewColumnProperties.ContainsKey(contextKey)) _NewColumnProperties.Remove(contextKey);               

                contextKey = _ContextID + GlobalConstants.TV_PROPERTY_EXPANDCOLLAPSE;
                if (_NewColumnProperties.ContainsKey(contextKey)) _NewColumnProperties.Remove(contextKey);

               

            }
        }


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
                }
            }
            if (string.IsNullOrEmpty(returnValue)) returnValue = defaultValue;
            return returnValue;
        }

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

    public partial class TreeViewFieldControl : BaseFieldControl
    {
        SPListItemCollection objItems;
        private TreeViewControl objTreeViewControlField;
        System.Web.UI.WebControls.TreeView objTreeView;
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

        protected override string DefaultTemplateName
        {
            get { return GlobalConstants.TV_TEMPLATE_NAME_TREEVIEWFIELDCONTROLTEMPLATE; }
        }

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

        private string GetValue()
        {
            StringBuilder objStringBuilder = new StringBuilder();

            foreach (TreeNode node in objTreeView.CheckedNodes)
            {
                objStringBuilder.Append(node.Value + ";#");
            }
            return objStringBuilder.ToString();
        }

        #region Overridden OnPreRender Method for New and Edit Mode

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SPList TasksList;
            List<SPListItem> objNodeCollectionForDisplay = new List<SPListItem>();            
            SPList objTaskList;
            string objLookupColumn = string.Empty;
            string Title = string.Empty;string objNodeTitle = string.Empty;
            SPQuery objSPQuery;
            StringBuilder Query = new StringBuilder();
            string[] valueArray = null;
            

            if (!Page.IsPostBack)
            {

                if ((SPContext.Current.FormContext.FormMode == SPControlMode.New) || (SPContext.Current.FormContext.FormMode == SPControlMode.Edit))
                {

                    Microsoft.SharePoint.SPFieldMultiColumnValue values = (Microsoft.SharePoint.SPFieldMultiColumnValue)this.ListItemFieldValue;
                    if (values != null)
                        valueArray = values.ToString().Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                    TasksList = SPContext.Current.Web.Lists[objTreeViewControlField.ListName];

                    SPField spField = SPContext.Current.Web.Lists[objTreeViewControlField.ListName].Fields[objTreeViewControlField.ParentLookup];
                    if (!string.IsNullOrEmpty(objTreeViewControlField.ListName))
                    {

                        objSPQuery = new SPQuery();

                        Query.Append(String.Format("<Where><IsNull><FieldRef Name='" + spField.InternalName + "' /></IsNull></Where>"));


                        objSPQuery.Query = Query.ToString();

                        objItems = TasksList.GetItems(objSPQuery);

                        if (objItems != null && objItems.Count > 0)
                        {
                            foreach (SPListItem objItem in objItems)
                            {
                                DisplayColumn = Convert.ToString(objItem[objTreeViewControlField.ValueColumn]);
                                Title = Convert.ToString(objItem[GlobalConstants.LIST_COLUMN_TITLE]);
                                CreateTree(Title, valueArray, null, DisplayColumn);
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

                            valueArray = this.ListItemFieldValue.ToString().Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
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
                                CreateTree(objLstItem[GlobalConstants.LIST_COLUMN_TITLE].ToString(), null, objNodeCollectionForDisplay, objLstItem[objTreeViewControlField.ValueColumn].ToString());
                            }
                            // objTreeView.RenderControl(output);

                        }
                    }
                }
            }

        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            //base.RenderControl(writer);
            RenderChildren(writer);
        }

        #endregion Overridden OnPreRender Method


        #region Overridden CreateChildControls Method

        protected override void CreateChildControls()
        {

            base.CreateChildControls();
            treeViewSelectedValues = new HiddenField();
            treeViewSelectedValues.EnableViewState = true;
            this.Controls.Add(treeViewSelectedValues);
           
                if (this.ControlMode != SPControlMode.Display)
                {
                    objTreeView = (System.Web.UI.WebControls.TreeView)TemplateContainer.FindControl(GlobalConstants.TV_CONTROL_NAME_TREEVIEWBRICKRED);
                    objTreeView.Enabled = true;
                }
                else
                {
                    objTreeView = new System.Web.UI.WebControls.TreeView();
                    objTreeView.Enabled = true;                    
                    this.Controls.Add(objTreeView);
                }
               

        }

        #endregion Overridden CreateChildControls Method


        #region Overridden RenderFieldForDisplay Method for Display Mode

        //protected override void RenderFieldForDisplay(System.Web.UI.HtmlTextWriter output)
        //{
        //    string[] valueArray;
        //    List<SPListItem> objNodeCollectionForDisplay = new List<SPListItem>();
        //    string objNodeTitle = string.Empty;
        //    string objLookupColumn = string.Empty;
        //    StringBuilder Query = new StringBuilder();
        //    SPList objTaskList;
   
          
        //    objLookupColumn = objTreeViewControlField.ParentLookup;

            
        //    if (this.ListItemFieldValue != null)
        //    {
        //        objTaskList = SPContext.Current.Web.Lists[objTreeViewControlField.ListName];
        //        if (objTaskList != null)
        //        {

        //            valueArray = this.ListItemFieldValue.ToString().Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
        //            if (this.ControlMode == SPControlMode.Display)
        //            {
        //                objNodeCollectionForDisplay = CreateNodeCollectionForDisplay(valueArray);
        //            }
        //            List<SPListItem> results = (from SPListItem item in objNodeCollectionForDisplay
        //                                        where (new SPFieldLookupValue(Convert.ToString(item[objTreeViewControlField.ParentLookup])).LookupValue == null)
        //                                        orderby item.ID
        //                                        select item).ToList();

        //            foreach (SPListItem objLstItem in results)
        //            {
        //                CreateTree(objLstItem["Title"].ToString(), null, objNodeCollectionForDisplay, objLstItem[objTreeViewControlField.ValueColumn].ToString());
        //            }
        //            objTreeView.RenderControl(output);

        //        }
        //    }
        //}

        #endregion Overridden RenderFieldForDisplay Method for Display Mode


        #region CreateTree Method

        private void CreateTree(string RootNode, string[] valueArray, List<SPListItem> objNodeCollection, string DisplayValue)
        {
            TreeNode objTreeNode;

            objTreeView.ShowLines = true;

            objTreeNode = new TreeNode(DisplayValue, RootNode);
            objTreeView.Nodes.Add(objTreeNode);
            objTreeNode.SelectAction = TreeNodeSelectAction.None;
            /*******Check for parent node if no child exists Show Checkbox -- Start***************/
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
            /*******Check for parent node if no child exists Show Checkbox -- End***************/
            TreeNodeCollection objTreenodeColn = GetChildNode(RootNode, valueArray, objNodeCollection);

            foreach (TreeNode childnode in objTreenodeColn)
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
            if (objExpandValue.Equals("True"))
            {
                objTreeView.ExpandAll();
            }
            else
            {
                objTreeView.CollapseAll();
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
                Query.Append(String.Format("<Where><Eq><FieldRef Name='" + spField.InternalName + "' /><Value Type='LookupMulti'>{0}</Value></Eq></Where>", RootNode));
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
                    objNodeTitle = Convert.ToString(objItem[objTreeViewControlField.ValueColumn]);
                    if (!String.IsNullOrEmpty(objNodeTitle))
                    {
                        TreeNode childNode = new TreeNode();
                        childNode.Text = objNodeTitle;
                        childNode.Value = RootNode;
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



            // Call method again (recursion) to get the child items



        }

        #endregion CreateTree Method

        private List<SPListItem> CreateNodeCollectionForDisplay(string[] valueArray)
        {

            SPList objRelationShipList;
            SPListItemCollection objRelationShipListItems;
            List<SPListItem> objTreeNodeCollection = new List<SPListItem>();
            objRelationShipList = SPContext.Current.Web.Lists[objTreeViewControlField.ListName];
            objRelationShipListItems = objRelationShipList.Items;

            List<SPListItem> results = (from SPListItem item in objRelationShipListItems
                                        where (valueArray.Contains(item[GlobalConstants.LIST_COLUMN_TITLE].ToString()))
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
                            value = value.Substring(value.IndexOf("#") + 1, value.Length - value.IndexOf("#") - 1);
                            List<SPListItem> objParentItemResult = (from SPListItem ParentItem in results
                                                                    where (ParentItem[GlobalConstants.LIST_COLUMN_TITLE].ToString().Equals(value))
                                                                    orderby ParentItem.ID
                                                                    select ParentItem).ToList();
                            if (objParentItemResult.Count == 0)
                            {
                                value = item[objTreeViewControlField.ParentLookup].ToString();
                                value = value.Substring(value.IndexOf("#") + 1, value.Length - value.IndexOf("#") - 1);
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
