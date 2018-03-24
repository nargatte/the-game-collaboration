using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Components.Fields
{
    public class HasCoordinates : IHasCoordinates
    {
        public uint X { get; }

        public uint Y { get; }
        public HasCoordinates(uint X, uint Y)
        {
            X = X;
            Y = Y;
        }
    }
}
