using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfTesting
{
    class ReadOnlyCollectionRefEnumerator<T> : IList<T>, IReadOnlyList<T>
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


        public ReadOnlyCollectionRefEnumerator(IEnumerable<T> items)
        {
            _items = Enumerable.ToArray(items);
        }

        public ReadOnlyCollectionRefEnumerator(IList<T> items)
        {
            _items = new T[items.Count];
            items.CopyTo(_items, 0);
        }

        public ReadOnlyCollectionRefEnumerator(List<T> items)
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

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
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


        public class Enumerator : IEnumerator<T>
        {
            private readonly ReadOnlyCollectionRefEnumerator<T> _list;
            private readonly int _endIndex;
            private int _index;

            public T Current
            {
                get
                {
                    if (_index < 0 || _endIndex <= _index)
                        throw new InvalidOperationException();

                    return _list._items[_index];
                }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }


            internal Enumerator(ReadOnlyCollectionRefEnumerator<T> list)
            {
                _list = list;
                _endIndex = _list._items.Length;
                _index = -1;
            }


            public void Dispose()
            { }

            public bool MoveNext()
            {
                if (_index < _endIndex)
                {
                    _index++;
                    return (_index < _endIndex);
                }

                return false;
            }

            public void Reset()
            {
                _index = -1;
            }
        }
    }
}
