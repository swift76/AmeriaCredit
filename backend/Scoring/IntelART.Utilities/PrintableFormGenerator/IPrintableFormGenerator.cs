using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IntelART.Utilities.PrintableFormGenerator
{
    public interface IPrintableFormGenerator
    {
        Task<Stream> GnerateFormAsync(string formCode, Dictionary<string, string> values);
    }
}
