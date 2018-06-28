using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf.parser;

namespace EDCreator.Logic
{
    class MyTextExtractionStrategy:ITextExtractionStrategy
    {
        
        public void BeginTextBlock()
        {
            
        }

        public void RenderText(TextRenderInfo renderInfo)
        {
            throw new NotImplementedException();
        }

        public void EndTextBlock()
        {
            throw new NotImplementedException();
        }

        public void RenderImage(ImageRenderInfo renderInfo)
        {
            throw new NotImplementedException();
        }

        public string GetResultantText()
        {
            throw new NotImplementedException();
        }
    }
}
