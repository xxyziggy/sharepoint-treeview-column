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

namespace Brickred.OpenSource.TreeView
{
   public static class GlobalConstants
    {
       public const string TREEVIEW_PROPERTY_LISTNAME = "ListName";
       public const string TREEVIEW_PROPERTY_VALUECOLUMN = "ValueColumn";
       public const string TREEVIEW_PROPERTY_PARENTLOOKUP = "ParentLookup";
       public const string TREEVIEW_PROPERTY_KEYCOLUMN = "KeyColumn";
       public const string TREEVIEW_PROPERTY_EXPANDCOLLAPSE = "ExpandCollapse";
       public const string TREEVIEW_CONTROL_NAME_TREEVIEWBRICKRED = "treeViewBrickred";
       public const string TREEVIEW_TEMPLATE_NAME_TREEVIEWFIELDCONTROLTEMPLATE = "TreeViewFieldControlTemplate";
       public const string TREEVIEW_JS_PATH = "/_layouts/BrickRed.OpenSource.TreeView/BrickRed.TreeViewControl.js";
       public const string LIST_COLUMN_TITLE = "Title";
       public const string VALUE_SEPARATOR = ";#";
       public const string HASH_SEPARATOR = "#";
       public const string MESSAGE_NO_SP_CONTEXT = "NO***SP***CONTEXT";
       public const string ERROR_MESSAGE_TREE_CREATION_FAILED = "Failed to create treeview sturcture";
       public const string ERROR_MESSAGE_CHILDNODE_CREATION_FAILED = "Failed to create childnodes";
       public const string HTML_BR = "<BR/>";
       public const string VALUE_TRUE = "True";
       public const string TEXT_SELECT_VALUE = "--Select Value--";
       public const string TEXT_SELECT_LIST = "--Select List--";
       public const string DYNAMIC_CAML_QUERY = "<Where><IsNull><FieldRef Name='{0}' /></IsNull></Where>";
       public const string DYNAMIC_CAML_QUERY_GET_CHILD_NODE = "<Where><Eq><FieldRef Name='{0}' /><Value Type='LookupMulti'>{1}</Value></Eq></Where>";
    }
}
