using System;
using System.Data;
using System.Data.OleDb;

namespace EDIDocsIn.Common.impl
{
    /// <summary>
    /// Summary description for OleDbWrapper.
    /// </summary>
    /// 
    [Serializable]
    public class OLEDBWrapper 
    {
        protected OleDbConnection cnn;
        protected int Timeout = 60;

        public OLEDBWrapper(string connectionstring)
        {
            cnn = new OleDbConnection(connectionstring);
            cnn.Open();
        }

        public OLEDBWrapper()
        {
        }


        public OleDbConnection Connection
        {
            get { return cnn; }
        }

        public ConnectionState ConnectionState
        {
            get { return cnn.State; }
        }

        public void Close()
        {
            if (cnn.State != ConnectionState.Closed)
                cnn.Close();
            cnn = null;
        }

        public void Refresh(string connectionString)
        {
            if (cnn != null)
            {
                Close();
            }
            cnn = new OleDbConnection(connectionString);
            cnn.Open();
        }

        public OleDbCommand GetCommand()
        {
            var cmd = new OleDbCommand();
            cmd.Connection = cnn;
            cmd.CommandTimeout = Timeout;
            return cmd;
        }

        public bool CheckConnection()
        {
            if (cnn == null) return false;
            if (cnn.State == ConnectionState.Closed)
                cnn.Open();
            return (cnn.State != ConnectionState.Broken);
        }

        public DataTable RecordSet(string SQL)
        {
            OleDbCommand cmd = GetCommand();
            cmd.CommandText = SQL;
            var da = new OleDbDataAdapter(cmd);
            var tbl = new DataTable();
            da.Fill(tbl);
            return tbl;
        }

        public void Execute(string SQL)
        {
            OleDbCommand cmd = GetCommand();
            cmd.CommandText = SQL;
            cmd.ExecuteNonQuery();
        }

        public Object ExecuteWithKeyValue(string SQL)
        {
            SQL += " SELECT @@IDENTITY";
            OleDbCommand cmd = GetCommand();
            cmd.CommandText = SQL;
            return cmd.ExecuteScalar();
        }

        public bool BooleanScalar(string SQL)
        {
            OleDbCommand cmd = GetCommand();
            cmd.CommandText = SQL;
            return (bool) cmd.ExecuteScalar();
        }

        public string StringScalar(string SQL)
        {
            OleDbCommand cmd = GetCommand();
            cmd.CommandText = SQL;
            return cmd.ExecuteScalar().ToString();
        }

        public int IntScalar(string SQL)
        {
            OleDbCommand cmd = GetCommand();
            cmd.CommandText = SQL;
            return (int) cmd.ExecuteScalar();
        }

        public long LongScalar(string SQL)
        {
            OleDbCommand cmd = GetCommand();
            cmd.CommandText = SQL;
            return (long) cmd.ExecuteScalar();
        }
    }
}