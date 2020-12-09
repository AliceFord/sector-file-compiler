﻿using Compiler.Event;
using System;

namespace Compiler.Error
{
    public class ConfigFileValidationError : ICompilerEvent
    {
        private readonly string exceptionMessage;

        public ConfigFileValidationError(string exceptionMessage)
        {
            this.exceptionMessage = exceptionMessage;
        }

        public string GetMessage()
        {
            return String.Format("Invalid compiler configuration: {0}", exceptionMessage);
        }

        public bool IsFatal()
        {
            return true;
        }
    }
}
