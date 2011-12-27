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

//************************** Treeview Parent-Child check behaviour ****************************//

function OnTreeClick(evt) {   
    var src = window.event != window.undefined ? window.event.srcElement : evt.target;
    var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
    if (isChkBoxClick) {
        var parentTable = GetParentByTagName("table", src);
        var nxtSibling = parentTable.nextSibling;
        if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
        {
            if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
            {
                //check or uncheck children at all levels
                CheckUncheckChildren(parentTable.nextSibling, src.checked);
            }
        }
        //check or uncheck parents at all levels
        
    }
}

function CheckUncheckChildren(childContainer, check) {   
    var childChkBoxes = childContainer.getElementsByTagName("input");
    var childChkBoxCount = childChkBoxes.length;
    for (var i = 0; i < childChkBoxCount; i++) {
        childChkBoxes[i].checked = check;
    }
}



function AreAllSiblingsChecked(chkBox) {
    var parentDiv = GetParentByTagName("div", chkBox);
    var childCount = parentDiv.childNodes.length;
    for (var i = 0; i < childCount; i++) {
        if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
        {
            if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                //if any of sibling nodes are not checked, return false
                if (!prevChkBox.checked) {
                    return false;
                }
            }
        }
    }
    return true;
}

//utility function to get the container of an element by tagname
function GetParentByTagName(parentTagName, childElementObj) {
    var parent = childElementObj.parentNode;
    while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
        parent = parent.parentNode;
    }
    return parent;
}

////////////////////////////////////////////////////////////////////////////////////// 