﻿using Xunit;
using Compiler.Validate;
using CompilerTest.Bogus.Factory;

namespace CompilerTest.Validate
{
    public class AllCoordinationPointsMustHaveValidDepartureRunwaysTest: AbstractValidatorTestCase
    {
        public AllCoordinationPointsMustHaveValidDepartureRunwaysTest()
        {
            this.sectorElements.Add(AirportFactory.Make("EGKK"));
            this.sectorElements.Add(RunwayFactory.Make("EGKK", "26L", "09"));
            this.sectorElements.Add(AirportFactory.Make("EGLL"));
            this.sectorElements.Add(RunwayFactory.Make("EGLL", "09R", "09"));
            this.sectorElements.Add(RunwayFactory.Make("EGLL", "09L", "09"));
            this.sectorElements.Add(AirportFactory.Make("EGSS"));
        }

        [Theory]
        [InlineData("EGKK", "26L")]
        [InlineData("EGLL", "09R")]
        [InlineData("EGLL", "09L")]
        [InlineData("EGLL", "*")]
        [InlineData("EGSS", "*")]
        [InlineData("XXXX", "*")] // Airport doesnt exist but any runway
        [InlineData("*", "09R")] // Should never happen
        [InlineData("*", "*")]

        public void TestItPassesOnValidDepartureRunway(string airport, string runway)
        {
            this.sectorElements.Add(CoordinationPointFactory.MakeAirport(departureAirport: airport, departureRunway: runway));

            this.AssertNoValidationErrors();
        }

        [Theory]
        [InlineData("EGLL", "27R")]
        [InlineData("EGLL", "27L")]
        [InlineData("EGKK", "08R")]
        [InlineData("EGKK", "08L")]
        [InlineData("EGSS", "04")] // Doesn't have a runway
        public void TestItFailsOnInvalidDepartureRunway(string airport, string runway)
        {
            this.sectorElements.Add(CoordinationPointFactory.MakeAirport(departureAirport: airport, departureRunway: runway));
            
            this.AssertValidationErrors();
        }

        protected override IValidationRule GetValidationRule()
        {
            return new AllCoordinationPointsMustHaveValidDepartureRunways();
        }
    }
}