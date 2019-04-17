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
    class DB_bring :DBM
    {
        // 設定DataGridView特性
        private static void SetDataGridViewProperties(DataGridView dataGridView)
        {
            //DataGridView properties
            dataGridView.ColumnCount = 7;
            dataGridView.Columns[0].Name = "型態";
            dataGridView.Columns[1].Name = "型號";
            dataGridView.Columns[2].Name = "種類名稱";
            dataGridView.Columns[3].Name = "廠牌";
            dataGridView.Columns[4].Name = "數量";
            dataGridView.Columns[5].Name = "日期時間";
            dataGridView.Columns[6].Name = "相關人員";

            // Column Font
            dataGridView.ColumnHeadersDefaultCellStyle.Font = columnFont;
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro;
            dataGridView.EnableHeadersVisualStyles = false;

            // Selection Mode
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
        }

        // 查詢庫存
        public override DataGridView Retrieve(DataGridView dataGridView, Label label)
        {
            dataGridView.Rows.Clear();
            SetDataGridViewProperties(dataGridView);

            string sqlQuery = "SELECT * FROM 查詢攜出入 WHERE 1 ORDER BY created_date DESC, type;";
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
                    dataGridView.Rows.Add(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4], DateTime.Parse(row[5].ToString()).ToString("yyyy-MM-dd HH:mm:ss"), row[6].ToString());
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

            string sqlQuery = "SELECT * FROM 查詢攜出入 " +
                              "WHERE 1 " + str + " " +
                              "ORDER BY created_date DESC, type;";
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
                    dataGridView.Rows.Add(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4], DateTime.Parse(row[5].ToString()).ToString("yyyy-MM-dd HH:mm:ss"), row[6].ToString());
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
    }
}
