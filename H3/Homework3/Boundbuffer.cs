using System;
using System.Threading;
using System.Collections.Generic;

namespace Homework3
{
    internal class Boundbuffer<T>
    {
 
            
        private Queue<T> _queue = new Queue<T>();
        private Semaphore _empty = new Semaphore(4000001, 4000001);
        private Semaphore _full = new Semaphore(0, 4000001);
        private SpinLock _lock = new SpinLock();
       
      

        public void Enqueue(T item)
        {
          
            _empty.WaitOne();
            var taken = false;
            _lock.Enter(ref taken);
            _queue.Enqueue(item);
            _lock.Exit();
            _full.Release();
        }

        public T Dequeue()
        {
            _full.WaitOne();
            var taken = false;
            _lock.Enter(ref taken);
            var item = _queue.Dequeue();
            _lock.Exit();
            _empty.Release();
            return item;
        }
    }
}