using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [TransactionAttribute(TransactionMode.ReadOnly)]
    public class GetElementId : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //GetUIDocument

            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //GetDocument

            Document document = uidoc.Document;

            try
            {
                //Pick Object

                Reference pickedObject = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                //Retrieve Element
                ElementId elementId = pickedObject.ElementId;

                Element element = document.GetElement(elementId);

                //Get Element Type

                ElementId eTypeId = element.GetTypeId();

                ElementType eType = document.GetElement(eTypeId) as ElementType;

                //Display Element Id

                if (pickedObject != null)
                {
                    TaskDialog.Show("Element Classification", elementId.ToString() + Environment.NewLine
                        + "Category: " + element.Category.Name + Environment.NewLine
                        + "Instance: " + element.Name + Environment.NewLine
                        + "Symbol: " + eType.Name + Environment.NewLine
                        + "Family: " + eType.FamilyName);
                }

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
             
            }
            
            
        }
    }
}
