using System;
using NUnit.Framework;
using TD.Rules;

namespace TD.Tests.EditMode
{


    public sealed class HealthTests
    {
        [Test]
        public void TakeDamage_LessThanCurrent_ReducesCurrentAndReturnsFalse()
        {
            // Arrange
            var health = new Health(3);

            //Act
            bool killed = health.TakeDamage(1);

            //Assert
            Assert.That(health.Current, Is.EqualTo(2));
            Assert.That(killed, Is.False);
        }
        [Test]
        public void Constructor_MaxBelowOne_Throws()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>( () => new Health(0));
        }

        [Test]
        public void NewHealth_StartsFull()
        {
            // Arrange
            var health = new Health(4);

            // Assert
            Assert.That(health.Current, Is.EqualTo(4));
            Assert.That(health.Max, Is.EqualTo(4));
            Assert.That(health.IsDead, Is.False);

        }
        
        [Test]
        public void TakeDamage_ExactlyLethal_ReturnsTrue()
        {
            // Arrange
            var health = new Health(2);

            // Act
            var Damage = health.TakeDamage(2);

            // Assert
            Assert.That(Damage, Is.True);
            Assert.That(health.IsDead, Is.True);
        }

        [Test]
        public void TakeDamage_Overkill_CurrentNotNegative()
        {
            // Arrange
            var health = new Health(2);

            // Act
            health.TakeDamage(5);

            // Assert
            Assert.That(health.Current, Is.EqualTo(0));
        }

        [Test]
        public void TakeDamage_AlreadyDead_ReturnsFalse()
        {
            // Arrange: the first hit kills it
            var health = new Health(1);
            health.TakeDamage(1);

            // Act: a second hit lands on the dead enemy
            bool killed = health.TakeDamage(1);

            // Assert: only the first hit counts as the kill
            Assert.That(killed, Is.False);
        }

        [Test]
        public void TakeDamage_Negative_Throws()
        {
            // Arrange
            var health = new Health(3);

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => health.TakeDamage(-1));
        }

    }

}