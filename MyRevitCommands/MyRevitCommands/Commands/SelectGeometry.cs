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
    public class SelectGeometry : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument and Document
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                //Pick Object
                Reference pickedObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                if (pickedObj != null)
                {
                    //Retrieve Element
                    ElementId eleId = pickedObj.ElementId;
                    Element ele = doc.GetElement(eleId);

                    //Get Geometry
                    Options gOptions = new Options();
                    gOptions.DetailLevel = ViewDetailLevel.Fine;
                    GeometryElement geom = ele.get_Geometry(gOptions);

                    //Traverse

                    foreach (GeometryObject gObj in geom)
                    {
                        Solid gSolid = gObj as Solid;
                        int faces = 0;
                        double area = 0.0;
                        foreach (Face gFace in gSolid.Faces)
                        {
                            area += gFace.Area;
                            faces++;
                        }

                        area = UnitUtils.ConvertFromInternalUnits(area, UnitTypeId.SquareMeters);

                        TaskDialog.Show("Geometry", string.Format("Number of Faces: {0}" + Environment.NewLine
                            + "Total Area: {1:F2}", faces, area));
                    }

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
