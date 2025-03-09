using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI.Wrappers
{
    public interface IElementWrapper
    {
        Autodesk.Revit.DB.Element GetElement();
    }
}
