﻿using Compiler.Error;
using Compiler.Event;
using System.Collections.Generic;
using Newtonsoft.Json;
using Compiler.Input;

namespace Compiler.Argument
{
    public class CompilerArgumentsValidator
    {
        public static void Validate(IEventLogger events, CompilerArguments arguments)
        {
            if (arguments.ConfigFiles.Count == 0)
            {
                events.AddEvent(
                    new CompilerArgumentError("No config files specificed")
                );
            }

            foreach (IFileInterface configFile in arguments.ConfigFiles)
            {
                if (!configFile.Exists())
                {
                    events.AddEvent(
                        new CompilerArgumentError("The configuration file could not be found: " + configFile.GetPath())
                    );
                }
            }

            if (arguments.OutFileEse == null)
            {
                events.AddEvent(new CompilerArgumentError("ESE output file path must be specified"));
            }

            if (arguments.OutFileSct == null)
            {
                events.AddEvent(new CompilerArgumentError("SCT output file path must be specified"));
            }
        }
    }
}
