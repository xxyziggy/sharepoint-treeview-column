## Prerequisites ##

> SharePoint Foundation 2010 / SharePoint 2010 Environment

## Step 1. WSP Installation ##

[Download](http://code.google.com/p/sharepoint-treeview-column/downloads/list) WSP for 2010 and unzip the TreeView archive. This contains the WSP file and power shell scripts files to retract and install.

### 2010 Installation ###

  * Right click on PackageDeploy.ps1 file and click "Run with PowerShell"

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/powershellinstall.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/powershellinstall.png)

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/powershellinstall1.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/powershellinstall1.png)

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/powershellinstall2.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/powershellinstall2.png)

  * If you have already installed this solution before; then you will be given 3 options :

> Retract and Install solution

> Upgrade Solution

> Exit
> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/SolutionAlreadyExists.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/SolutionAlreadyExists.png)

  * In this demo, I opted for 1st option <br />

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/Deployment-Success.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/Deployment-Success.png)

  * This will globally deploy the solution.

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/GloballyDeployedStatus.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/GloballyDeployedStatus.png)


  * Also, there is another powersheel script - PackageRetract.ps1. This script is used to rectrat solution.

  * Right click on PackageRetract.ps1 file and click "Run with PowerShell"

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/RetractSolution.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/RetractSolution.png)

## Step 2. How to use ##

> After deployment, this custom field will be available within any SharePoint site under List Setting -> Create new column.

> Using this column, you can create TreeView type of column.

  * Before you create TreeView column, first create a master List that holds the master Parent/Child data, means what we are going to show at Root node of the tree and what on leaf node.

> For this demo we have create a list named "Parent Child Master" with two columns -
    1. Title (Single line text field)
> > 2. Parent (Lookup of Title field of the same list)


> The list column structure will be look like this:

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/ListColumnStructure.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/ListColumnStructure.png)

> After that, put some data into the list. For demo purpose, we have stored Country, Province and City data into this list. To make root or parent, just leave "Parent" column blank.

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/ParentChildMasterData.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/ParentChildMasterData.png)

  * After creating master list, create another list that in real use the TreeView field. For demo purpose, create "Tree Demo" list with two columns:
    1. Title (Single line text)
> > 2. Tree (TreeView type)


> While creating Tree column, in the existing sharepoint fields you will see your newly deployed column "TreeView"

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeViewColumn.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeViewColumn.png)

> After select "TreeView" field, you need to set some values:

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeColumn_ChooseListName.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeColumn_ChooseListName.png)

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeColumn_SetOtherValues.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeColumn_SetOtherValues.png)

> After filling the required values, your "Tree" field is ready.

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeDemoListColumnStructure.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeDemoListColumnStructure.png)

> "Tree" column in edit mode:

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeColumn_EditMode.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeColumn_EditMode.png)

  * After this you will see your Tree while putting data into the list.

> AddNew Item - New Form

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeDemo_NewForm.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeDemo_NewForm.png)

> Edit existing item - EditForm

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeDemo_EditForm.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeDemo_EditForm.png)

> Display existing item - DisplayForm

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeDemo_DisplayForm.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeDemo_DisplayForm.png)

> All Items listing

> ![https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeDemo_AllItems.png](https://sharepoint-treeview-column.googlecode.com/svn/wiki/Images/TreeDemo_AllItems.png)