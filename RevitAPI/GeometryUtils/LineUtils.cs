using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAPI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI.GeometryUtils
{
    public static class LineUtils
    {
        public static void Visualize(this Line line, Document document)
        {
            document.CreateDirectShape(line);
        }
    }
}
