﻿using EngineeringMath.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.EngineeringModel
{
    public class BuiltFunction : IBuiltModel
    {
        public int Id { get; internal set; }

        public string Name { get; internal set; }

        public string OwnerName { get; internal set; }

        public IEnumerable<BuiltEquation> Equations { get; internal set; }

        public IEnumerable<BuiltParameter> Parameters { get; internal set; }

        public override bool Equals(object obj)
        {
            if (obj is BuiltFunction builtFunction)
            {
                return Id == builtFunction.Id;
            }
            else if (obj is Function function)
            {
                return Id == function.FunctionId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<int>.Default.GetHashCode(Id);
        }
    }
}
