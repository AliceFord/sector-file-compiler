﻿namespace Compiler.Model
{
    public class ActiveRunway : AbstractCompilableElement
    {
        public ActiveRunway(
            string identifier,
            string airfield,
            int mode,
            Definition definition,
            Docblock docblock,
            Comment inlineComment

        ) : base(definition, docblock, inlineComment)
        {
            Identifier = identifier;
            Airfield = airfield;
            Mode = mode;
        }

        public string Identifier { get; }
        public string Airfield { get; }
        
        /*
         * 0 for active for arrival, 1 for active for departure
         */
        public int Mode { get; }

        public override string GetCompileData(SectorElementCollection elements)
        {
            return $"ACTIVE_RUNWAY:{this.Airfield}:{this.Identifier}:{this.Mode}";
        }
    }
}