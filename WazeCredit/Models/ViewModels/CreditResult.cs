﻿using System;
using System.Collections.Generic;

namespace WazeCredit.Models.ViewModels
{
    public class CreditResult
    {
        public bool Success { get; set; }
        public IEnumerable<String> ErrorList { get; set; }
        public int CreditID { get; set; }
        public double CreditApproved { get; set; }
    }
}
