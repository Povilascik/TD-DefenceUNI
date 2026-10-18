using System;

namespace TD.Rules
{
    public sealed class Health
    {
        public int Max { get; }
        public int Current { get; private set; }
        public bool IsDead => Current <= 0;
        public Health(int max)
        {
            if (max < 1) throw new ArgumentOutOfRangeException(nameof(max));
            Max = max; 
            Current = max;
        }
        public bool TakeDamage(int amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
            if (IsDead) return false;
            
            Current = Math.Max(Current - amount, 0);
            return IsDead;
        }
    }
}
