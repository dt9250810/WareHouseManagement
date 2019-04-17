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
    class DB_brand : DBM
    {
        // 設定DataGridView特性
        private static void SetDataGridViewProperties(DataGridView dataGridView)
        {
            //DataGridView properties
            dataGridView.ColumnCount = 4;
            dataGridView.Columns[0].Name = "編號";
            dataGridView.Columns[1].Name = "廠牌名稱";
            dataGridView.Columns[2].Name = "備註";
            dataGridView.Columns[3].Name = "建立日期";

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

        public override DataGridView Retrieve(DataGridView dataGridView, Label label)
        {
            dataGridView.Rows.Clear();
            SetDataGridViewProperties(dataGridView);

            string sqlQuery = "SELECT brandID, brandName, remark, created_date FROM brand ORDER BY brandID;";
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
                    dataGridView.Rows.Add(row[0], row[1].ToString(), row[2].ToString(), DateTime.Parse(row[3].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                BuildDeleteButton(dataGridView);

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

        // 查詢(新增廠牌) 進階搜尋功能
        public DataGridView RetrieveAddSelectPage(DataGridView dataGridView, String str, Label label)
        {
            dataGridView.Rows.Clear();
            SetDataGridViewProperties(dataGridView);

            string sqlQuery = "SELECT brandID, brandName, remark, created_date FROM brand WHERE 1 " + str + " ORDER BY brandID;";
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
                    dataGridView.Rows.Add(row[0], row[1].ToString(), row[2].ToString(), DateTime.Parse(row[3].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                BuildDeleteButton(dataGridView);

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

        //DELETE FROM DB
        public Boolean Delete(int brandid)
        {
            //SQLSTMT
            string sql = "DELETE FROM brand WHERE brandID = " + brandid + ";";

            cmd = new MySqlCommand(sql, conn);

            //'OPEN CON,EXECUTE DELETE,CLOSE CON
            try
            {
                conn.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.DeleteCommand = conn.CreateCommand();
                adapter.DeleteCommand.CommandText = sql;
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("編號: " + brandid + " 的廠牌資料已刪除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("可能【貨料】資料表的廠牌欄位有參考到所點選的該筆【廠牌】，故無法刪除該筆資料。\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
                return false;
            }
        }

        // ADD
        public Boolean Add(string brandName, string remark)
        {
            string sql = "INSERT INTO brand (brandName, remark) " +
                         "VALUES (UPPER(@BRANDNAME), @REMARK);";
            cmd = new MySqlCommand(sql, conn);

            //ADD PARAMETERS
            cmd.Parameters.AddWithValue("@BRANDNAME", brandName.Trim());
            cmd.Parameters.AddWithValue("@REMARK", remark.Trim());

            //OPEN CON AND EXEC insert
            try
            {
                conn.Open();

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("新增廠牌成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("資料庫中已存在相同【廠牌名稱】的資料，新增中止\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
                return false;
            }
        }

        // btn刪除按鈕建立
        private void BuildDeleteButton(DataGridView dataGridView)
        {
            var buttons = new DataGridViewButtonColumn
            {
                Text = "刪除",
                UseColumnTextForButtonValue = true,
                Name = "btn",
                HeaderText = "選項",
                DataPropertyName = "DELETE",
                FlatStyle = FlatStyle.Flat
            };

            buttons.CellTemplate.Style.BackColor = Color.FromArgb(212, 63, 58);
            buttons.CellTemplate.Style.ForeColor = Color.White;
            buttons.CellTemplate.Style.Font = columnFont;
            buttons.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridView.Columns.Add(buttons);
        }

        // 設定廠牌comboBox
        public void InitBrandCombobox(ComboBox comboBox)
        {
            Dictionary<string, string> cboData = new Dictionary<string, string>();
            cboData.Add("%", "所有廠牌");

            string sqlQuery = "SELECT brandID, brandName FROM brand ORDER BY length(brandName), brandName;";

            cmd = new MySqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    cboData.Add(row["brandID"].ToString(), row["brandName"].ToString());
                    comboBox.DataSource = new BindingSource(cboData, null);
                    comboBox.DisplayMember = "Value";
                    comboBox.ValueMember = "Key";
                }

                conn.Close();
                dt.Rows.Clear();
                cboData.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        // 新增貨料的廠牌comboBox
        public void InitAddBrandCombobox(ComboBox comboBox)
        {
            Dictionary<string, string> cboData = new Dictionary<string, string>();

            string sqlQuery = "SELECT brandID, brandName FROM brand ORDER BY length(brandName), brandName;";

            cmd = new MySqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    cboData.Add(row["brandID"].ToString(), row["brandName"].ToString());
                    comboBox.DataSource = new BindingSource(cboData, null);
                    comboBox.DisplayMember = "Value";
                    comboBox.ValueMember = "Key";
                }

                conn.Close();
                dt.Rows.Clear();
                cboData.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        // update
        public void Update(int brandID, string remark)
        {
            //SQL STMT
            string sql = "UPDATE brand SET remark = '" + remark + "' " + "WHERE brandID = " + brandID + ";";
            cmd = new MySqlCommand(sql, conn);

            //OPEN CON,UPDATE,RETRIEVE DGVIEW
            try
            {
                conn.Open();
                adapter = new MySqlDataAdapter(cmd);

                adapter.UpdateCommand = conn.CreateCommand();
                adapter.UpdateCommand.CommandText = sql;

                if (adapter.UpdateCommand.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("編號: " + brandID + " 的資料已更新成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

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
