using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.Creation;
using System.Xml.Linq;

namespace RevitAPI.Extensions
{
    public static class DocumentExtensions
    {
        public static void Run(this Autodesk.Revit.DB.Document document, Action action)
        {
            using (Autodesk.Revit.DB.Transaction tr = new Autodesk.Revit.DB.Transaction(document, "Transaction"))
            {
                try
                {
                    tr.Start();
                    action.Invoke();
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.RollBack();
                    TaskDialog.Show("Revit", $"Error: {ex.Message}");
                }
            }
            return;
        }
        public static void CreatePoint(this Autodesk.Revit.DB.Document document, int x = 0, int y = 0, int z = 0)
        {
            document.Run(() =>
            {
                Point pnt = Point.Create(new XYZ(x, y, z));
                DirectShape ds = DirectShape.CreateElement(document, new Autodesk.Revit.DB.ElementId(BuiltInCategory.OST_GenericModel));
                ds.SetShape(new List<GeometryObject> { pnt });
            });
            return;
        }
        public static void CreatePoint(this Autodesk.Revit.DB.Document document, XYZ xyzPnt)
        {
            document.Run(() =>
            {
                Point pnt = Point.Create(xyzPnt);
                DirectShape ds = DirectShape.CreateElement(document, new Autodesk.Revit.DB.ElementId(BuiltInCategory.OST_GenericModel));
                ds.SetShape(new List<GeometryObject> { pnt });
            });
            return;
        }
        public static void CreateLine(this Autodesk.Revit.DB.Document document, XYZ staPnt, XYZ endPnt)
        {
            document.Run(() =>
            {
                Line ln = Line.CreateBound(staPnt, endPnt);
                DirectShape ds = DirectShape.CreateElement(document, new Autodesk.Revit.DB.ElementId(BuiltInCategory.OST_GenericModel));
                ds.SetShape(new List<GeometryObject> { ln });
            });
            return;
        }
        public static DirectShape CreateDirectShape(
            this Autodesk.Revit.DB.Document document,
            GeometryObject geometryObject,
            BuiltInCategory builtInCategory = BuiltInCategory.OST_GenericModel)
        {
            var directShape = DirectShape.CreateElement(document, new Autodesk.Revit.DB.ElementId(builtInCategory));
            directShape.SetShape(new List<GeometryObject> { geometryObject });
            return directShape;
        }
        public static DirectShape CreateDirectShapes(
            this Autodesk.Revit.DB.Document doc,
            IEnumerable<GeometryObject> geometryObjects,
            BuiltInCategory builtInCategory = BuiltInCategory.OST_GenericModel)
        {
            var directShapes = DirectShape.CreateElement(doc, new Autodesk.Revit.DB.ElementId(builtInCategory));
            directShapes.SetShape(geometryObjects.ToList());
            return directShapes;
        }
        public static List<Level> GetLevels(this Autodesk.Revit.DB.Document document)
        {
            return new FilteredElementCollector(document)
                .OfClass(typeof(Level))
                .Cast<Level>()
                .ToList();
        }
        public static List<TElement> GetElementByTypes<TElement>(this Autodesk.Revit.DB.Document document, Func<TElement, bool> validate = null) where TElement : class
        {
            validate = validate ?? (e => true);
            var elements = new FilteredElementCollector(document)
                .OfClass(typeof(TElement))
                .Cast<TElement>()
                .Where(e => validate(e))
                .ToList();
            return elements;
        }
    }
}
