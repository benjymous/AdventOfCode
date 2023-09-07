using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Utils.Collections
{

    public class Circle<T> : IEnumerable<T>
    {
        public T Value { get; set; }

        readonly Dictionary<T, Circle<T>> index;

        public static Circle<T> Create(IEnumerable<T> input)
        {
            Circle<T> first = new(input.First());
            var current = first;
            current.InsertRange(input.Skip(1));
            return first;
        }

        public Circle(T v, Circle<T> p = null, Circle<T> n = null)
        {
            index = p == null ? new Dictionary<T, Circle<T>>() : p.index;

            index[v] = this;

            Value = v;

            Insert(p, n);
        }

        void Insert(Circle<T> p = null, Circle<T> n = null)
        {
            orphaned = false;
            prev = p;
            next = n;

            if (prev != null)
            {
                prev.next = this;
            }
            else
            {
                prev = this;
            }

            if (next != null)
            {
                next.prev = this;
            }
            else
            {
                next = this;
            }
        }

        public Circle<T> Next() => next;
        public Circle<T> Prev() => prev;

        public bool Orphaned => orphaned;

        public Circle<T> Back(int distance)
        {
            var node = this;
            for (int i = 0; i < distance; ++i)
            {
                node = node.prev;
            }
            return node;
        }

        public Circle<T> Forward(int distance)
        {
            if (distance > Count) distance %= Count;

            var node = this;
            for (int i = 0; i < distance; ++i)
            {
                node = node.next;
            }
            return node;
        }

        public Circle<T> InsertNext(T v)
        {
            return new Circle<T>(v, this, next);
        }

        public Circle<T> InsertNext(Circle<T> v)
        {
            v.Insert(this, next);
            return v;
        }

        public Circle<T> PopNext()
        {
            var next = Next();
            next.Remove();
            return next;
        }

        public Circle<T> InsertRange(IEnumerable<T> vals)
        {
            var current = this;
            foreach (var v in vals)
            {
                current = current.InsertNext(v);
            }
            return current;
        }

        public Circle<T> InsertRange(IEnumerable<Circle<T>> vals)
        {
            var current = this;
            foreach (var v in vals)
            {
                current = current.InsertNext(v);
            }
            return current;
        }

        public Circle<T> Remove()
        {
            var removed = this;
            orphaned = true;
            removed.prev.next = removed.next;
            removed.next.prev = removed.prev;

            return removed.next;
        }

        public Circle<T> Remove(int count)
        {
            var current = this;
            while (!Solo() && count-- > 0)
            {
                current = current.Remove();
            }
            return current;
        }

        public void Move(int delta) => Move((long)delta);

        public void Move(long delta)
        {
            if (delta == 0) return;
            int move = (int)(Math.Abs(delta) % (Count - 1));
            if (move > Count / 2)
            {
                delta = -delta;
                move = Count - move - 1;
            }

            Circle<T> newPos;
            if (delta > 0)
            {
                newPos = Prev();
                Remove();
                newPos = newPos.Forward(move);
            }
            else
            {
                newPos = Next();
                Remove();
                newPos = newPos.Back(move + 1);
            }
            newPos.InsertNext(this);
        }

        public void Set(T val) => Value = val;

        public int Count => index.Count;

        public Circle<T> Find(T v)
        {
            var val = index[v]; return val.orphaned ? null : val;
        }

        public Circle<T> Reverse(int count)
        {
            var vals = Values().Take(count).Reverse();

            var current = this;
            foreach (var i in vals)
            {
                current.Set(i);
                current = current.Forward(1);
            }

            return this;
        }

        public bool Solo() => next == this;

        Circle<T> prev;
        Circle<T> next;

        bool orphaned = false;

        IEnumerable<T> Values()
        {
            yield return Value;
            var current = next;
            while (current != this)
            {
                yield return current.Value;
                current = current.Next();
            }
        }

        public IEnumerable<Circle<T>> Elements()
        {
            yield return this;
            var current = next;
            while (current != this)
            {
                yield return current;
                current = current.Next();
            }
        }

        public static implicit operator T(Circle<T> node) => node.Value;

        public override string ToString() => Value.ToString();

        public IEnumerator<T> GetEnumerator()
        {
            yield return Value;
            var current = next;
            while (current != this)
            {
                yield return current.Value;
                current = current.Next();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}