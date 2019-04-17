using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Data;
using System.Collections;

namespace WareHouseManagement
{
    class DB_goods : DBM
    {
        // 設定DataGridView特性
        private static void SetDataGridViewProperties(DataGridView dataGridView)
        {
            //DataGridView properties
            dataGridView.ColumnCount = 9;
            dataGridView.Columns[0].Name = "編號";
            dataGridView.Columns[1].Name = "型號";
            dataGridView.Columns[2].Name = "種類名稱";
            dataGridView.Columns[3].Name = "種類代號";
            dataGridView.Columns[4].Name = "規格";
            dataGridView.Columns[5].Name = "廠牌";
            dataGridView.Columns[6].Name = "條碼號";
            dataGridView.Columns[7].Name = "數量";
            dataGridView.Columns[8].Name = "備註";

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

            string sqlQuery = "SELECT * FROM 查詢庫存 WHERE 1";
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
                    dataGridView.Rows.Add(row[0], row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7], row[8].ToString());
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

        // 查詢(新增貨料)
        public DataGridView RetrieveAddSelectPage(DataGridView dataGridView, Label label)
        {
            dataGridView.Rows.Clear();
            SetDataGridViewProperties(dataGridView);
            //DataGridView properties
            dataGridView.ColumnCount = 11;
            dataGridView.Columns[0].Name = "編號";
            dataGridView.Columns[1].Name = "型號";
            dataGridView.Columns[2].Name = "種類名稱";
            dataGridView.Columns[3].Name = "種類代號";
            dataGridView.Columns[4].Name = "規格";
            dataGridView.Columns[5].Name = "廠牌";
            dataGridView.Columns[6].Name = "條碼號";
            dataGridView.Columns[7].Name = "數量";
            dataGridView.Columns[8].Name = "備註";
            dataGridView.Columns[9].Name = "建立日期";
            dataGridView.Columns[10].Name = "最後修改日期";

            string sqlQuery = "SELECT * FROM 查詢新增貨料 WHERE 1";
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
                    dataGridView.Rows.Add(row[0], row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7], row[8].ToString(), DateTime.Parse(row[9].ToString()).ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Parse(row[10].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
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

            string sqlQuery = "SELECT goodsID, type, categoryName, engAlias, spec, brandName,CONCAT(UPPER(engAlias), '-', goodsID) as 'barcode', quantity, goods.remark " +
                              "FROM goods, category, brand " +
                              "WHERE (goods.categoryID = category.categoryID and goods.brandID = brand.brandID) " + str + " " +
                              "ORDER BY goodsID;";
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
                    dataGridView.Rows.Add(row[0], row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7], row[8].ToString());
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

        // 查詢(新增貨料) 進階搜尋功能
        public DataGridView RetrieveAddSelectPage(DataGridView dataGridView, String str, Label label)
        {
            dataGridView.Rows.Clear();
            SetDataGridViewProperties(dataGridView);

            //DataGridView properties
            dataGridView.ColumnCount = 11;
            dataGridView.Columns[0].Name = "編號";
            dataGridView.Columns[1].Name = "型號";
            dataGridView.Columns[2].Name = "種類名稱";
            dataGridView.Columns[3].Name = "種類代號";
            dataGridView.Columns[4].Name = "規格";
            dataGridView.Columns[5].Name = "廠牌";
            dataGridView.Columns[6].Name = "條碼號";
            dataGridView.Columns[7].Name = "數量";
            dataGridView.Columns[8].Name = "備註";
            dataGridView.Columns[9].Name = "建立日期";
            dataGridView.Columns[10].Name = "最後修改日期";

            string sqlQuery = "SELECT * FROM 查詢新增貨料 WHERE 1 " + str;
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
                    dataGridView.Rows.Add(row[0], row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7], row[8].ToString(), DateTime.Parse(row[9].ToString()).ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Parse(row[10].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
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

        // update
        public void Update(int goodsid , string spec, string remark, int quantity)
        {
            //SQL STMT
            string sql = "UPDATE goods SET spec = '" + spec + "', remark = '" + remark + "', quantity = " + quantity + " WHERE goodsID = " + goodsid + ";";
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
                    MessageBox.Show("編號: " + goodsid + " 的資料已更新成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        //DELETE FROM DB
        public void Delete(int goodsid)
        {
            if (CheckQuantity(goodsid) == false)
                return;

            //SQLSTMT
            string sql = "DELETE FROM goods WHERE goodsID = " + goodsid + " AND quantity = 0;";
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
                    MessageBox.Show("編號: " + goodsid + " 的資料已刪除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }
        public void Add(string type, int categoryID, string spec, int brandID, string remark, int quantity)
        {
            string sql = "INSERT INTO `goods` (`type`, `categoryID`, `spec`, `brandID`, `remark`, `quantity`) " +
                         "VALUES (UPPER(@TYPE), @CATEGORYID, @SPEC, @BRANDID, @REMARK, @QUANTITY);";
            cmd = new MySqlCommand(sql, conn);

            //ADD PARAMETERS
            cmd.Parameters.AddWithValue("@TYPE", type.Trim());
            cmd.Parameters.AddWithValue("@CATEGORYID", categoryID);
            cmd.Parameters.AddWithValue("@SPEC", spec.Trim());
            cmd.Parameters.AddWithValue("@BRANDID", brandID);
            cmd.Parameters.AddWithValue("@REMARK", remark.Trim());
            cmd.Parameters.AddWithValue("@QUANTITY", quantity);
            //OPEN CON AND EXEC insert
            try
            {
                conn.Open();

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("新增貨料成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        public Boolean CheckQuantity(int goodsid)
        {

            string sqlQuery = "SELECT * FROM goods WHERE goodsID = " + goodsid + " AND quantity = 0";
            cmd = new MySqlCommand(sqlQuery, conn);

            //OPEN CON,RETRIEVE,FILL DGVIEW
            try
            {
                conn.Open();
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                conn.Close();

                if(dt.Rows.Count != 0)
                {
                    dt.Rows.Clear();
                    return true;
                }
                else
                {
                    MessageBox.Show("刪除失敗，數量為 0 的資料才可以刪除", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public Boolean CheckType(string type)
        {

            string sqlQuery = "SELECT * FROM goods WHERE type LIKE '" + type + "';";
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
                    // 表示有相同型號的情況
                    dt.Rows.Clear();
                    return true;
                }
                else
                {
                    // 沒有相同型號
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
    }
}
