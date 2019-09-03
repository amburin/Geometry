using System;
using FluentAssertions;
using NGerasimov.Geometry.UnitTests;
using Xunit;

namespace NGerasimov.Geometry.UnitTests
{
    public class CircleTests
    {
        [Theory]
        [InlineData(-0.1)]
        [InlineData(0)]
        public void RadiusIsNotPositive_RoundIsNotCreated(double radius)
        {
            Action act = () => new Circle(radius);

            act.Should().Throw<ArgumentException>();
        }
        
        [Theory]
        [InlineData(1, Math.PI)]
        [InlineData(2, 4 * Math.PI)]
        public void CalculateArea_ReturnsValidValue(double radius, double area)
        {
            var circle = new Circle(radius);

            var value = circle.CalculateArea();
            
            value.Should().Be(Math.Round(area, 2, MidpointRounding.AwayFromZero));
        }
    }
}