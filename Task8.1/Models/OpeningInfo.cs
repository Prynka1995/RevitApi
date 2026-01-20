using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task8.Models
{
    public class OpeningInfo
    {
        public string FamilyName { get; set; }
        public string FamilyType { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Volume { get; set; }
        public double Area { get; set; }
        public bool IsCorrect{  get; set; }
    }
}
