using System.Collections.Generic;
using System.Linq;

namespace Advent.Utils.Collections
{

    public class Circle<T>
    {
        public T Value { get; set; }

        public Circle(T v, Circle<T> p = null, Circle<T> n = null)
        {
            Value = v;
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

        public Circle<T> Remove()
        {
            var removed = this;
            removed.prev.next = removed.next;
            removed.next.prev = removed.prev;

            return removed.next;
        }

        public int Count() => Values().Count();

        public bool Solo() => next == this;

        Circle<T> prev;
        Circle<T> next;

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