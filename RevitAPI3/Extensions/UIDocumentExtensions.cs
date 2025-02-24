using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitAPI3.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI3
{
    public static class UIDocumentExtensions
    {
        /// <summary>
        /// This method select elements from the model
        /// </summary>
        /// <param name="uidoc"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static List<Element> PickElements(this UIDocument uidoc, string message = "Select multiple elements: ")
        {
            IList<Reference> refList = uidoc.Selection.PickObjects(ObjectType.Element, message);

            return refList.Select(x => uidoc.Document.GetElement(x)).ToList();
        }
        public static Element PickElement(this UIDocument uidoc, string message = "Select element: ")
        {
            Reference refe = uidoc.Selection.PickObject(ObjectType.Element, message);

            return uidoc.Document.GetElement(refe);
        }
        public static T PickWrapper<T>(this UIDocument uidoc, string message = "Select an element") where T : class, IElementWrapper
        {
            Reference reference = uidoc.Selection.PickObject(ObjectType.Element, message);
            Element element = uidoc.Document.GetElement(reference);

            return Activator.CreateInstance(typeof(T), element) as T
                ?? throw new InvalidOperationException($"Cannot create an instances of {typeof(T).Name}.");
        }
        public static List<T> PickWrappers<T>(this UIDocument uidoc, string message = "Select elements") where T : class, IElementWrapper
        {
            List<Reference> references = uidoc.Selection.PickObjects(ObjectType.Element, message).ToList();

            return references
                .Select(r => uidoc.Document.GetElement(r))
                .Select(element => Activator.CreateInstance(typeof(T), element) as T)
                .Where(wrapper => wrapper != null)
                .ToList();
        }
        public static List<Element> GetSelectedElements(this UIDocument uidoc)
        {
            return uidoc.Selection
                .GetElementIds()
                .Select(id => uidoc.Document.GetElement(id))
                .ToList();
        }
    }
}
