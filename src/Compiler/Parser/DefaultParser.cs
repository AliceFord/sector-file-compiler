﻿namespace Compiler.Parser
{
    /**
     * A default parser for data that can only parse comments and
     * blank lines.
     */
    public class DefaultParser : AbstractSectorElementParser
    {
        public DefaultParser(MetadataParser metadata) : base(metadata)
        {

        }

        public override void ParseData(SectorFormatData data)
        {
            foreach (string line in data.lines)
            {
                this.ParseMetadata(line);
            }
        }
    }
}