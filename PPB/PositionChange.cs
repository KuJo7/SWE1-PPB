using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPB
{
    public class PositionChange
    {
        private int _fromPosition;
        private int _toPosition;
        public int FromPosition
        {
            get => _fromPosition - 1;
            set => _fromPosition = value;
        }
        public int ToPosition
        {
            get => _toPosition - 1;
            set => _toPosition = value;
        }
    }
}
