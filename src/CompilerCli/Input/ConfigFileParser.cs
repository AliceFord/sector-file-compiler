﻿using System;
using System.Collections.Generic;
using Compiler.Argument;

namespace CompilerCli.Input
{
    public class ConfigFileParser : IInputParser
    {
        public CompilerArguments Parse(List<string>values, CompilerArguments compilerSettings)
        {
            if (values.Count != 1)
            {
                throw new ArgumentException("Config file path should have only one argument");
            }

            compilerSettings.ConfigFiles.Add(values[0]);
            return compilerSettings;
        }
    }
}
