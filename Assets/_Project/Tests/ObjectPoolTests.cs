using System;
using NUnit.Framework;
using TD.Rules;

namespace TD.Tests.EditMode
{
    public sealed class ObjectPoolTests
    {
        // Stand-in item: the pool works with any class, so tests don't need Unity objects.
        private sealed class Dummy { }

        [Test]
        public void Constructor_NullCreate_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new ObjectPool<Dummy>(null, null, null));
        }

        [Test]
        public void Constructor_NegativePrewarm_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ObjectPool<Dummy>(() => new Dummy(), null, null, -1));
        }

        [Test]
        public void Get_EmptyPool_CreatesNewItem()
        {
            // Arrange
            int created = 0;
            var pool = new ObjectPool<Dummy>(() => { created++; return new Dummy(); }, null, null);

            // Act
            Dummy item = pool.Get();

            // Assert
            Assert.That(item, Is.Not.Null);
            Assert.That(created, Is.EqualTo(1));
        }

        [Test]
        public void Release_ThenGet_ReturnsSameInstance()
        {
            // Arrange
            var pool = new ObjectPool<Dummy>(() => new Dummy(), null, null);
            Dummy first = pool.Get();

            // Act
            pool.Release(first);
            Dummy second = pool.Get();

            // Assert: the exact same object came back, so nothing new was made
            Assert.That(second, Is.SameAs(first));
        }

        [Test]
        public void Get_WithAvailableItem_DoesNotCreate()
        {
            // Arrange
            int created = 0;
            var pool = new ObjectPool<Dummy>(() => { created++; return new Dummy(); }, null, null);
            pool.Release(pool.Get());

            // Act
            pool.Get();

            // Assert: only the very first Get created an item
            Assert.That(created, Is.EqualTo(1));
        }

        [Test]
        public void Prewarm_CreatesItemsUpFrontAndReleasesThem()
        {
            // Arrange
            int created = 0;
            int released = 0;

            // Act
            var pool = new ObjectPool<Dummy>(
                () => { created++; return new Dummy(); },
                null,
                item => released++,
                prewarm: 3);

            // Assert: three items exist, all switched off and waiting
            Assert.That(pool.CountAvailable, Is.EqualTo(3));
            Assert.That(created, Is.EqualTo(3));
            Assert.That(released, Is.EqualTo(3));
        }

        [Test]
        public void Get_CallsOnGet()
        {
            // Arrange
            Dummy received = null;
            var pool = new ObjectPool<Dummy>(() => new Dummy(), item => received = item, null);

            // Act
            Dummy item = pool.Get();

            // Assert
            Assert.That(received, Is.SameAs(item));
        }

        [Test]
        public void Release_CallsOnRelease()
        {
            // Arrange
            Dummy received = null;
            var pool = new ObjectPool<Dummy>(() => new Dummy(), null, item => received = item);
            Dummy item = pool.Get();

            // Act
            pool.Release(item);

            // Assert
            Assert.That(received, Is.SameAs(item));
        }

        [Test]
        public void Release_SameItemTwice_Throws()
        {
            // Arrange
            var pool = new ObjectPool<Dummy>(() => new Dummy(), null, null);
            Dummy item = pool.Get();
            pool.Release(item);

            // Act + Assert: a second release would let two Gets hand out the same object
            Assert.Throws<InvalidOperationException>(() => pool.Release(item));
        }

        [Test]
        public void Release_Null_Throws()
        {
            // Arrange
            var pool = new ObjectPool<Dummy>(() => new Dummy(), null, null);

            // Act + Assert
            Assert.Throws<ArgumentNullException>(() => pool.Release(null));
        }
    }
}
