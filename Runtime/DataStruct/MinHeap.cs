using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFramework
{
    /// <summary>
    /// 最小堆
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MinHeap<T> : ICollection<T>
    {
        private T[] _data;
        private readonly Comparison<T> _comparison;
        private int _length;

        public int Count => _length;

        public T this[int index] => _data[index];

        public T TopValue => _data[0];

        public bool IsReadOnly => false;

        public MinHeap(Comparison<T> comparison = null)
        {
            _data = new T[256];
            if (comparison == null)
                _comparison = (x, y) => x.GetHashCode() - y.GetHashCode();
            else
                _comparison = comparison;
        }

        public MinHeap(int capacity, Comparison<T> comparison = null)
        {
            _data = new T[Mathf.Max(16, capacity)];
            if (comparison == null)
                _comparison = (x, y) => x.GetHashCode() - y.GetHashCode();
            else
                _comparison = comparison;
        }

        public void Add(T value)
        {
            if (_length == _data.Length)
            {
                var data = new T[_length << 1];
                Array.Copy(_data, data, _length);
                _data = data;
            }

            int child = _length;
            _data[_length++] = value;
            while (child > 0)
            {
                var parent = (child - 1) >> 1;
                if (_comparison(_data[parent], _data[child]) <= 0)
                    break;
                (_data[parent], _data[child]) = (_data[child], _data[parent]);
                child = parent;
            }
        }

        public void Clear()
        {
            var def = default(T);
            for (int i = 0; i < _length; i++)
            {
                _data[i] = def;
            }

            _length = 0;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _length; i++)
            {
                if (_comparison(_data[i], item) == 0)
                    return true;
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_data, 0, array, arrayIndex, _length);
        }

        public T RemoveTop()
        {
            return RemoveAt(0);
        }

        public T RemoveAt(int index)
        {
            var def = default(T);
            if (index >= _length)
                return def;
            var ret = _data[index];
            _data[index] = def;
            var tmp = _data[--_length];
            _data[_length] = def;
            int child = (index << 1) + 1;
            int parent;
            while (child < _length)
            {
                if (child < _length - 1 && _comparison(_data[child], _data[child + 1]) > 0)
                    child++;
                parent = (child - 1) >> 1;
                _data[parent] = _data[child];
                _data[child] = def;
                child = (child << 1) + 1;
            }

            child = (child - 1) >> 1;
            _data[child] = tmp;
            while (child > 0)
            {
                parent = (child - 1) >> 1;
                if (_comparison(_data[parent], _data[child]) <= 0)
                    break;
                tmp = _data[parent];
                _data[parent] = _data[child];
                _data[child] = tmp;
                child = parent;
            }

            return ret;
        }

        public bool Remove(T item)
        {
            int rev = -1;
            for (int i = 0; i < _length; i++)
            {
                if (_comparison(_data[i], item) == 0)
                {
                    rev = i;
                    break;
                }
            }

            if (rev == -1)
                return false;
            RemoveAt(rev);
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        private class Enumerator : IEnumerator<T>
        {
            private int _mPtr;
            private MinHeap<T> _mHeap;

            public T Current => _mPtr < _mHeap._length && _mPtr >= 0 ? _mHeap._data[_mPtr] : default(T);

            object IEnumerator.Current => _mPtr < _mHeap._length && _mPtr >= 0 ? _mHeap._data[_mPtr] : default(T);

            public Enumerator(MinHeap<T> heap)
            {
                _mHeap = heap;
                _mPtr = -1;
            }

            public void Dispose()
            {
                _mHeap = null;
            }

            public bool MoveNext()
            {
                if (++_mPtr < _mHeap._length)
                    return true;
                else
                    return false;
            }

            public void Reset()
            {
                _mPtr = -1;
            }
        }
    }
}