using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace CmdletHelpViewer.Models
{
    public class CmdletCommand
    {
        public string Name
        {
            get;
            set;
        }

        public string ShortDescription
        {
            get;
            set;
        }

        public string DetailDescription
        {
            get;
            set;
        }

        public Collection<CmdletParameter> Parameters
        {
            get;
            set;
        }

        public Collection<CmdletExample> Examples
        {
            get;
            set;
        }

        public string Syntax
        {
            get
            {
                StringBuilder syntaxStringBuilder = new StringBuilder(Name);
                if (Parameters != null)
                {
                    foreach (CmdletParameter cmdletParameter in Parameters)
                    {
                        syntaxStringBuilder.AppendFormat("  {0}", cmdletParameter.ToString());
                    }
                }
                return syntaxStringBuilder.ToString();
            }
        }
    }
}

