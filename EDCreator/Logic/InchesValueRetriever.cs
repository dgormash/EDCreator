using System;

namespace FDCreator.Logic
{
    public static class InchesValueRetriever
    {
        public static float GetInchesValue(string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return Convert.ToSingle("0");
            }
            if (!stringValue.Contains("/"))
            {
                return Convert.ToSingle(stringValue);
            }
            var spaceSplitter = stringValue.Split(' ');
            var slashSplitter = spaceSplitter[1].Split('/');
            var a = Convert.ToInt32(spaceSplitter[0]);
            var b = Convert.ToSingle(slashSplitter[0]);
            var c = Convert.ToInt32(slashSplitter[1]);
            var result = a + b/c;
            return result;

        }
    }
}