using EngineeringMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static EngineeringMath.Resources.LibraryResources;

namespace DataSetBuilder.SeedData
{
    public class FunctionSeed : SeedBase
    {
        public FunctionSeed(EngineeringMathDS ds) : base(ds)
        {
            NumType = GetParaTypeByName(nameof(Number));
            LengthCat = GetUnitCatByName(nameof(Length));
        }

        public override void SeedData()
        {
            AddUnitConverter();
        }

        private void AddUnitConverter()
        {
            var fun = Ds.Function.AddFunctionRow(
                GetCatByName(nameof(Utility)), nameof(UnitConverter), false);

            foreach (var cat in Ds.UnitCategory)
            {
                string inputName = $"{cat.Name}.{nameof(Input)}";
                string outputName = $"{cat.Name}.{nameof(Output)}";
                Ds.Equation.AddEquationRow(fun, $"${inputName}", outputName);
                Ds.Parameter.AddParameterRow(fun, NumType, LengthCat, inputName, outputName);
            }


        }

        private EngineeringMathDS.FunctionCategoryRow GetCatByName(string name)
        {
            return Ds.FunctionCategory.Where(x => x.Name == name).Single();
        }

        private EngineeringMathDS.ParameterTypeRow GetParaTypeByName(string name)
        {
            return Ds.ParameterType.Where(x => x.Name == name).Single();
        }

        private readonly EngineeringMathDS.ParameterTypeRow NumType;


        private EngineeringMathDS.UnitCategoryRow GetUnitCatByName(string name)
        {
            return Ds.UnitCategory.Where(x => x.Name == name).Single();
        }

        private readonly EngineeringMathDS.UnitCategoryRow LengthCat;


    }
}
