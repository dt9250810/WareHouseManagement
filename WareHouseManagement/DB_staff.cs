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
    class DB_staff : DBM
    {
        // 設定DataGridView特性
        private static void SetDataGridViewProperties(DataGridView dataGridView)
        {
            //DataGridView properties
            dataGridView.ColumnCount = 5;
            dataGridView.Columns[0].Name = "編號";
            dataGridView.Columns[1].Name = "身份證字號";
            dataGridView.Columns[2].Name = "姓名";
            dataGridView.Columns[3].Name = "備註";
            dataGridView.Columns[4].Name = "建立日期";

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

        // 一般SELECT
        public override DataGridView Retrieve(DataGridView dataGridView, Label label)
        {
            dataGridView.Rows.Clear();
            SetDataGridViewProperties(dataGridView);

            string sqlQuery = "SELECT id, identity, name, remark, created_date FROM staff ORDER BY id;";
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
                    dataGridView.Rows.Add(row[0], row[1].ToString(), row[2].ToString(), row[3].ToString(), DateTime.Parse(row[4].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
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

        // 查詢進出人員 進階搜尋功能
        public DataGridView Retrieve(DataGridView dataGridView, String str, Label label)
        {
            dataGridView.Rows.Clear();
            SetDataGridViewProperties(dataGridView);

            string sqlQuery = "SELECT id, identity, name, remark, created_date FROM staff WHERE 1 " + str + " ORDER BY id;";
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
                    dataGridView.Rows.Add(row[0], row[1].ToString(), row[2].ToString(), row[3].ToString(), DateTime.Parse(row[4].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
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
        public Boolean Delete(int id)
        {
            //SQLSTMT
            string sql = "DELETE FROM staff WHERE id = " + id + ";";

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
                    MessageBox.Show("編號: " + id + " 的員工資料已刪除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                conn.Close();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("可能【攜出入】資料表的員工欄位有參考到所點選的該筆【員工】，故無法刪除該筆資料。\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
                return false;
            }
        }

        // ADD
        public void Add(string identity, string name, string remark)
        {
            string sql = "INSERT INTO staff (identity, name, remark) " +
                         "VALUES (UPPER(@IDENTITY), @NAME, @REMARK);";
            cmd = new MySqlCommand(sql, conn);

            //ADD PARAMETERS
            cmd.Parameters.AddWithValue("@IDENTITY", identity.Trim());
            cmd.Parameters.AddWithValue("@NAME", name.Trim());
            cmd.Parameters.AddWithValue("@REMARK", remark.Trim());

            //OPEN CON AND EXEC insert
            try
            {
                conn.Open();

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("新增人員成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
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

        // 檢查有無重複的種類名稱或種類代號
        public Boolean CheckDuplicate(string identity)
        {

            string sqlQuery = "SELECT * FROM staff WHERE identity LIKE '" + identity + "';";
            cmd = new MySqlCommand(sqlQuery, conn);

            //OPEN CON,RETRIEVE,FILL DGVIEW
            try
            {
                conn.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    // 表示有相同的情況
                    dt.Rows.Clear();
                    return true;
                }
                else
                {
                    // 沒有相同
                    dt.Rows.Clear();
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

        // 驗證身分證字號
        public Boolean IsIdentificationId(string arg_Identify)
        {
            Boolean d = false;
            if (arg_Identify.Length == 10)
            {
                arg_Identify = arg_Identify.ToUpper();
                if (arg_Identify[0] >= 0x41 && arg_Identify[0] <= 0x5A)
                {
                    var a = new[] { 10, 11, 12, 13, 14, 15, 16, 17, 34, 18, 19, 20, 21, 22, 35, 23, 24, 25, 26, 27, 28, 29, 32, 30, 31, 33 };
                    var b = new int[11];
                    b[1] = a[(arg_Identify[0]) - 65] % 10;
                    var c = b[0] = a[(arg_Identify[0]) - 65] / 10;
                    for (var i = 1; i <= 9; i++)
                    {
                        b[i + 1] = arg_Identify[i] - 48;
                        c += b[i] * (10 - i);
                    }
                    if (((c % 10) + b[10]) % 10 == 0)
                    {
                        d = true;
                    }
                }
            }
            return d;
        }

        // update
        public void Update(int id, string remark)
        {
            //SQL STMT
            string sql = "UPDATE staff SET remark = '" + remark + "' " + "WHERE id = " + id + ";";
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
                    MessageBox.Show("編號: " + id + " 的資料已更新成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        // 設定員工comboBox
        public void InitStaffCombobox(ComboBox comboBox)
        {
            Dictionary<string, string> cboData = new Dictionary<string, string>();
            cboData.Add("%", "所有員工");

            string sqlQuery = "SELECT id, name FROM staff ORDER BY LENGTH(name), name;";

            cmd = new MySqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    cboData.Add(row["id"].ToString(), row["name"].ToString());
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
    }   
}
