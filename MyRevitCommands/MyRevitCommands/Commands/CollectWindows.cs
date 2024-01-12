using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Collections;
using System.Collections.Generic;

namespace MyRevitCommands
{
    [TransactionAttribute(TransactionMode.ReadOnly)]
    public class CollectWindows : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument

            UIDocument uiDoc = commandData.Application.ActiveUIDocument;

            //Get Document

            Document doc = uiDoc.Document;

            //Create Filtered Element Collector

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            
            //Create Filter
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);


            //Apply Filter

            IList<Element> windows = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

            TaskDialog.Show("Windows", string.Format("{0} windows counted!", windows.Count));

            return Result.Succeeded;
        }
    }
}
