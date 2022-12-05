using SQLite.Net.Async;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamSQLite
{
    public interface ISQLiteConnection
    {
        string GetDataBasePath();
        SQLiteAsyncConnection GetConnection();
    }
}
