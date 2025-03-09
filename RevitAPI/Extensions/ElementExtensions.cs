using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RevitAPI.Extensions
{
    public static class ElementExtensions
    {
        public static List<Solid> GetSolids(this Autodesk.Revit.DB.Element element, Autodesk.Revit.DB.Options options)
        {
            Document document = element.Document;
            List<Solid> solids = new List<Solid>();

            if (element == null)
            {
                TaskDialog.Show("Error", "Element cannot be null.");
                return solids;
            }

            List<Autodesk.Revit.DB.Element> subComponents = new List<Autodesk.Revit.DB.Element>();
            GetSubComponents(element, subComponents);

            foreach (Autodesk.Revit.DB.Element subComponent in subComponents)
            {
                GeometryElement geometryElement = subComponent.get_Geometry(options);
                if (geometryElement == null)
                {
                    TaskDialog.Show("Error", $"Geometry cannot be null: {subComponent.Name}");                    
                    continue;
                }
                ProcessGeometry(geometryElement, solids);
            }
            TaskDialog.Show("Results", $"Number of Solids: {solids.Count}");

            return solids;
        }
        private static void GetSubComponents(Autodesk.Revit.DB.Element element, List<Autodesk.Revit.DB.Element> subComponents)
        {
            Document document = element.Document;
            subComponents.Add(element);

            if (element is FamilyInstance familyInstance)
            {
                List<Autodesk.Revit.DB.ElementId> subComponentIds = familyInstance.GetSubComponentIds().ToList();
                foreach (Autodesk.Revit.DB.ElementId subComponentId in subComponentIds)
                {
                    Autodesk.Revit.DB.Element subComponent = document.GetElement(subComponentId);
                    GetSubComponents(subComponent, subComponents);
                }
            }
        }
        private static void ProcessGeometry(GeometryElement geometryElement, List<Solid> solids)
        {
            foreach (GeometryObject geometryObject in geometryElement)
            {
                if (geometryObject is Solid solid && solid != null && solid.Volume > 0 && solid.Faces.Size > 0)
                {
                    solids.Add(solid);
                }
                else if (geometryObject is GeometryInstance geometryInstance)
                {
                    GeometryElement instanceGeometry = geometryInstance.GetInstanceGeometry();
                    if (instanceGeometry != null)
                    {
                        ProcessGeometry(instanceGeometry, solids);
                    }
                }
            }
        }
    }
}