using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class Closing_Bll
    {
        DbHelper.SqlDb db;

        public Closing_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public SnehDLL.Closing_Dll Get(DateTime _closingDate)
        {
            SnehDLL.Closing_Dll CD = new SnehDLL.Closing_Dll();
            CD.ClosingDate = _closingDate;

            return CD;
        }
    }
}
