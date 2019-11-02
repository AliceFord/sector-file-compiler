﻿using System.Collections.Generic;
using Xunit;
using Compiler.Model;
using Compiler.Event;
using Compiler.Validate;
using Moq;

namespace CompilerTest.Validate
{
    public class AllSidsMustBeUniqueTest
    {
        private readonly SectorElementCollection sectorElements;
        private readonly Mock<IEventLogger> loggerMock;
        private readonly SidStar first;
        private readonly SidStar second;
        private readonly SidStar third;
        private readonly SidStar fourth;
        private readonly AllSidsMustBeUnique rule;

        public AllSidsMustBeUniqueTest()
        {
            this.sectorElements = new SectorElementCollection();
            this.loggerMock = new Mock<IEventLogger>();
            this.first = new SidStar("SID", "EGKK", "26L", "ADMAG2X", new List<string>());
            this.second = new SidStar("STAR", "EGKK", "26L", "ADMAG2X", new List<string>());
            this.third = new SidStar("SID", "EGKK", "26L", "ADMAG2X", new List<string>());
            this.fourth = new SidStar("SID", "EGKK", "26L", "ADMAG2X", new List<string>(new string[] { "a" }));
            this.rule = new AllSidsMustBeUnique();
        }

        [Fact]
        public void TestItPassesIfNoDuplicates()
        {
            this.sectorElements.AddSidStar(this.first);
            this.sectorElements.AddSidStar(this.second);
            this.rule.Validate(sectorElements, this.loggerMock.Object);

            this.loggerMock.Verify(foo => foo.AddEvent(It.IsAny<ICompilerEvent>()), Times.Never);

        }

        [Fact]
        public void TestItPassesOnDifferentRoutes()
        {
            this.sectorElements.AddSidStar(this.first);
            this.sectorElements.AddSidStar(this.fourth);
            this.rule.Validate(sectorElements, this.loggerMock.Object);
            this.loggerMock.Verify(foo => foo.AddEvent(It.IsAny<ICompilerEvent>()), Times.Never);
        }

        [Fact]
        public void TestItFailsIfThereAreDuplicates()
        {
            this.sectorElements.AddSidStar(this.first);
            this.sectorElements.AddSidStar(this.second);
            this.sectorElements.AddSidStar(this.third);
            this.rule.Validate(sectorElements, this.loggerMock.Object);
            this.loggerMock.Verify(foo => foo.AddEvent(It.IsAny<ICompilerEvent>()), Times.Once);
        }
    }
}