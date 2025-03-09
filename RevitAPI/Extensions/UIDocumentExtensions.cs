using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitAPI.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI
{
    public static class UIDocumentExtensions
    {
        /// <summary>
        /// This method select elements from the model
        /// </summary>
        /// <param name="uiDocument"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static List<Autodesk.Revit.DB.Element> PickElements(this Autodesk.Revit.UI.UIDocument uiDocument, string message = "Select multiple elements: ")
        {
            IList<Reference> refList = uiDocument.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, message);

            return refList.Select(x => uiDocument.Document.GetElement(x)).ToList();
        }
        public static Autodesk.Revit.DB.Element PickElement(this Autodesk.Revit.UI.UIDocument uiDocument, string message = "Select element: ")
        {
            Reference refe = uiDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, message);

            return uiDocument.Document.GetElement(refe);
        }
        public static T PickWrapper<T>(this Autodesk.Revit.UI.UIDocument uiDocument, string message = "Select an element") where T : class, IElementWrapper
        {
            Reference reference = uiDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, message);
            Autodesk.Revit.DB.Element element = uiDocument.Document.GetElement(reference);

            return Activator.CreateInstance(typeof(T), element) as T;
        }
        public static List<T> PickWrappers<T>(this Autodesk.Revit.UI.UIDocument uiDocument, string message = "Select elements") where T : class, IElementWrapper
        {
            List<Reference> references = uiDocument.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, message).ToList();

            return references
                .Select(r => uiDocument.Document.GetElement(r))
                .Select(element => Activator.CreateInstance(typeof(T), element) as T)
                .Where(wrapper => wrapper != null)
                .ToList();
        }
        public static List<Autodesk.Revit.DB.Element> GetSelectedElements(this Autodesk.Revit.UI.UIDocument uiDocument)
        {
            return uiDocument.Selection
                .GetElementIds()
                .Select(id => uiDocument.Document.GetElement(id))
                .ToList();
        }
    }
}
