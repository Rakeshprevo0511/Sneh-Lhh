using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class Diagnosis_Dll
    {
        public long DiagnosisID { get; set; }
        public string UniqueID { get; set; }
        public string dName { get; set; }
        public string dCode { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
    }
}
