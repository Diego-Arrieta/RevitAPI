using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI3.Wrappers
{
    public interface IElementWrapper
    {
        Element GetElement();
    }
}
