using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;
using static XamFormsMessagingCenter.App;

namespace XamFormsMessagingCenter
{
    public class CrashManager
    {
        public void Dispose()
        {
            MessagingCenter.Unsubscribe<object, Exception>(
            this, MessagingKey.HandledException.ToString());
        }
        public CrashManager()
        {
            MessagingCenter.Subscribe<object, Exception>(
            this, MessagingKey.HandledException.ToString(), (obj, ex) => {
                Debug.WriteLine("Wyjątek: {0}", ex);
            });
        }
    }
}
