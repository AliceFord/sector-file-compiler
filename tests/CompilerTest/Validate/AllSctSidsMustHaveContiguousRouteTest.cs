﻿using System.Collections.Generic;
using Xunit;
using Compiler.Model;
using Compiler.Error;
using Compiler.Event;
using Compiler.Validate;
using Moq;
using Compiler.Argument;

namespace CompilerTest.Validate
{
    public class AllSctSidsMustHaveContiguousRouteTest
    {
        private readonly SectorElementCollection sectorElements;
        private readonly Mock<IEventLogger> loggerMock;
        private readonly AllSctSidsMustHaveContiguousRoute rule;
        private readonly CompilerArguments args;

        public AllSctSidsMustHaveContiguousRouteTest()
        {
            this.sectorElements = new SectorElementCollection();
            this.loggerMock = new Mock<IEventLogger>();
            this.rule = new AllSctSidsMustHaveContiguousRoute();
            this.args = new CompilerArguments();
            this.args.EnforceContiguousRouteSegments = true;
        }

        [Fact]
        public void TestItPassesOnValidRoute()
        {
            List<RouteSegment> segments = new List<RouteSegment>
            {
                new RouteSegment(new Point("testfix"), new Point("testvor"), null),
                new RouteSegment(new Point("testvor"), new Point("testndb"), null),
                new RouteSegment(new Point("testndb"), new Point("testairport"), null),
                new RouteSegment(new Point("testairport"), new Point(new Coordinate("abc", "def")), null),
                new RouteSegment(new Point(new Coordinate("abc", "def")), new Point("testfix"), null),
            };
            SidStarRoute route = new SidStarRoute(
                SidStarType.SID,
                "EGKK TEST",
                segments
            );

            this.sectorElements.Add(route);
            this.rule.Validate(sectorElements, this.args, this.loggerMock.Object);

            this.loggerMock.Verify(foo => foo.AddEvent(It.IsAny<ValidationRuleFailure>()), Times.Never);
        }

        [Fact]
        public void TestItPassesOnSingleLineRoute()
        {
            List<RouteSegment> segments = new List<RouteSegment>
            {
                new RouteSegment(new Point("testfix"), new Point("testvor"), null),
            };
            SidStarRoute route = new SidStarRoute(
                SidStarType.SID,
                "EGKK TEST",
                segments
            );

            this.sectorElements.Add(route);
            this.rule.Validate(sectorElements, this.args, this.loggerMock.Object);

            this.loggerMock.Verify(foo => foo.AddEvent(It.IsAny<ValidationRuleFailure>()), Times.Never);
        }

        [Fact]
        public void TestItPassesOnSingleLineRouteStartingWithDefaultStarter()
        {
            List<RouteSegment> segments = new List<RouteSegment>
            {
                new RouteSegment(
                    new Point(new Coordinate("S999.00.00.000", "E999.00.00.000")),
                    new Point(new Coordinate("S999.00.00.000", "E999.00.00.000")),
                    null
                ),
                new RouteSegment(new Point("testfix"), new Point("testvor"), null),
            };
            SidStarRoute route = new SidStarRoute(
                SidStarType.SID,
                "EGKK TEST",
                segments
            );

            this.sectorElements.Add(route);
            this.rule.Validate(sectorElements, this.args, this.loggerMock.Object);

            this.loggerMock.Verify(foo => foo.AddEvent(It.IsAny<ValidationRuleFailure>()), Times.Never);
        }

        [Fact]
        public void TestItPassesOnMultiLineRouteStartingWithDefaultStarter()
        {
            List<RouteSegment> segments = new List<RouteSegment>
            {
                new RouteSegment(
                    new Point(new Coordinate("S999.00.00.000", "E999.00.00.000")),
                    new Point(new Coordinate("S999.00.00.000", "E999.00.00.000")),
                    null
                ),
                new RouteSegment(new Point("testfix"), new Point("testvor"), null),
                new RouteSegment(new Point("testvor"), new Point("testndb"), null),
            };
            SidStarRoute route = new SidStarRoute(
                SidStarType.SID,
                "EGKK TEST",
                segments
            );

            this.sectorElements.Add(route);
            this.rule.Validate(sectorElements, this.args, this.loggerMock.Object);

            this.loggerMock.Verify(foo => foo.AddEvent(It.IsAny<ValidationRuleFailure>()), Times.Never);
        }

        [Fact]
        public void TestItFailsOnBadRoute()
        {
            List<RouteSegment> segments = new List<RouteSegment>
            {
                new RouteSegment(new Point("testfix"), new Point("testvor"), null),
                new RouteSegment(new Point("testvor"), new Point("nottestndb"), null),
                new RouteSegment(new Point("testndb"), new Point("testairport"), null),
                new RouteSegment(new Point("testairport"), new Point(new Coordinate("abc", "def")), null),
                new RouteSegment(new Point(new Coordinate("abc", "def")), new Point("testfix"), null),
            };
            SidStarRoute route = new SidStarRoute(
                SidStarType.SID,
                "EGKK TEST",
                segments
            );
            this.sectorElements.Add(route);
            this.rule.Validate(sectorElements, this.args, this.loggerMock.Object);

            this.loggerMock.Verify(foo => foo.AddEvent(It.IsAny<ValidationRuleFailure>()), Times.Once);
        }

        [Fact]
        public void TestItAllowsBadRoutesToPassIfArgumentPassed()
        {
            this.args.EnforceContiguousRouteSegments = false;

            List<RouteSegment> segments = new List<RouteSegment>
            {
                new RouteSegment(new Point("testfix"), new Point("testvor"), null),
                new RouteSegment(new Point("testvor"), new Point("nottestndb"), null),
                new RouteSegment(new Point("testndb"), new Point("testairport"), null),
                new RouteSegment(new Point("testairport"), new Point(new Coordinate("abc", "def")), null),
                new RouteSegment(new Point(new Coordinate("abc", "def")), new Point("testfix"), null),
            };
            SidStarRoute route = new SidStarRoute(
                SidStarType.SID,
                "EGKK TEST",
                segments
            );
            this.sectorElements.Add(route);
            this.rule.Validate(sectorElements, this.args, this.loggerMock.Object);

            this.loggerMock.Verify(foo => foo.AddEvent(It.IsAny<ValidationRuleFailure>()), Times.Never);
        }
    }
}