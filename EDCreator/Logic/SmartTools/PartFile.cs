using System.Collections;
using System.Collections.Generic;

namespace FDCreator.Logic.SmartTools
{
    public class PartFile:IEnumerable

    {
        public SmartToolPartType Type { get; set; }
        public string File { get; set; }
        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}