﻿using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace _01_DI_Example_HelloDI
{
    class Program
    {
        static void Main(string[] args)
        {
            IMessageWriter writer = new SecureMessageWriter(writer: new ConsoleMessageWriter(), identity: GetIdentity());
            var salutation = new Salutation(writer);
            salutation.Exclaim();
        }

        private static IIdentity GetIdentity()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return WindowsIdentity.GetCurrent();
            }
            else
            {
                // For non-Windows OSes, like Mac and Linux.
                return new GenericIdentity(
                    Environment.GetEnvironmentVariable("USERNAME")
                    ?? Environment.GetEnvironmentVariable("USER"));
            }
        }
    }
}
