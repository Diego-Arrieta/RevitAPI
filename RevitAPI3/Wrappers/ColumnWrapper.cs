using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using RevitAPI3.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI3.Wrappers
{
    public class ColumnWrapper: IElementWrapper
    {
        private readonly FamilyInstance column;
        private readonly Document doc;
        public ColumnWrapper(FamilyInstance column)
        {
            this.column = column;
            this.doc = column.Document;
        }
        public Element GetElement()
        {
            return column;
        }
        public double GetZTop()
        {
            ElementId topLevelId = column.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).AsElementId();
            Level topLevel = doc.GetElement(topLevelId) as Level;
            double topElevation = topLevel.Elevation;

            double topOffset = column.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM).AsDouble();
            
            return topElevation + topOffset;
        }
        public double GetZBottom()
        {
            ElementId baseLevelId = column.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).AsElementId();
            Level baselevel = doc.GetElement(baseLevelId) as Level;
            double baseElevation = baselevel.Elevation;

            double baseOffset = column.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).AsDouble();

            return baseElevation + baseOffset;
        }
        public List<Level> GetIntersectingLevels()
        {
            List<Level> levels = doc.GetLevels();

            return levels
                    .Where(l => l.Elevation >= GetZBottom() && l.Elevation <= GetZTop())
                    .OrderBy(l => l.Elevation)
                    .ToList();
        }
        public void CutInstance()
        {
            LocationPoint columnLocation = column.Location as LocationPoint;
            if (columnLocation == null) throw new InvalidOperationException("Cannot get location.");

            XYZ position = columnLocation.Point;

            List<Level> intersectingLevels = GetIntersectingLevels();

            if (this.GetZBottom() < intersectingLevels.First().Elevation)
            {
                Level startLevel = intersectingLevels.First();
                double offset = this.GetZBottom() - startLevel.Elevation;

                FamilyInstance startColumn = doc
                    .Create.NewFamilyInstance(
                        position,
                        column.Symbol,
                        startLevel,
                        StructuralType.NonStructural);

                startColumn.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).Set(startLevel.Id);
                startColumn.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).Set(offset);
                startColumn.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).Set(startLevel.Id);
                startColumn.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM).Set(0.0);
            }
            for (int i = 0; i < intersectingLevels.Count - 1; i++)
            {
                Level baseLevel = intersectingLevels[i];
                Level topLevel = intersectingLevels[i + 1];

                FamilyInstance newColumn = doc
                    .Create.NewFamilyInstance(
                        position, 
                        column.Symbol, 
                        baseLevel, 
                        StructuralType.NonStructural);

                newColumn.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).Set(baseLevel.Id);
                newColumn.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).Set(0.0);
                newColumn.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).Set(topLevel.Id);
                newColumn.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM).Set(0.0);

            }
            if (this.GetZTop() > intersectingLevels.Last().Elevation)
            {
                Level endLevel = intersectingLevels.Last();
                double offset = this.GetZTop() - endLevel.Elevation;

                FamilyInstance endColumn = doc
                    .Create.NewFamilyInstance(
                        position,
                        column.Symbol,
                        endLevel,
                        StructuralType.NonStructural);

                endColumn.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).Set(endLevel.Id);
                endColumn.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).Set(0.0);
                endColumn.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).Set(endLevel.Id);
                endColumn.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM).Set(offset);                               
            }
            doc.Delete(column.Id);
        }
    }
}
