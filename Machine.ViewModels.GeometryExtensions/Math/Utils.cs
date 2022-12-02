using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.GeometryExtensions.Math
{
    internal static class Utils
    {
        public static bool IsZero(float a)
        {
            return  System.Math.Abs(a) < 1E-06f;
        }
    }
}
