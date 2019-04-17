using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WareHouseManagement
{
    class DB_manager : DBM
    {
        private static string staffID ="0";
        private static string name = "NONE";

        public string Name { get { return name; } }
        public string StaffID { get { return staffID; } }

        public override DataGridView Retrieve(DataGridView dataGridView, Label label)
        {
            throw new NotImplementedException();
        }
        public Boolean CheckAccount(string account_number, string password)
        {

            string sqlQuery = "SELECT staffID, name FROM manager, staff WHERE manager.staffID = staff.id AND account_number LIKE '" + account_number + "' AND password LIKE '" + password + "';";
            cmd = new MySqlCommand(sqlQuery, conn);

            //OPEN CON,RETRIEVE,FILL DGVIEW
            try
            {
                conn.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    staffID = row["staffID"].ToString();
                    name = row["name"].ToString();
                }
                if (dt.Rows.Count > 0)
                {
                    dt.Rows.Clear();
                    conn.Close();
                    return true;
                }
                else
                {
                    dt.Rows.Clear();
                    conn.Close();
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
                return false;
            }
        }
    }
}
