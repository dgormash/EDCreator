using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDCreator.Logic
{
    public static class LengthConverter
    {
        public static float InchesToMeters(float length)
        {
            return length/39.370f;
        }
    }
}
