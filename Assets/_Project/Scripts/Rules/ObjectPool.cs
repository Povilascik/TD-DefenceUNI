using System;
using System.Collections.Generic;

namespace TD.Rules
{
    public sealed class ObjectPool<T> where T : class
    {
        private readonly Func<T> create;
        private readonly Action<T> onGet;
        private readonly Action<T> onRelease;
        private readonly Stack<T> available;
        public int CountAvailable => available.Count;
        public ObjectPool(Func<T> create, Action<T> onGet, Action<T> onRelease, int prewarm = 0)
        {
            if (create == null) throw new ArgumentNullException(nameof(create));
            if (prewarm < 0) throw new ArgumentOutOfRangeException(nameof(prewarm));

            this.create = create;
            this.onGet = onGet;
            this.onRelease = onRelease;
            available = new Stack<T>();

            for (int i = 0; i < prewarm; i++)
            {
                T item = create();
                onRelease?.Invoke(item);
                available.Push(item);
            }
        }
        public T Get()
        {
            T item = available.Count > 0 ? available.Pop() : create();
            onGet?.Invoke(item);

            return item;
        }
        public void Release(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if(available.Contains(item)) throw new InvalidOperationException("Item was already released");
            onRelease?.Invoke(item);
            available.Push(item);
        }
    }
}
