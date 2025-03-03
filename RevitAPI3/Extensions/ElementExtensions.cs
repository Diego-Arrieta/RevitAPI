using Autodesk.Revit.DB;
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

            GeometryElement geometryElement = element.get_Geometry(options);
            if (geometryElement != null)
            {
                ProcessGeometry(geometryElement, solids);
            }           

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