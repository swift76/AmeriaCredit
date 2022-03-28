using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IntelART.Utilities.PrintableFormGenerator
{
    public class SimplePrintableFormGenerator : IPrintableFormGenerator
    {
        private string fileTemplatePath;
        private Dictionary<string, string> formCodeToTemplateMapping;

        public SimplePrintableFormGenerator(string fileTemplatePath, Dictionary<string, string> formCodeToTemplateMapping)
        {
            this.fileTemplatePath = fileTemplatePath;
            if (!string.IsNullOrEmpty(this.fileTemplatePath))
            {
                this.fileTemplatePath = this.fileTemplatePath.Trim();
                if (this.fileTemplatePath.EndsWith("/")
                    && this.fileTemplatePath.EndsWith("\\"))
                {
                    this.fileTemplatePath = this.fileTemplatePath.Substring(0, this.fileTemplatePath.Length - 1);
                }
                this.fileTemplatePath = string.Format("{0}\\", this.fileTemplatePath);
            }

            this.formCodeToTemplateMapping = formCodeToTemplateMapping;
        }

        public async Task<Stream> GnerateFormAsync(string formCode, Dictionary<string, string> values)
        {
            Stream result;
            string templateFile;

            if (!this.formCodeToTemplateMapping.TryGetValue(formCode, out templateFile))
            {
                throw new ApplicationException("E-4001", string.Format("Unknown form with code {0}", formCode));
            }
            try
            {
                using (FileStream fs = new FileStream(string.Format("{0}//{1}", this.fileTemplatePath, templateFile), FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string template = sr.ReadToEnd();
                        foreach (KeyValuePair<string, string> fieldValue in values)
                        {
                            string value = fieldValue.Value;
                            if (string.IsNullOrWhiteSpace(value))
                            {
                                value = "&nbsp;";
                            }
                            template = template.Replace(string.Format("{{{0}}}", fieldValue.Key), value);
                        }
                        result = new MemoryStream();
                        StreamWriter writer = new StreamWriter(result);
                        writer.Write(template);
                        writer.Flush();
                        result.Position = 0;
                    }
                }
            }
            catch(Exception e)
            {
                throw new ApplicationException("E-4002", string.Format("Error occured while reading the template file {0} for form {1}", templateFile, formCode));
            }

            return result;
        }
    }
}
