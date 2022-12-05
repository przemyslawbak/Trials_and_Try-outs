using Microsoft.ML.Data;
using System;

namespace Financial_ML.Models
{
    public class TotalQuote
    {
        [LoadColumn(0)]
        public DateTime Date { get; set; }
        [LoadColumn(1)]
        public float SmaDax { get; set; }
        [LoadColumn(2)]
        public float SmaBrent { get; set; }
        [LoadColumn(3)]
        public float SmaDeltaDax { get; set; }
        [LoadColumn(4)]
        public float SmaDeltaBrent { get; set; }
        [LoadColumn(5)]
        public float CloseDax { get; set; }
        [LoadColumn(6)]
        public float CloseBrent { get; set; }
        [LoadColumn(7)]
        public float NextDayCloseDax { get; set; }
        public bool NextDayCloseDaxBoolean { get; set; }
    }
}
