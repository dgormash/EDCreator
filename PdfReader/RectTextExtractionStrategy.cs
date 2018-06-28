using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf.parser;

namespace PdfReader
{
    public class RectTextExtractionStrategy: LocationTextExtractionStrategy
    {
        internal List<ChunksAndRects> Collection = new List<ChunksAndRects>();

        public override void RenderText(TextRenderInfo renderInfo)
        {
            base.RenderText(renderInfo);
            //Get the bounding box for the chunk of text
            var bottomLeft = renderInfo.GetDescentLine().GetStartPoint();
            var topRight = renderInfo.GetAscentLine().GetEndPoint();

            //Create a rectangle from it
            var rect = new iTextSharp.text.Rectangle(
                                                    bottomLeft[Vector.I1],
                                                    bottomLeft[Vector.I2],
                                                    topRight[Vector.I1],
                                                    topRight[Vector.I2]
                                                    );

            //Add this to our main collection
            Collection.Add(new ChunksAndRects(rect, renderInfo.GetText()));
        }
    }
}
