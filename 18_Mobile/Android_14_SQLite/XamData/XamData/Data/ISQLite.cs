using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamData.Data
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
