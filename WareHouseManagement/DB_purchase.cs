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
    class DB_purchase : DBM
    {
        // 設定DataGridView特性
        public static void SetDataGridViewProperties(DataGridView dataGridView)
        {
            //DataGridView properties
            dataGridView.ColumnCount = 10;
            dataGridView.Columns[0].Name = "編號";
            dataGridView.Columns[1].Name = "型號";
            dataGridView.Columns[2].Name = "種類名稱";
            dataGridView.Columns[3].Name = "種類代號";
            dataGridView.Columns[4].Name = "規格";
            dataGridView.Columns[5].Name = "廠牌";
            dataGridView.Columns[6].Name = "條碼號";
            dataGridView.Columns[7].Name = "庫存數量";
            dataGridView.Columns[8].Name = "備註";
            dataGridView.Columns[9].Name = "本次進貨數量";

            // Column Font
            dataGridView.ColumnHeadersDefaultCellStyle.Font = columnFont;
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro;
            dataGridView.EnableHeadersVisualStyles = false;

            // Selection Mode
            //dataGridView.ReadOnly = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView.AllowUserToAddRows = false;
            //dataGridView.AllowUserToDeleteRows = false;

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
        }


        public override DataGridView Retrieve(DataGridView dataGridView, Label label)
        {
            dataGridView.Rows.Clear();
            SetDataGridViewProperties(dataGridView);

            //DataGridView properties
            dataGridView.ColumnCount = 9;
            dataGridView.Columns[0].Name = "貨料編號";
            dataGridView.Columns[1].Name = "型號";
            dataGridView.Columns[2].Name = "種類名稱";
            dataGridView.Columns[3].Name = "種類代號";
            dataGridView.Columns[4].Name = "規格";
            dataGridView.Columns[5].Name = "廠牌";
            dataGridView.Columns[6].Name = "條碼號";
            dataGridView.Columns[7].Name = "進貨數量";
            dataGridView.Columns[8].Name = "建立時間";

            // DGV特性覆寫
            dataGridView.ReadOnly = true;
            dataGridView.AllowUserToDeleteRows = false;

            string sqlQuery = "SELECT * FROM 查詢進貨 WHERE 1;";
            cmd = new MySqlCommand(sqlQuery, conn);

            //OPEN CON,RETRIEVE,FILL DGVIEW
            try
            {
                conn.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);

                label.Text = "" + dt.Rows.Count;
                //LOOP THRU DT
                foreach (DataRow row in dt.Rows)
                {
                    dataGridView.Rows.Add(row[0], row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7], DateTime.Parse(row[8].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                }

                // DataGridView Style Setting
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    if ((i % 2) == 1)
                        dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Gainsboro;
                }

                conn.Close();

                //CLEAR DT
                dt.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            return dataGridView;
        }

        // select(多載加入WHERE條件) 進階搜尋功能
        public DataGridView Retrieve(DataGridView dataGridView, String str, Label label)
        {
            dataGridView.Rows.Clear();
            SetDataGridViewProperties(dataGridView);

            //DataGridView properties
            dataGridView.ColumnCount = 9;
            dataGridView.Columns[0].Name = "貨料編號";
            dataGridView.Columns[1].Name = "型號";
            dataGridView.Columns[2].Name = "種類名稱";
            dataGridView.Columns[3].Name = "種類代號";
            dataGridView.Columns[4].Name = "規格";
            dataGridView.Columns[5].Name = "廠牌";
            dataGridView.Columns[6].Name = "條碼號";
            dataGridView.Columns[7].Name = "進貨數量";
            dataGridView.Columns[8].Name = "建立時間";

            // DGV特性覆寫
            dataGridView.ReadOnly = true;
            dataGridView.AllowUserToDeleteRows = false;

            string sqlQuery = "SELECT * FROM 查詢進貨 " +
                              "WHERE 1 " + str + " " +
                              ";";
            cmd = new MySqlCommand(sqlQuery, conn);

            //OPEN CON,RETRIEVE,FILL DGVIEW
            try
            {
                conn.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);

                label.Text = "" + dt.Rows.Count;

                //LOOP THRU DT
                foreach (DataRow row in dt.Rows)
                {
                    dataGridView.Rows.Add(row[0], row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7], DateTime.Parse(row[8].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                // DataGridView Style Setting
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    if ((i % 2) == 1)
                        dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Gainsboro;
                }
                conn.Close();
                //CLEAR DT
                dt.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            return dataGridView;
        }

        public void Add(string sqlValue)
        {
            string sql = "INSERT INTO `purchase` (`goodsID`, `quantity`) " +
                         "VALUES " + sqlValue;
            cmd = new MySqlCommand(sql, conn);

            //OPEN CON AND EXEC insert
            try
            {
                conn.Open();

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("進貨資料已成功存入資料庫", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        public void UpdatePurchase()
        {
            //SQL STMT
            string sql = "SET SQL_SAFE_UPDATES = 0; " +
                         "UPDATE goods g INNER JOIN purchase p ON g.goodsID = p.goodsID " +
                         "SET g.quantity = g.quantity + p.quantity, p.saveGoods = 1 WHERE p.saveGoods = 0; " +
                         "SET SQL_SAFE_UPDATES = 1;";

            cmd = new MySqlCommand(sql, conn);

            //OPEN CON,UPDATE,RETRIEVE DGVIEW
            try
            {
                conn.Open();
                adapter = new MySqlDataAdapter(cmd);

                adapter.UpdateCommand = conn.CreateCommand();
                adapter.UpdateCommand.CommandText = sql;

                adapter.UpdateCommand.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }
    }
}
