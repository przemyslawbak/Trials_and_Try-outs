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
using Xamarin.Forms;
using XamData.Data;
using XamData.Droid;

[assembly: Dependency(typeof(SQLite_Android))]
namespace XamData.Droid
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android() { }
        public SQLite.SQLiteConnection GetConnection()
        {
            var DBFileName = "backend.db3";
            string DocumentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(DocumentPath, DBFileName);
            var conn = new SQLite.SQLiteConnection(path);

            return conn;
        }
    }
}