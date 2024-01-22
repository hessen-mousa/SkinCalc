using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public record CalculationLogicResponse
    {
        public int NumberOfCases { get; set; }
        public double MoneyForNextCase { get; set; }

    }
}
