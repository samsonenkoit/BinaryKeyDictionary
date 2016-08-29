using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Dictionarys
{
    public sealed class ThreadSafeEnumerator<T>: IEnumerator<T>, IDisposable
    {
        private readonly ReaderWriterLockSlim _sync;
        private readonly IEnumerator<T> _inner;

        public ThreadSafeEnumerator(ReaderWriterLockSlim sync, IEnumerator<T> inner)
        {
            _sync = sync;
            _inner = inner;

            _sync.EnterReadLock();
        }

        public bool MoveNext()
        {
            return _inner.MoveNext();
        }

        public void Reset()
        {
            _inner.Reset();
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }



        #region IDisposable

        public void Dispose()
        {
            _sync.ExitReadLock();
        }

        #endregion
    }
}
