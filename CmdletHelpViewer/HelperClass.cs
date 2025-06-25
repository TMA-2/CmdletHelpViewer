using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using CmdletHelpViewer.Models;

namespace CmdletHelpViewer
{
    internal static class HelperClass
    {
        internal static void ParseXElement(XElement cmdRootXElement, CmdletCommand cmdletCommand)
        {
            foreach (XElement cmdParentXElement in cmdRootXElement.Elements())
            {
                switch (cmdParentXElement.Name.LocalName)
                {
                    case "details":
                        {
                            GetCmdDetail(cmdParentXElement, cmdletCommand);
                        }
                        break;
                    case "description":
                        {
                            cmdletCommand.DetailDescription = cmdParentXElement.Value;
                            if (!string.IsNullOrWhiteSpace(cmdletCommand.DetailDescription))
                            {
                                cmdletCommand.DetailDescription = cmdletCommand.DetailDescription.Trim();
                            }
                        }
                        break;
                    case "parameters":
                        {
                            GetCmdParameter(cmdParentXElement, cmdletCommand);
                        }
                        break;
                    case "examples":
                        {
                            GetCmdExample(cmdParentXElement, cmdletCommand);
                        }
                        break;
                }
            }
        }

        internal static void GetCmdDetail(XElement cmdParentXElement, CmdletCommand cmdletCommand)
        {
            foreach (XElement cmdChildXElement in cmdParentXElement.Elements())
            {
                switch (cmdChildXElement.Name.LocalName)
                {
                    case "name":
                        {
                            cmdletCommand.Name = cmdChildXElement.Value;
                            if (!string.IsNullOrWhiteSpace(cmdletCommand.Name))
                            {
                                cmdletCommand.Name = cmdletCommand.Name.Trim();
                            }
                        }
                        break;
                    case "description":
                        {
                            cmdletCommand.ShortDescription = cmdChildXElement.Value;
                            if (!string.IsNullOrWhiteSpace(cmdletCommand.ShortDescription))
                            {
                                cmdletCommand.ShortDescription = cmdletCommand.ShortDescription.Trim();
                            }
                        }
                        break;
                }
            }
        }
        
        internal static void GetCmdParameter(XElement cmdParentXElement, CmdletCommand cmdletCommand)
        {
            Collection<CmdletParameter> parametersCollection = new Collection<CmdletParameter>();
            foreach (XElement cmdChildXElement in cmdParentXElement.Elements())
            {
                CmdletParameter cmdletparameter = new CmdletParameter();
                foreach (XAttribute xParameters in cmdChildXElement.Attributes())
                {
                    if (xParameters.Name.LocalName == "required")
                    {
                        cmdletparameter.Required = Convert.ToBoolean(xParameters.Value);
                    }
                    if (xParameters.Name.LocalName == "variableLength")
                    {
                        cmdletparameter.VariableLength = Convert.ToBoolean(xParameters.Value);
                    }
                    if (xParameters.Name.LocalName == "globbing")
                    {
                        cmdletparameter.Globbing = Convert.ToBoolean(xParameters.Value);
                    }
                    if (xParameters.Name.LocalName == "pipelineInput")
                    {
                        cmdletparameter.PipeLineInput = xParameters.Value;
                        if (!string.IsNullOrWhiteSpace(cmdletparameter.PipeLineInput))
                        {
                            cmdletparameter.PipeLineInput = cmdletparameter.PipeLineInput.Trim();
                        }
                    }
                    if (xParameters.Name.LocalName == "position")
                    {
                        cmdletparameter.Position = xParameters.Value;
                        if (!string.IsNullOrWhiteSpace(cmdletparameter.Position))
                        {
                            cmdletparameter.Position = cmdletparameter.Position.Trim();
                        }
                    }
                }
                foreach (XElement xparametersXElement in cmdChildXElement.Elements())
                {
                    switch (xparametersXElement.Name.LocalName)
                    {
                        case "name":
                            {
                                cmdletparameter.Name = xparametersXElement.Value;
                                if (!string.IsNullOrWhiteSpace(cmdletparameter.Name))
                                {
                                    cmdletparameter.Name = cmdletparameter.Name.Trim();
                                }
                            }
                            break;
                        case "description":
                            {
                                cmdletparameter.Description = xparametersXElement.Value;
                                if (!string.IsNullOrWhiteSpace(cmdletparameter.Description))
                                {
                                    cmdletparameter.Description = cmdletparameter.Description.Trim();
                                }
                            }
                            break;
                        case "parameterValue":
                            {
                                cmdletparameter.ParameterType = xparametersXElement.Value;
                                if (!string.IsNullOrWhiteSpace(cmdletparameter.ParameterType))
                                {
                                    cmdletparameter.ParameterType = cmdletparameter.ParameterType.Trim();
                                }
                            }
                            break;
                    }
                }
                parametersCollection.Add(cmdletparameter);
            }
            cmdletCommand.Parameters = parametersCollection;
        }
        
        internal static void GetCmdExample(XElement cmdParentXElement, CmdletCommand cmdletCommand)
        {
            Collection<CmdletExample> examplescollection = new Collection<CmdletExample>();
            foreach (XElement cmdChildXElement in cmdParentXElement.Elements())
            {
                CmdletExample cmdletexample = new CmdletExample();
                foreach (XElement childXElement in cmdChildXElement.Elements())
                {
                    switch (childXElement.Name.LocalName)
                    {
                        case "title":
                            {
                                childXElement.Value = childXElement.Value.Replace("-", "");
                                childXElement.Value = childXElement.Value.Replace("\n", "");
                                cmdletexample.Title = childXElement.Value;
                                if (!string.IsNullOrWhiteSpace(cmdletexample.Title))
                                {
                                    cmdletexample.Title = cmdletexample.Title.Trim();
                                }
                            }
                            break;
                        case "code":
                            {
                                cmdletexample.Code = childXElement.Value;
                                if (!string.IsNullOrWhiteSpace(cmdletexample.Code))
                                {
                                    cmdletexample.Code = cmdletexample.Code.Trim();
                                }
                            }
                            break;
                        case "remarks":
                            {
                                cmdletexample.Remarks = childXElement.Value;
                                if (!string.IsNullOrWhiteSpace(cmdletexample.Remarks))
                                {
                                    cmdletexample.Remarks = cmdletexample.Remarks.Trim();
                                }
                            }
                            break;
                    }
                }
                examplescollection.Add(cmdletexample);
            }
            cmdletCommand.Examples = examplescollection;
        }
    }
}
