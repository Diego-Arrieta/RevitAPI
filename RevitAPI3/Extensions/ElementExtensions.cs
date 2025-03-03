using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI3.Extensions
{
    public static class ElementExtensions
    {
        public static List<Solid> GetSolids(this Element element, Options options)
        {
            List<Solid> solids = new List<Solid>();

            if (element == null)
            {
                TaskDialog.Show("Error", "El elemento no puede ser nulo.");
                return solids;
            }           

            GeometryElement geometryElement = element.get_Geometry(options);
            if (geometryElement != null)
            {
                TaskDialog.Show("Información", "No se pudo obtener la geometría del elemento.");
                return solids;                
            }

            ProcessGeometry(geometryElement, solids);
            TaskDialog.Show("Resultado", $"Cantidad de sólidos obtenidos: {solids.Count}");

            return solids;
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
                    try
                    {
                        GeometryElement instanceGeometry = geometryInstance.GetInstanceGeometry();
                        if (instanceGeometry != null)
                        {
                            ProcessGeometry(instanceGeometry, solids);
                        }
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("Error", $"Error al procesar GeometryInstance: {ex.Message}");
                    }
                }
            }
        }
    }
}