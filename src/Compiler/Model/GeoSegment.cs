﻿namespace Compiler.Model
{
    public class GeoSegment : AbstractCompilableElement
    {
        public GeoSegment(
            Point firstPoint,
            Point secondPoint,
            string colour,
            Definition definition,
            Docblock docblock,
            Comment comment

        ) : base(definition, docblock, comment)
        {
            FirstPoint = firstPoint;
            SecondPoint = secondPoint;
            Colour = colour;
        }

        public Point FirstPoint { get; }
        public Point SecondPoint { get; }
        public string Colour { get; }

        public override string GetCompileData()
        {
            return string.Format(
                "{0} {1} {2}",
                this.FirstPoint.ToString(),
                this.SecondPoint.ToString(),
                this.Colour
            );
        }
    }
}
