using System;
using FluentAssertions;
using Xunit;

namespace NGerasimov.Geometry.UnitTests
{
    public class TriangleTests
    {
        [Theory]
        [InlineData(0, 3, 4)]
        [InlineData(-3, 4, 2)]
        public void WhenSideLengthIsNotPositive_TriangleIsNotCreated(
            double firstSideLength,
            double secondSideLength,
            double thirdSideLength)
        {
            Action act = () => new Triangle(firstSideLength, secondSideLength, thirdSideLength);

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(1, 3, 4)]
        public void WhenOneSideIsNotSmallerThanSumOfOthers_TriangleIsNotCreated(
            double firstSideLength,
            double secondSideLength,
            double thirdSideLength)
        {
            Action act = () => new Triangle(firstSideLength, secondSideLength, thirdSideLength);

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(2, 3, 4)]
        public void WhenSidesAreValid_TriangleShouldBeCreated(
            double firstSideLength,
            double secondSideLength,
            double thirdSideLength)
        {
            Action act = () => new Triangle(firstSideLength, secondSideLength, thirdSideLength);

            act.Should().NotThrow<ArgumentException>();
        }

        [Theory]
        [InlineData(300000, 400000, 500000, 60000000000)]
        [InlineData(223, 160, 145, 11574.02)]
        [InlineData(0.3, 0.4, 0.5, 0.06)]
        public void CalculateArea_WorksCorrectly(
            double firstSideLength,
            double secondSideLength,
            double thirdSideLength,
            double expectedArea)
        {
            var triangle = new Triangle(firstSideLength, secondSideLength, thirdSideLength);

            var area = triangle.CalculateArea();

            area.Should().Be(expectedArea);
        }

        [Theory]
        [InlineData(300000, 400000, 500000, true)]
        [InlineData(223, 160, 145, false)]
        [InlineData(0.3, 0.4, 0.5, true)]
        public void IsRightAngled_WorksCorrectly(
            double firstSideLength,
            double secondSideLength,
            double thirdSideLength,
            bool result)
        {
            var triangle = new Triangle(firstSideLength, secondSideLength, thirdSideLength);

            var area = triangle.IsRightAngled();

            area.Should().Be(result);
        }
    }
}