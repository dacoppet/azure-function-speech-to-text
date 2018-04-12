using System;
using System.Collections.Generic;
using System.Text;

namespace AudioToText.Model
{
    public class ItemSTT
    {
        public string FileID { get; set; }
        public string CapturedText { get; set; }
        public double Confidence { get; set; }
    }
}
