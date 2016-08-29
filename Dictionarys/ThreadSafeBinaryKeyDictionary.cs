using System;
using System.Collections.Generic;
using System.Threading;
using Dictionarys.Interface;

namespace Dictionarys
{
    /// <summary>
    /// Потокобезопасная версия словаря BinaryKeyDictionary
    /// 
    /// За раз позволяет либо читать данные нескольким потокам, либо писать данные одному.
    /// 
    /// Если нет активных операций записи, то данные могут читаться несколькими потоками без блокировки.
    /// Если есть активная оперция записи, то потоки требующие доступа на чтение будут заблокированы до 
    /// завершения операции записи.
    /// Если есть потоки читающие данные, то блокируются потоки ожидающие доступа на запись (читающие потоки не блокируются)
    /// 
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TName"></typeparam>
    /// <typeparam name="T"></typeparam>
    public sealed class ThreadSafeBinaryKeyDictionary<T, TId, TName> : BinaryKeyDictionary<T, TId, TName>, IDisposable
        where T : IBinaryKeyItem<TId, TName>
    {
        private readonly ReaderWriterLockSlim _sync;

        public ThreadSafeBinaryKeyDictionary(): base()
        {
            _sync = new ReaderWriterLockSlim();
        }

        protected override int GetCount()
        {
            return ThreadSafeRead(() => base.GetCount());
        }

        protected override void Set(T item, bool isNew = false)
        {

            ThreadSafeWrite(() =>
            {
                base.Set(item, isNew);
            });
        }

        public override bool Remove(IBinaryKey<TId, TName> key)
        {
            return ThreadSafeWrite(() => base.Remove(key));
        }

        public override T Get(IBinaryKey<TId, TName> key)
        {
            return ThreadSafeRead(() => base.Get(key));
        }

        public override IList<T> GetByKeyId(TId id)
        {
            return ThreadSafeRead(() => base.GetByKeyId(id));
        }

        public override bool ContainsKeyId(TId id)
        {
            return ThreadSafeRead(() => base.ContainsKeyId(id));
        }

        public override bool ContainsKeyName(TName name)
        {
            return ThreadSafeRead(() => base.ContainsKeyName(name));
        }

        public override IList<T> GetByKeyName(TName name)
        {
            return ThreadSafeRead(() => base.GetByKeyName(name));
        }

        public override void Clear()
        {
            ThreadSafeRead(() => base.Clear());
        }

        public override bool ContainsKey(IBinaryKey<TId, TName> key)
        {
            return ThreadSafeRead(() => base.ContainsKey(key));
        }

        protected override IList<TId> GetIds()
        {
            return ThreadSafeRead(() => base.GetIds());
        }

        protected override IList<IBinaryKey<TId, TName>> GetKeys()
        {
            return ThreadSafeRead(() => base.GetKeys());
        }

        protected override IList<TName> GetNames()
        {
            return ThreadSafeRead(() => base.GetNames());
        }

        /// <summary>
        /// Важно: При завершении работы с enumerator необходимо вызвать метод Dispose, т.к. enumerator блокирует поток
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<T> GetEnumerator()
        {
            return new ThreadSafeEnumerator<T>(_sync, base.GetEnumerator());
        }

        #region Helper

        private void ThreadSafeWrite(Action writeAction)
        {
            try
            {
                _sync.EnterWriteLock();

                writeAction();
            }
            finally
            {
                _sync.ExitWriteLock();
            }
        }

        private TR ThreadSafeWrite<TR>(Func<TR> writeAction)
        {
            try
            {
                _sync.EnterWriteLock();

                return writeAction();
            }
            finally
            {
                _sync.ExitWriteLock();
            }
        }

        private void ThreadSafeRead(Action readAction)
        {
            try
            {
                _sync.EnterReadLock();

                readAction();
            }
            finally
            {
                _sync.ExitReadLock();
            }
        }

        private TR ThreadSafeRead<TR>(Func<TR> readAction)
        {
            try
            {
                _sync.EnterReadLock();

                return readAction();
            }
            finally
            {
                _sync.ExitReadLock();
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _sync.Dispose();
        }

        #endregion
    }
}
