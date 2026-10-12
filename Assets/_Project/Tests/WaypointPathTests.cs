using NUnit.Framework;
using System.Collections.Generic;
using System.Numerics;
using TD.Rules;
using System;
using System.IO;
using NUnit.Compatibility;

namespace TD.Tests.EditMode
{
    public sealed class WaypointPathTests
    {
        private const float Tolerance = 0.0001f;
        private WaypointPath CreateLPath()
        {
            var points = new List<Vector2>
            {
              new Vector2(0,0),  
              new Vector2(3,0),  
              new Vector2(3,4),  
            };
            return new WaypointPath(points);
        }
        private void AssertVector(Vector2 expected, Vector2 actual)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(Tolerance));
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(Tolerance));
        }

        [Test]
        public void TotalLength_LSHape_IsSeven()
        {
            //Arrange
            WaypointPath path = CreateLPath();

            //Act
            float length = path.TotalLength;

            //Assert
            Assert.That(length,Is.EqualTo(7).Within(Tolerance));
        }

        [Test]
        public void Constructor_NullPoints_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new WaypointPath(null));
        }

        [Test]
        public void Constructor_OnePoint_ThrowsArgumentException()
        {
            var onePoint = new List<Vector2> {Vector2.One};
            Assert.Throws<ArgumentException>( () => new WaypointPath(onePoint));
        }

        [Test]
        public void Constructor_SourceListChangedAfterwards_PathUnchanged()
        {
            // Arrange
            var points = new List<Vector2> { new Vector2(0, 0), new Vector2(3, 0), new Vector2(3, 4) };

            WaypointPath path = new(points);

            // Act
            points[0] = new Vector2(100, 100);

            // Assert
            Assert.That(path.Waypoints[0], Is.EqualTo(new Vector2(0, 0)));
        }
    
        [Test]
        public void PositionAt_Five_ReturnsThreeTwo()
        {
            // Arrange
            WaypointPath path = CreateLPath();

            // Act
            Vector2 position = path.PositionAt(5);

            // Assert
            AssertVector(new Vector2 (3, 2), position);
        }   
        [Test]
        public void PositionAt_Zero_ReturnsFirstCorner()
        {
            // Arrange
            WaypointPath path = CreateLPath();

            // Act
            Vector2 position = path.PositionAt(0f);

            // Assert
            Assert.That(position, Is.EqualTo(new Vector2(0, 0)));
        }

        [Test]
        public void PositionAt_Negative_ClampsToStart()
        {
            // Arrange
            WaypointPath path = CreateLPath();

            // Act
            Vector2 point = path.PositionAt(-1);

            // Assert
            Assert.That(point, Is.EqualTo(new Vector2(0, 0)));
        }

        [Test]
        public void PositionAt_TotalLength_ReturnsLastCorner()
        {
            // Arrange
            WaypointPath path = CreateLPath();

            // Act
            Vector2 position = path.PositionAt(7f);

            // Assert
            AssertVector(new Vector2(3, 4), position);
        }

        [Test]
        public void PositionAt_BeyondEnd_ClampsToEnd()
        {
            // Arrange
            WaypointPath path = CreateLPath();

            // Act
            Vector2 position = path.PositionAt(100f);

            // Assert
            AssertVector(new Vector2(3, 4), position);
        }

        [Test]
        public void PositionAt_ZeroLengthSegment_ReturnsCornerNotNaN()
        {
            // Arrange: the first two corners sit on the same spot
            var points = new List<Vector2> { new Vector2(0, 0), new Vector2(0, 0), new Vector2(1, 0) };
            WaypointPath path = new(points);

            // Act
            Vector2 position = path.PositionAt(0f);

            // Assert
            AssertVector(new Vector2(0, 0), position);
        }

        [Test]
        public void DirectionAt_FirstSegment_PointsRight()
        {
            // Arrange
            WaypointPath path = CreateLPath();

            // Act
            Vector2 direction = path.DirectionAt(1);

            //Assert
            AssertVector(new Vector2(1, 0), direction);
        }

        [Test]
        public void HasReachedEnd_AtEnd_IsTrue()
        {
            // Arrange
            WaypointPath path = CreateLPath();

            // Act
            bool finished = path.HasReachedEnd(7);

            // Assert
            Assert.That(finished, Is.True);
        }

        [Test]
        public void DirectionAt_SecondSegment_PointsUp()
        {
            // Arrange
            WaypointPath path = CreateLPath();

            // Act
            Vector2 direction = path.DirectionAt(5);

            // Assert
            AssertVector(new Vector2(0, 1), direction);
        }

        [Test]
        public void DirectionAt_ZeroLengthSegment_ReturnsZero()
        {
            // Arrange: the first two corners sit on the same spot
            var points = new List<Vector2> { new Vector2(0, 0), new Vector2(0, 0), new Vector2(1, 0) };
            WaypointPath path = new(points);

            // Act
            Vector2 direction = path.DirectionAt(0f);

            // Assert
            AssertVector(Vector2.Zero, direction);
        }

        [Test]
        public void HasReachedEnd_BeforeEnd_IsFalse()
        {
            // Arrange
            WaypointPath path = CreateLPath();

            // Act
            bool finished = path.HasReachedEnd(6.9f);

            // Assert
            Assert.That(finished, Is.False);
        }
    }
}