﻿using Xunit;
using System.Collections.Generic;
using Compiler.Parser;
using Compiler.Model;

namespace CompilerTest.Parser
{
    public class CoordinateParserTest
    {
        [Fact]
        public void TestItParsesCoordinates()
        {
            Coordinate coordinate = CoordinateParser.Parse("N054.39.27.000", "W006.12.57.000");
            Assert.Equal("N054.39.27.000", coordinate.latitude);
            Assert.Equal("W006.12.57.000", coordinate.longitude);
        }

        [Fact]
        public void TestItAllowsOfScreenCoordinate()
        {
            Coordinate coordinate = CoordinateParser.Parse("S999.00.00.000", "E999.00.00.000");
            Assert.Equal("S999.00.00.000", coordinate.latitude);
            Assert.Equal("E999.00.00.000", coordinate.longitude);
        }

        public static IEnumerable<object[]> BadData => new List<object[]>
        {
            new object[] {"W054.39.27.000", "W006.12.57.000"}, // Invalid latitude north south
            new object[] {"N054.39.27", "W006.12.57.000"}, // Invalid latitude parts
            new object[] {"N05a.39.27.000", "W006.12.57.000"}, // Invalid latitude degrees
            new object[] {"N054.39a.27.000", "W006.12.57.000"}, // Invalid latitude minutes
            new object[] {"N054.39.27a.000", "W006.12.57.000"}, // Invalid latitude seconds
            new object[] {"N054.39.27.a2", "W006.12.57.000"}, // Invalid latitude fractions
            new object[] {"N054.39.27.000", "S006.12.57.000"}, // Invalid longitude north south
            new object[] {"N054.39.27.000", "S006.12.57.000"}, // Invalid longitude north south
            new object[] {"N054.39.27.000", "W006.57.000"}, // Invalid longitude parts
            new object[] {"N054.39.27.000", "W0a6.12.57.000"}, // Invalid longitude degrees
            new object[] {"N054.39.27.000", "W006.12a.57.000"}, // Invalid longitude minutes
            new object[] {"N054.39.27.000", "W006.12.57a.000"}, // Invalid longitude seconds
            new object[] {"N054.39.27.000", "W006.12.57.0a0"}, // Invalid longitude fractions
            new object[] {"N090.00.00.001", "W006.12.57.000"}, // 90 degrees latitude, with fractions
            new object[] {"N090.00.01.000", "W006.12.57.000"}, // 90 degrees latitude, with seconds
            new object[] {"N090.01.00.000", "W006.12.57.000"}, // 90 degrees latitude, with minutes
            new object[] {"N091.00.00.000", "W006.12.57.000"}, // More than 90 degrees latitude
            new object[] {"N070.00.60.001", "W006.12.57.000"}, // More than 60 seconds latitude
            new object[] {"N070.61.00.000", "W006.12.57.000"}, // More than 60 minutes latitude
            new object[] {"N054.39.27.000", "W180.00.00.001"}, // 180 degrees longitude, with fractions
            new object[] {"N054.39.27.000", "W180.00.01.000"}, // 180 degrees longitude, with seconds
            new object[] {"N054.39.27.000", "W180.01.00.000"}, // 180 degrees longitude, with minutes
            new object[] {"N054.39.27.000", "W181.00.00.000"}, // More than 180 degrees longitude
            new object[] {"N054.39.27.000", "W171.00.60.001"}, // More than 60 seconds longitude
            new object[] {"N054.39.27.000", "W171.61.00.000"}, // More than 60 minutes longitude

        };

        [Theory]
        [MemberData(nameof(BadData))]
        public void ItReturnsInvalidOnBadData(string latitude, string longitude)
        {
            Coordinate coordinate = CoordinateParser.Parse(latitude, longitude);
            Assert.Equal(CoordinateParser.invalidCoordinate, coordinate);
        }
    }
}
