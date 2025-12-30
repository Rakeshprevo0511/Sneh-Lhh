using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SnehBLL
{
    public class Location_Bll
    {
        DbHelper.SqlDb db;

        public enum LocationType
        {
            Country,
            State,
            City
        }

        private int LocationTypeGet(LocationType _locationType)
        {
            int _type = 0;
            switch (_locationType)
            {
                case LocationType.Country: _type = 0; break;
                case LocationType.State: _type = 1; break;
                case LocationType.City: _type = 2; break;
                default: _type = 0; break;
            }
            return _type;
        }

        public Location_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public SnehDLL.Location_Dll Get(int location_id)
        {
            SnehDLL.Location_Dll D = null;
            SqlCommand cmd = new SqlCommand("Location_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@location_id", SqlDbType.Int).Value = location_id;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.Location_Dll(); int i = 0;
                D.location_id = int.Parse(dt.Rows[i]["location_id"].ToString());
                D.name = dt.Rows[i]["name"].ToString();
                int location_type = 0; int.TryParse(dt.Rows[i]["location_type"].ToString(), out location_type);
                D.location_type = location_type;
                int parent_id = 0; int.TryParse(dt.Rows[i]["parent_id"].ToString(), out parent_id);
                D.parent_id = parent_id;
                int is_visible = 0; int.TryParse(dt.Rows[i]["is_visible"].ToString(), out is_visible);
                D.is_visible = is_visible;
            }
            return D;
        }

        public List<SnehDLL.Location_Dll> Get(int parent_id, LocationType _locationType)
        {
            List<SnehDLL.Location_Dll> DL = new List<SnehDLL.Location_Dll>();
            SqlCommand cmd = new SqlCommand("Location_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@location_type", SqlDbType.Int).Value = LocationTypeGet(_locationType);
            cmd.Parameters.Add("@parent_id", SqlDbType.Int).Value = parent_id;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.Location_Dll D = new SnehDLL.Location_Dll();
                D.location_id = int.Parse(dt.Rows[i]["location_id"].ToString());
                D.name = dt.Rows[i]["name"].ToString();
                int location_type = 0; int.TryParse(dt.Rows[i]["location_type"].ToString(), out location_type);
                D.location_type = location_type;
                D.parent_id = parent_id;
                int is_visible = 0; int.TryParse(dt.Rows[i]["is_visible"].ToString(), out is_visible);
                D.is_visible = is_visible;

                DL.Add(D);
            }
            return DL;
        }
    }
}
