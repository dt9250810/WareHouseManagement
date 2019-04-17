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
    class DB_category : DBM
    {
        // 設定DataGridView特性
        private static void SetDataGridViewProperties(DataGridView dataGridView)
        {
            //DataGridView properties
            dataGridView.ColumnCount = 5;
            dataGridView.Columns[0].Name = "編號";
            dataGridView.Columns[1].Name = "種類名稱";
            dataGridView.Columns[2].Name = "種類代號";
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

        public override DataGridView Retrieve(DataGridView dataGridView, Label label)
        {
            dataGridView.Rows.Clear();
            SetDataGridViewProperties(dataGridView);

            string sqlQuery = "SELECT categoryID, categoryName, engAlias, remark, created_date FROM category ORDER BY categoryID;";
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

        // 查詢(新增種類) 進階搜尋功能
        public DataGridView RetrieveAddSelectPage(DataGridView dataGridView, String str, Label label)
        {
            dataGridView.Rows.Clear();
            SetDataGridViewProperties(dataGridView);

            string sqlQuery = "SELECT categoryID, categoryName, engAlias, remark, created_date FROM category WHERE 1 " + str + " ORDER BY categoryID;";
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
        public Boolean Delete(int categoryid)
        {
            //SQLSTMT
            string sql = "DELETE FROM category WHERE categoryID = " + categoryid + ";";

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
                    MessageBox.Show("編號: " + categoryid + " 的種類資料已刪除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                conn.Close();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("可能【貨料】資料表的種類欄位有參考到所點選的該筆【種類】，故無法刪除該筆資料。\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
                return false;
            }
        }

        // ADD
        public void Add(string categoryName, string engAlias, string remark)
        {
            string sql = "INSERT INTO category (categoryName, engAlias, remark) " +
                         "VALUES (@CATEGORYNAME, UPPER(@ENGALIAS), @REMARK);";
            cmd = new MySqlCommand(sql, conn);

            //ADD PARAMETERS
            cmd.Parameters.AddWithValue("@CATEGORYNAME", categoryName.Trim());
            cmd.Parameters.AddWithValue("@ENGALIAS", engAlias.Trim());
            cmd.Parameters.AddWithValue("@REMARK", remark.Trim());

            //OPEN CON AND EXEC insert
            try
            {
                conn.Open();

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("新增種類成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        // 防止完全一樣的2個欄位完全一樣
        public Boolean CheckSame(string categoryName, string engAlias)
        {

            string sqlQuery = "SELECT * FROM category WHERE categoryName LIKE '" + categoryName + "' AND engAlias LIKE '" + engAlias + "';";
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

        // 檢查有無重複的種類名稱或種類代號
        public Boolean CheckDuplicate(string categoryName, string engAlias)
        {

            string sqlQuery = "SELECT * FROM category WHERE categoryName LIKE '" + categoryName + "' OR engAlias LIKE '" + engAlias + "';";
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

        // 設定種類comboBox
        public void InitCategoryCombobox(ComboBox comboBox)
        {
            Dictionary<string, string> cboData = new Dictionary<string, string>();
            cboData.Add("%", "所有種類");

            string sqlQuery = "SELECT categoryID, CONCAT(engAlias, ' ', categoryName) as mergeCategory FROM category ORDER BY engAlias, LENGTH(mergeCategory);";

            cmd = new MySqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    cboData.Add(row["categoryID"].ToString(), row["mergeCategory"].ToString());
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

        // 設定加入貨料區的種類comboBox
        public void InitAddCategoryCombobox(ComboBox comboBox)
        {
            Dictionary<string, string> cboData = new Dictionary<string, string>();

            string sqlQuery = "SELECT categoryID, CONCAT(engAlias, ' ', categoryName) as mergeCategory FROM category ORDER BY engAlias, LENGTH(mergeCategory);";

            cmd = new MySqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    cboData.Add(row["categoryID"].ToString(), row["mergeCategory"].ToString());
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
        public void Update(int categoryID, string remark)
        {
            //SQL STMT
            string sql = "UPDATE category SET remark = '" + remark + "' " + "WHERE categoryID = " + categoryID + ";";
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
                    MessageBox.Show("編號: " + categoryID + " 的資料已更新成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
