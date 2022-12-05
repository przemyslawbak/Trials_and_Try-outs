using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.XamarinAndroid;
using Xamarin.Forms;

[assembly: Dependency(typeof(XamSQLite.Droid.SQLiteConnectionDroid))]
namespace XamSQLite.Droid
{
    public class SQLiteConnectionDroid : ISQLiteConnection
    {
        private SQLiteAsyncConnection _connection;
        public string GetDataBasePath()
        {
            string filename = "MyDb.db3";
            string path = System.Environment.GetFolderPath(
            System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
        public SQLiteAsyncConnection GetConnection()
        {
            if (_connection != null)
            {
                return _connection;
            }
            SQLiteConnectionWithLock connectioonWithLock =
            new SQLiteConnectionWithLock(
            new SQLitePlatformAndroid(),
            new SQLiteConnectionString(GetDataBasePath(), true)
            );
            return _connection = new SQLiteAsyncConnection(() => connectioonWithLock);
        }
    }
}