﻿using System.Collections.Generic;
using Xunit;
using Moq;
using Compiler.Parser;
using Compiler.Error;
using Compiler.Model;
using Compiler.Event;
using Compiler.Input;
using Compiler.Output;
using CompilerTest.Mock;

namespace CompilerTest.Parser
{
    public class LabelParserTest: AbstractParserTestCase
    {
        [Fact]
        public void TestItRaisesASyntaxErrorIfIncorrectNumberOfSegments()
        {
            this.RunParserOnLines(new List<string>(new[] { "\"test label\" N050.57.00.000 W001.21.24.490" }));

            Assert.Empty(this.sectorElementCollection.Labels);
            this.logger.Verify(foo => foo.AddEvent(It.IsAny<SyntaxError>()), Times.Once);
        }

        [Fact]
        public void TestItRaisesASyntaxErrorIfCoordinateNotvalid()
        {
            this.RunParserOnLines(new List<string>(new[] { "\"test label\" N050.57.00.000 N001.21.24.490 red" }));
            
            Assert.Empty(this.sectorElementCollection.Labels);
            this.logger.Verify(foo => foo.AddEvent(It.IsAny<SyntaxError>()), Times.Once);
        }

        [Fact]
        public void TestItAddsLabelData()
        {
            this.RunParserOnLines(new List<string>(new[] { "\"test label\" N050.57.00.000 W001.21.24.490 red ;comment" }));

            Label result = this.sectorElementCollection.Labels[0];
            Assert.Equal("test label", result.Text);
            Assert.Equal(new Coordinate("N050.57.00.000", "W001.21.24.490"), result.Position);
            Assert.Equal("red", result.Colour);
            this.AssertExpectedMetadata(result);
        }

        protected override InputDataType GetInputDataType()
        {
            return InputDataType.SCT_LABELS;
        }
    }
}
