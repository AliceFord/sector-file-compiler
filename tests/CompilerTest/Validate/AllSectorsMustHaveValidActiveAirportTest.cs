﻿using System.Collections.Generic;
using Xunit;
using Moq;
using Compiler.Model;
using Compiler.Validate;
using Compiler.Event;
using Compiler.Error;
using Compiler.Argument;

namespace CompilerTest.Validate
{
    public class AllSectorsMustHaveValidActiveAirportTest
    {
        private readonly SectorElementCollection elements;
        private readonly AllSectorsMustHaveValidActiveAirport validator;
        private readonly Mock<IEventLogger> loggerMock;
        private readonly CompilerArguments args;

        public AllSectorsMustHaveValidActiveAirportTest()
        {
            this.elements = new SectorElementCollection();
            this.loggerMock = new Mock<IEventLogger>();
            this.validator = new AllSectorsMustHaveValidActiveAirport();
            this.args = new CompilerArguments();

            this.elements.Add(new Airport("a", "EGKK", new Coordinate("a", "b"), "a", "c"));
            this.elements.Add(new Airport("a", "EGLL", new Coordinate("a", "b"), "a", "c"));
            this.elements.Add(new Airport("a", "EGCC", new Coordinate("a", "b"), "a", "c"));
        }

        [Theory]
        [InlineData("EGLL", "EGCC", "EGKK")]
        [InlineData("EGCC", "EGCC", "EGKK")]
        [InlineData("EGKK", "EGLL", "EGKK")]
        public void TestItPassesOnAllValid(string first, string second, string third)
        {
            this.elements.Add(
                new Sector(
                    "COOL1",
                    5000,
                    66000,
                    new SectorOwnerHierarchy(
                        new List<string>()
                        {
                            "LS", "LC"
                        },
                        ""
                    ),
                    new List<SectorAlternateOwnerHierarchy>()
                    {
                        new SectorAlternateOwnerHierarchy("ALT1", new List<string>(){"LON1", "LON2"}, ""),
                        new SectorAlternateOwnerHierarchy("ALT2", new List<string>(){"LON3", "LON4"}, ""),
                    },
                    new List<SectorActive>()
                    {
                        new SectorActive(first, "09R", ""),
                        new SectorActive(third, "05", ""),
                    },
                    new List<SectorGuest>()
                    {
                        new SectorGuest("LLN", "EGLL", "EGWU", ""),
                        new SectorGuest("LLS", "EGLL", "*", ""),
                    },
                    new SectorBorder(
                    new List<string>()
                        {
                            "ONE",
                            "TWO",
                        },
                        ""
                    ),
                    new SectorArrivalAirports(new List<string>() { "EGSS", "EGGW" }, ""),
                    new SectorDepartureAirports(new List<string>() { "EGLL", "EGWU" }, ""),
                    "comment"
                )
            );
            this.elements.Add(
                new Sector(
                    "COOL2",
                    5000,
                    66000,
                    new SectorOwnerHierarchy(
                        new List<string>()
                        {
                            "LS", "LC"
                        },
                        ""
                    ),
                    new List<SectorAlternateOwnerHierarchy>()
                    {
                        new SectorAlternateOwnerHierarchy("ALT1", new List<string>(){"LON1", "LON2"}, ""),
                        new SectorAlternateOwnerHierarchy("ALT2", new List<string>(){"LON3", "LON4"}, ""),
                    },
                    new List<SectorActive>()
                    {
                        new SectorActive(second, "09R", ""),
                        new SectorActive(third, "05", ""),
                    },
                    new List<SectorGuest>()
                    {
                        new SectorGuest("LLN", "EGLL", "EGWU", ""),
                        new SectorGuest("LLS", "EGLL", "*", ""),
                    },
                    new SectorBorder(
                    new List<string>()
                        {
                            "ONE",
                            "TWO",
                        },
                        ""
                    ),
                    new SectorArrivalAirports(new List<string>() { "EGSS", "EGGW" }, ""),
                    new SectorDepartureAirports(new List<string>() { "EGLL", "EGWU" }, ""),
                    "comment"
                )
            );

            this.validator.Validate(this.elements, this.args, this.loggerMock.Object);
            this.loggerMock.Verify(foo => foo.AddEvent(It.IsAny<ValidationRuleFailure>()), Times.Never);
        }

        [Theory]
        [InlineData("EGLL", "EGCC", "WHAT", 2)]
        [InlineData("WHAT", "EGCC", "EGKK", 1)]
        [InlineData("EGKK", "WHAT", "WHAT", 2)]
        [InlineData("WHAT", "WHAT", "WHAT", 2)]
        public void TestItFailsOnInvalid(string first, string second, string third, int timesCalled)
        {
            this.elements.Add(
                new Sector(
                    "COOL1",
                    5000,
                    66000,
                    new SectorOwnerHierarchy(
                        new List<string>()
                        {
                            "LS", "LC"
                        },
                        ""
                    ),
                    new List<SectorAlternateOwnerHierarchy>()
                    {
                        new SectorAlternateOwnerHierarchy("ALT1", new List<string>(){"LON1", "LON2"}, ""),
                        new SectorAlternateOwnerHierarchy("ALT2", new List<string>(){"LON3", "LON4"}, ""),
                    },
                    new List<SectorActive>()
                    {
                        new SectorActive(first, "09R", ""),
                        new SectorActive(third, "05", ""),
                    },
                    new List<SectorGuest>()
                    {
                        new SectorGuest("LLN", "EGLL", "EGWU", ""),
                        new SectorGuest("LLS", "EGLL", "*", ""),
                    },
                    new SectorBorder(
                    new List<string>()
                        {
                            "ONE",
                            "TWO",
                        },
                        ""
                    ),
                    new SectorArrivalAirports(new List<string>() { "EGSS", "EGGW" }, ""),
                    new SectorDepartureAirports(new List<string>() { "EGLL", "EGWU" }, ""),
                    "comment"
                )
            );
            this.elements.Add(
                new Sector(
                    "COOL2",
                    5000,
                    66000,
                    new SectorOwnerHierarchy(
                        new List<string>()
                        {
                            "LS", "LC"
                        },
                        ""
                    ),
                    new List<SectorAlternateOwnerHierarchy>()
                    {
                        new SectorAlternateOwnerHierarchy("ALT1", new List<string>(){"LON1", "LON2"}, ""),
                        new SectorAlternateOwnerHierarchy("ALT2", new List<string>(){"LON3", "LON4"}, ""),
                    },
                    new List<SectorActive>()
                    {
                        new SectorActive(second, "09R", ""),
                        new SectorActive(third, "05", ""),
                    },
                    new List<SectorGuest>()
                    {
                        new SectorGuest("LLN", "EGLL", "EGWU", ""),
                        new SectorGuest("LLS", "EGLL", "*", ""),
                    },
                    new SectorBorder(
                    new List<string>()
                        {
                            "ONE",
                            "TWO",
                        },
                        ""
                    ),
                    new SectorArrivalAirports(new List<string>() { "EGSS", "EGGW" }, ""),
                    new SectorDepartureAirports(new List<string>() { "EGLL", "EGWU" }, ""),
                    "comment"
                )
            );

            this.validator.Validate(this.elements, this.args, this.loggerMock.Object);
            this.loggerMock.Verify(foo => foo.AddEvent(It.IsAny<ValidationRuleFailure>()), Times.Exactly(timesCalled));
        }
    }
}