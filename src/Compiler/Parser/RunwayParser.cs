﻿using System.Collections.Generic;
using Compiler.Model;
using Compiler.Error;
using Compiler.Event;
using Compiler.Input;
using Compiler.Validate;
using System.Linq;

namespace Compiler.Parser
{
    public class RunwayParser: AbstractSectorElementParser, ISectorDataParser
    {
        private readonly ISectorLineParser sectorLineParser;
        private readonly SectorElementCollection sectorElements;
        private readonly IEventLogger errorLog;

        public RunwayParser(
            MetadataParser metadataParser,
            ISectorLineParser sectorLineParser,
            SectorElementCollection sectorElements,
            IEventLogger errorLog
        ) : base(metadataParser)
        {
            this.sectorLineParser = sectorLineParser;
            this.sectorElements = sectorElements;
            this.errorLog = errorLog;
        }

        public void ParseData(AbstractSectorDataFile data)
        {
            foreach (string line in data)
            {
                // Defer all metadata lines to the base
                if (this.ParseMetadata(line))
                {
                    continue;
                }

                SectorFormatLine sectorData = this.sectorLineParser.ParseLine(line);
                if (sectorData.dataSegments.Count < 8)
                {
                    this.errorLog.AddEvent(
                        new SyntaxError("Too few RUNWAY segments", data.FullPath, data.CurrentLineNumber)
                    );
                    continue;
                }

                // Check the two identifiers
                if (!RunwayValidator.RunwayValidIncludingAdjacent(sectorData.dataSegments[0]))
                {
                    this.errorLog.AddEvent(
                        new SyntaxError("Invalid runway designator " + sectorData.dataSegments[0], data.FullPath, data.CurrentLineNumber)
                    );
                    continue;
                }

                if (!RunwayValidator.RunwayValidIncludingAdjacent(sectorData.dataSegments[1]))
                {
                    this.errorLog.AddEvent(
                        new SyntaxError("Invalid runway designator " + sectorData.dataSegments[1], data.FullPath, data.CurrentLineNumber)
                    );
                    continue;
                }

                // Check the two headings
                if (!this.HeadingIsValid(sectorData.dataSegments[2]))
                {
                    this.errorLog.AddEvent(
                        new SyntaxError("Invalid runway heading " + sectorData.dataSegments[2], data.FullPath, data.CurrentLineNumber)
                    );
                    continue;
                }

                if (!this.HeadingIsValid(sectorData.dataSegments[3]))
                {
                    this.errorLog.AddEvent(
                        new SyntaxError("Invalid runway heading " + sectorData.dataSegments[3], data.FullPath, data.CurrentLineNumber)
                    );
                    continue;
                }

                // Check the two coordinates
                Coordinate firstThreshold = CoordinateParser.Parse(sectorData.dataSegments[4], sectorData.dataSegments[5]);
                if (firstThreshold.Equals(CoordinateParser.invalidCoordinate))
                {
                    this.errorLog.AddEvent(
                        new SyntaxError("Invalid runway first threshold ", data.FullPath, data.CurrentLineNumber)
                    );
                    continue;
                }

                Coordinate reverseThreshold = CoordinateParser.Parse(sectorData.dataSegments[6], sectorData.dataSegments[7]);
                if (reverseThreshold.Equals(CoordinateParser.invalidCoordinate))
                {
                    this.errorLog.AddEvent(
                        new SyntaxError("Invalid runway reverse threshold ", data.FullPath, data.CurrentLineNumber)
                    );
                    continue;
                }

                // Compile together the airfield description
                string runwayDialogDescription = "";
                if (sectorData.dataSegments.Count > 8)
                {

                    runwayDialogDescription = string.Join(
                        " ", 
                        sectorData.dataSegments
                            .Skip(8)
                            .Take(sectorData.dataSegments.Count - 8)
                            .ToList()
                    );
                }


                // Add the element
                this.sectorElements.Add(
                    new Runway(
                        sectorData.dataSegments[0],
                        int.Parse(sectorData.dataSegments[2]),
                        firstThreshold,
                        sectorData.dataSegments[1],
                        int.Parse(sectorData.dataSegments[3]),
                        reverseThreshold,
                        runwayDialogDescription,
                        sectorData.comment
                    )
                );
            }
        }

        private bool HeadingIsValid(string heading)
        {
            return int.TryParse(heading, out int headingInt) &&
                headingInt >= 0 &&
                headingInt < 360;
        }
    }
}
