﻿using Microsoft.ML.Data;
using System;

namespace chapter03_logistic_regression.ML.Objects
{
    public class FileInput
    {
        [LoadColumn(0)]
        public DateTime Date { get; set; }
        [LoadColumn(1)]
        public float SmaDax { get; set; }
        [LoadColumn(2)]
        public float SmaBrent { get; set; }
        [LoadColumn(3)]
        public bool SmaDeltaDax { get; set; }
        [LoadColumn(4)]
        public bool SmaDeltaBrent { get; set; }
        [LoadColumn(5)]
        public float CloseDax { get; set; }
        [LoadColumn(6)]
        public float CloseBrent { get; set; }
    }
}