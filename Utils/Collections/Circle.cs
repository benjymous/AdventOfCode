using System.Collections.Generic;
using System.Linq;

namespace AoC.Utils.Collections
{

    public class Circle<T>
    {
        public T Value { get; set; }

        Dictionary<T, Circle<T>> index;

        public static Circle<T> Create(IEnumerable<T> input)
        {
            Circle<T> first = new Circle<T>(input.First());
            var current = first;
            current.InsertRange(input.Skip(1));
            return first;
        }

        public Circle(T v, Circle<T> p = null, Circle<T> n = null)
        {
            if (p == null)
            {
                index = new Dictionary<T, Circle<T>>();
            }
            else
            {
                index = p.index;
            }

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
            var node = this;
            for (int i = 0; i < distance; ++i)
            {
                node = node.next;
            }
            return node;
        }

        public Circle<T> InsertNext(T v)
        {
            return new Circle<T>(v, this, this.next);
        }

        public Circle<T> InsertNext(Circle<T> v)
        {
            v.Insert(this, this.next);
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

        public void Set(T val)
        {
            this.Value = val;
        }

        public int Count() => index.Count();

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

        public IEnumerable<T> Values()
        {
            yield return Value;
            var current = next;
            while (current != this)
            {
                yield return current.Value;
                current = current.Next();
            }
        }

        public override string ToString() => Value.ToString();
    }
}