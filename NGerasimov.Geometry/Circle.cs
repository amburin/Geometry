using System;

namespace NGerasimov.Geometry
{
    public class Circle : ICalculatableArea
    {
        private readonly double _radius;

        public Circle(double radius)
        {
            EnsureThatRadiusIsPositive(radius);
            _radius = radius;
        }
        
        private void EnsureThatRadiusIsPositive(double radiusValue)
        {
            if (radiusValue <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    $"Radius should be positive, but actual value is {radiusValue}");
            }
        }

        public double CalculateArea()
        {
            var preciseArea = Math.PI * Math.Pow(_radius, 2);
            return Math.Round(preciseArea, 2, MidpointRounding.AwayFromZero);
        }
    }
}