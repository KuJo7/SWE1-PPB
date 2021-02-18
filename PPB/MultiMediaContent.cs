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
        public string Filetype { get; set; } = "";
        public int Filesize { get; set; } = 0;
        public string Title { get; set; } = "";
        public string Artist { get; set; } = "";
        public string Album { get; set; } = "";
        public int Rating { get; set; } = 0;
        public string Genre { get; set; }
        public string Length { get; set; } = "";
        public string Url { get; set; }
    }
}

