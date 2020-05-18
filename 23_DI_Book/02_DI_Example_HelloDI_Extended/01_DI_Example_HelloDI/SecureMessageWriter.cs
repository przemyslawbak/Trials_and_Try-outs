using System;
using System.Security.Principal;

namespace _01_DI_Example_HelloDI
{
    internal class SecureMessageWriter : IMessageWriter
    {
        private IMessageWriter writer;
        private IIdentity identity;

        public SecureMessageWriter(IMessageWriter writer, IIdentity identity)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            if (identity == null)
                throw new ArgumentNullException("identity");

            this.writer = writer;
            this.identity = identity;
        }

        public void Write(string message)
        {
            if (this.identity.IsAuthenticated)
            {
                this.writer.Write(message);
            }
        }
    }
}