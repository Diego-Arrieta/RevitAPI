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

namespace RevitAPI3.Extensions
{
    public static class DocumentExtensions
    {
        public static void Run(this Document doc, Action action)
        {
            using (Transaction tr = new Transaction(doc, "Transaction"))
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
        public static void CreatePoint(this Document doc, int x = 0, int y = 0, int z = 0)
        {
            using (Transaction tr = new Transaction(doc, "Create geometry"))
            {
                tr.Start();

                Point pnt = Point.Create(new XYZ(x, y, z));
                DirectShape ds = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
                ds.SetShape(new List<GeometryObject> { pnt });

                tr.Commit();
            }
            return;
        }
        public static void CreatePoint(this Document doc, XYZ xyzPnt)
        {
            using (Transaction tr = new Transaction(doc, "Create point geometry"))
            {
                tr.Start();

                Point pnt = Point.Create(xyzPnt);
                DirectShape ds = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
                ds.SetShape(new List<GeometryObject> { pnt });

                tr.Commit();
            }
            return;
        }
        public static void CreateLine(this Document doc, XYZ staPnt, XYZ endPnt)
        {
            using (Transaction tr = new Transaction(doc, "Create line geometry"))
            {
                tr.Start();

                Line ln = Line.CreateBound(staPnt, endPnt);
                DirectShape ds = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
                ds.SetShape(new List<GeometryObject> { ln });

                tr.Commit();
            }
            return;
        }
        public static DirectShape CreateDirectShape(
            this Document doc,
            GeometryObject geometryObject,
            BuiltInCategory builtInCategory = BuiltInCategory.OST_GenericModel)
        {
            var directShape = DirectShape.CreateElement(doc, new ElementId(builtInCategory));
            directShape.SetShape(new List<GeometryObject> { geometryObject });
            return directShape;
        }
        public static DirectShape CreateDirectShapes(
            this Document doc,
            IEnumerable<GeometryObject> geometryObjects,
            BuiltInCategory builtInCategory = BuiltInCategory.OST_GenericModel)
        {
            var directShapes = DirectShape.CreateElement(doc, new ElementId(builtInCategory));
            directShapes.SetShape(geometryObjects.ToList());
            return directShapes;
        }
        public static List<Level> GetLevels(this Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Cast<Level>()
                .ToList();
        }
        public static List<TElement> GetElementByTypes<TElement>(this Document doc, Func<TElement, bool> validate = null) where TElement : class
        {
            validate = validate ?? (e => true);
            var elements = new FilteredElementCollector(doc)
                .OfClass(typeof(TElement))
                .Cast<TElement>()
                .Where(e => validate(e))
                .ToList();
            return elements;
        }

    }
}
