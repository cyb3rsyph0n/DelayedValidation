using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayedValidation
{
    public interface IDraftable
    {
        bool isDraft { get; set; }
    }
}
