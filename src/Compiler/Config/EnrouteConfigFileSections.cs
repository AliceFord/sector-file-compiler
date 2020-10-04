﻿using System;
using System.Collections.Generic;
using System.Text;
using Compiler.Input;

namespace Compiler.Config
{
    public class EnrouteConfigFileSections
    {
        public static readonly List<ConfigFileSection> configFileSections = new List<ConfigFileSection>
        {
            new ConfigFileSection("sector_lines", InputDataType.ESE_SECTORLINES),
            new ConfigFileSection("ownership", InputDataType.ESE_OWNERSHIP),
            new ConfigFileSection("positions", InputDataType.ESE_POSITIONS),
        };

    }
}
