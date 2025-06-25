using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmdletHelpViewer.Models
{
    public class CmdletParameter
    {
        public bool Required
        {
            get;
            set;
        }

        public bool VariableLength
        {
            get;
            set;
        }

        public bool Globbing
        {
            get;
            set;
        }

        public string PipeLineInput
        {
            get;
            set;
        }

        public string Position
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string ParameterType
        {
            get;
            set;
        }
        public override string ToString()
        {
            if (Required == true)
            {
                if (ParameterType == null)
                    return string.Format(" -{0}", Name);
                else
                    return string.Format(" -{0} <{1}>", Name, ParameterType);
            }
            else
                if (ParameterType == null)
                    return string.Format("[-{0}]", Name);
                else
                    return string.Format(" [-{0} <{1}>]", Name, ParameterType);
        }
    }
}

