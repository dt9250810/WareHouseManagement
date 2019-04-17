using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WareHouseManagement
{
    abstract class DBM
    {
        // 設定資料庫細節
        protected MySqlConnection conn = new MySqlConnection(connectionString);
        protected MySqlCommand cmd;
        protected MySqlDataAdapter adapter;
        protected DataTable dt = new DataTable();


        public static Font columnFont = new Font("微軟正黑體", 12, FontStyle.Bold);
        
        private static readonly string dataSource = "127.0.0.1";
        private static readonly string port = "3306";
        private static readonly string userName = "root";
        private static readonly string password = "78119";
        private static readonly string database = "ct_warehouse_db";

        protected static readonly string connectionString =
            "Server=" + dataSource +
            ";Port=" + port +
            ";Uid=" + userName +
            ";Pwd=" + password +
            ";Database=" + database + ";";

        public abstract DataGridView Retrieve(DataGridView dataGridView, Label label);
    }
}
