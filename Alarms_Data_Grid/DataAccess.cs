using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace DATAGRID_NATIVE_ATTEMPT_1
{
    [ComVisible(true)]  
    [Guid("12345678-abcd-1234-ef00-0123456789ab")] 
    [ClassInterface(ClassInterfaceType.AutoDual)]  
    public class DataAccess
    {
        private static SqlConnection activeConnection;

        public static void InitializeConnection(string connectionString)
        {
            activeConnection = new SqlConnection(connectionString);
        }

        public static DataTable GetData()
        {
            string query = "SELECT ID, Description, State, Status, HiHiLimit, HiLimit, LoLimit, LoLoLimit, Inhibit, Delay, GroupDesc FROM dbo.alarmstable";
            SqlDataAdapter adapter = new SqlDataAdapter(query, activeConnection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }

        public static string GetCellValue(int rowIndex, string columnName)
        {
            DataTable dataTable = GetData();
            if (rowIndex >= 0 && rowIndex < dataTable.Rows.Count)
            {
                return dataTable.Rows[rowIndex][columnName]?.ToString() ?? "";
            }
            return "";
        }

        public static int GetRowCount()
        {
            return GetData().Rows.Count;
        }
    }
}
