using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using RevitAPI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI.GeometryUtils
{
    public static class PlaneUtils
    {
        public static void Visualize(this Autodesk.Revit.DB.Plane plane, Document document, int scale = 3)
        {
            XYZ planeOrigin = plane.Origin;
            XYZ upperRightCorner = planeOrigin + (plane.XVec * scale) + (plane.YVec * scale);
            XYZ upperLeftCorner = planeOrigin - (plane.XVec * scale) + (plane.YVec * scale);
            XYZ bottomRightCorner = planeOrigin + (plane.XVec * scale) - (plane.YVec * scale);
            XYZ bottomLeftCorner = planeOrigin - (plane.XVec * scale) - (plane.YVec * scale);

            List<GeometryObject> curves = new List<GeometryObject>();
            curves.Add(Line.CreateBound(upperRightCorner, upperLeftCorner));
            curves.Add(Line.CreateBound(upperLeftCorner, bottomLeftCorner));
            curves.Add(Line.CreateBound(bottomLeftCorner, bottomRightCorner));
            curves.Add(Line.CreateBound(bottomRightCorner, upperRightCorner));
            curves.Add(Line.CreateBound(planeOrigin, planeOrigin + plane.Normal));

            document.CreateDirectShapes(curves);
        }        
    }
}
