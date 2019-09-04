using System;
using System.Linq;

namespace NGerasimov.Geometry
{
    public class Triangle : ICalculatableArea
    {
        private double _biggestSideLength;
        private double _mediumSideLength;
        private double _lowestSideLength;

        public Triangle(double sideOneLength, double sideTwoLength, double sideThreeLength)
        {
            EnsureThatSideLengthIsPositive(sideOneLength, nameof(sideOneLength));
            EnsureThatSideLengthIsPositive(sideTwoLength, nameof(sideTwoLength));
            EnsureThatSideLengthIsPositive(sideThreeLength, nameof(sideThreeLength));

            InitializeSideLengths(sideOneLength, sideTwoLength, sideThreeLength);
            EnsureThatTriangleExists();
        }

        public double CalculateArea()
        {
            var semiPerimeter = (_biggestSideLength + _lowestSideLength + _mediumSideLength) / 2;

            var preciseResult = Math.Sqrt(
                semiPerimeter *
                (semiPerimeter - _biggestSideLength) *
                (semiPerimeter - _mediumSideLength) *
                (semiPerimeter - _lowestSideLength));

            if (double.IsInfinity(preciseResult))
            {
                throw new ArgumentException("Side lengths are too big");
            }

            return Math.Round(preciseResult, 2, MidpointRounding.AwayFromZero);
        }

        public bool IsRightAngled()
        {
            return Math.Pow(_lowestSideLength, 2) + Math.Pow(_mediumSideLength, 2) ==
                   Math.Pow(_biggestSideLength, 2);
        }

        private void InitializeSideLengths(
            double sideOneLength,
            double sideTwoLength,
            double sideThreeLength)
        {
            var sides = new[] {sideOneLength, sideTwoLength, sideThreeLength}
                .OrderBy(x => x)
                .ToArray();

            _lowestSideLength = sides[0];
            _mediumSideLength = sides[1];
            _biggestSideLength = sides[2];
        }

        private void EnsureThatTriangleExists()
        {
            if (_biggestSideLength >= _lowestSideLength + _mediumSideLength)
            {
                throw new ArgumentException(
                    "Triangle with these sides will never exist " +
                    $"because biggest side length ({_biggestSideLength}) not less " +
                    $"than sum of lowest ({_lowestSideLength}) and medium ({_mediumSideLength}) sides)");
            }
        }

        private void EnsureThatSideLengthIsPositive(double lengthValue, string argumentName)
        {
            if (lengthValue <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    argumentName,
                    $"{argumentName} should be positive, but actual value is {lengthValue}");
            }
        }
    }
}