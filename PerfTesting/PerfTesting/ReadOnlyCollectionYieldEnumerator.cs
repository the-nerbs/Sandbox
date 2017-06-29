using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfTesting
{
    class ReadOnlyCollectionYieldEnumerator<T> : IList<T>, IReadOnlyList<T>
    {
        private readonly T[] _items;


        public int Count
        {
            get { return _items.Length; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public T this[int index]
        {
            get { return _items[index]; }
        }


        public ReadOnlyCollectionYieldEnumerator(IEnumerable<T> items)
        {
            _items = Enumerable.ToArray(items);
        }

        public ReadOnlyCollectionYieldEnumerator(IList<T> items)
        {
            _items = new T[items.Count];
            items.CopyTo(_items, 0);
        }

        public ReadOnlyCollectionYieldEnumerator(List<T> items)
        {
            _items = items.ToArray();
        }


        public bool Contains(T item)
        {
            return (IndexOf(item) != -1);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf(_items, item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            // note: this is made to mimic SZArrayEnumerator, since we already know that has the best performance.
            int end = _items.Length;

            for (int i = 0; i < end; i++)
            {
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        T IList<T>.this[int index]
        {
            get { return _items[index]; }
            set { throw new NotSupportedException(); }
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }
    }
}
