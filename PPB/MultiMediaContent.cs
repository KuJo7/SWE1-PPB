using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPB
{
    public class MultiMediaContent
    {
        public Guid ContentId { get; set; }
        public string Name { get; set; }
        string Filetype { get; set; }
        int Filesize { get; set; }
        string Title { get; set; }
        string Artist { get; set; }
        string Album { get; set; }
        int Rating { get; set; }
        string Genre { get; set; }
        int Length { get; set; }
        string Url { get; set; }
    }
}

