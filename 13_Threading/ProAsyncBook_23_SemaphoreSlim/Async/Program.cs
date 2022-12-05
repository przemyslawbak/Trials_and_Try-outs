using System;
using System.Collections.Generic;
using System.Threading;

namespace Async
{
    //Listing 4-20. The BufferPool Implementation
    class Program
    {
        static void Main(string[] args)
        {
            //
        }
    }
    public interface IBuffer
    {
        byte[] Buffer { get; }
    }
    public interface IBufferRegistration : IBuffer, IDisposable
    {
    }

    public class LOHBuffer : IBuffer //tworzenie buforu o maksymalnej wielkości
    {
        private readonly byte[] buffer;
        private const int LOHBufferMin = 85000;
        internal bool InUse { get; set; }
        public LOHBuffer()
        {
            buffer = new byte[LOHBufferMin];
        }
        public byte[] Buffer { get { return buffer; } }
    }

    public class BufferPool
    {
        private SemaphoreSlim guard;
        private List<LOHBuffer> buffers;
        public BufferPool(int maxSize)
        {
            guard = new SemaphoreSlim(maxSize);
            buffers = new List<LOHBuffer>(maxSize);
        }

        public IBufferRegistration GetBuffer()
        {
            // this blocks until a buffer is free
            guard.Wait();
            // can now get buffer so make sure we're the only thread manipulating
            // the list of buffers
            lock (buffers)
            {
                IBufferRegistration freeBuffer = null;
                // look for a free buffer
                foreach (LOHBuffer buffer in buffers)
                {
                    if (!buffer.InUse)
                    {
                        buffer.InUse = true;
                        freeBuffer = new BufferReservation(this, buffer);
                    }
                }
                // no free buffer so allocate a new one
                if (freeBuffer == null)
                {
                    var buffer = new LOHBuffer();
                    buffer.InUse = true;
                    buffers.Add(buffer);
                    freeBuffer = new BufferReservation(this, buffer);
                }
                return freeBuffer;
            }
        }

        private void Release(LOHBuffer buffer)
        {
            // flag buffer as no longer in use and release the semaphore
            // to allow more requests into the pool
            buffer.InUse = false;
            guard.Release();
        }
        class BufferReservation : IBufferRegistration
        {
            private readonly BufferPool pool;
            private readonly LOHBuffer buffer;
            public BufferReservation(BufferPool pool, LOHBuffer buffer)
            {
                this.pool = pool;
                this.buffer = buffer;
            }
            public byte[] Buffer
            {
                get { return buffer.Buffer; }
            }
            public void Dispose()
            {
                pool.Release(buffer);
            }
        }
    }
}
