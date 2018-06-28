using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfReader
{
    public class ChunksAndRects
    {
        public iTextSharp.text.Rectangle Rect;
        public string Text;

        public ChunksAndRects(iTextSharp.text.Rectangle rect, string text)
        {
        
            Rect = rect;
            Text = text;
        }
    }
}
