﻿using System.Collections.Generic;
using Compiler.Model;
using Compiler.Event;

namespace Compiler.Validate
{
    public class OutputValidator
    {
        private static readonly List<IValidationRule> validationRules = new List<IValidationRule>
        {
            new AllAirportsMustHaveUniqueCode(),
            new AllColoursMustBeValid(),
            new AllColoursMustHaveAUniqueId(),
            new AllSidsMustBeUnique(),
            new AllSidsMustHaveAValidAirport(),
        };

        public static void Validate(SectorElementCollection sectorElements, IEventLogger events)
        {
            foreach (IValidationRule rule in OutputValidator.validationRules)
            {
                rule.Validate(sectorElements, events);
            }
        }
    }
}
