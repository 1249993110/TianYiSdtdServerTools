using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.ObservableObjects
{
    public class PlayerInventoryData : ColoredImageData
    {
        public int? Count { get; set; }

        public string QualityColor { get; set; }
    }
}
