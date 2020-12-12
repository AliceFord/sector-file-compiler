﻿using System.Collections.Generic;
using Compiler.Input;

namespace Compiler.Config
{
    public class MiscConfigFileSections
    {
        public static readonly List<ConfigFileSection> configFileSections = new List<ConfigFileSection>
        {
            new ConfigFileSection("agreements", InputDataType.ESE_AGREEMENTS, "Agreements"),
            new ConfigFileSection("freetext", InputDataType.ESE_FREETEXT, "Freetext"),
            new ConfigFileSection("colours", InputDataType.SCT_COLOUR_DEFINITIONS, "Colour Definitions"),
            new ConfigFileSection("info", InputDataType.SCT_INFO),
            new ConfigFileSection("file_headers", InputDataType.FILE_HEADERS),
            new ConfigFileSection("pre_positions", InputDataType.ESE_PRE_POSITIONS),
            new ConfigFileSection("fixes", InputDataType.SCT_FIXES, null),
            new ConfigFileSection("ndbs", InputDataType.SCT_NDBS, null),
            new ConfigFileSection("vors", InputDataType.SCT_VORS, null),
            new ConfigFileSection("danger_areas", InputDataType.SCT_ARTCC, "Danger Areas"),
            new ConfigFileSection("artcc_low", InputDataType.SCT_ARTCC_LOW, "Low ARTCCs"),
            new ConfigFileSection("artcc_high", InputDataType.SCT_ARTCC_HIGH, "High ARTCCs"),
            new ConfigFileSection("lower_airways", InputDataType.SCT_LOWER_AIRWAYS, "Lower Airways"),
            new ConfigFileSection("upper_airways", InputDataType.SCT_UPPER_AIRWAYS, "Upper Airways"),
            new ConfigFileSection("sid_airspace", InputDataType.SCT_SIDS, "SID Airspace"),
            new ConfigFileSection("star_airspace", InputDataType.SCT_STARS, "STAR Airspace"),
            new ConfigFileSection("geo", InputDataType.SCT_GEO, "Geo"),
            new ConfigFileSection("regions", InputDataType.SCT_REGIONS, "Regions"),
        };

    }
}
