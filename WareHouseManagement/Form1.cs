using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BarcodeLib;
using UIToolbox;
using Spire.Xls;
using System.Runtime.InteropServices;

namespace WareHouseManagement
{
    public partial class Form1 : Form
    {
        public readonly string FINAL_UPDATED = "2018/10/20 11:00"; // 專案最終改動日期
        private string staffID = "0", magerName = "NONE";

        // 建立db物件
        DB_goods db_goods;
        DB_category db_category;
        DB_brand db_brand;
        DB_staff db_staff;
        DB_bring db_bring;
        DB_purchase db_purchase;

        // 條碼物件
        Barcode bc_p2;

        public Form1()
        {
            InitializeComponent();
        }

        public Form1(string staffID, string magerName)
        {
            InitializeComponent();
            this.staffID = staffID;
            this.magerName = magerName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 初始化設定
            InitialSetting initialSetting = new InitialSetting(statusStrip1, timer1);

            // 建立資料庫物件實體
            db_goods = new DB_goods();
            db_category = new DB_category();
            db_brand = new DB_brand();
            db_staff = new DB_staff();
            db_bring = new DB_bring();
            db_purchase = new DB_purchase();

            // 載入資料庫資料
            // 查詢庫存
            db_goods.Retrieve(goodsDataGridView, lbl_p2_total);
            // 查詢新增貨料
            db_goods.RetrieveAddSelectPage(addGoodsDataGridView, lbl_p5_1_total);
            // 查詢新增種類
            db_category.Retrieve(categoryDataGridView, lbl_p5_2_total);
            // 查詢新增廠牌
            db_brand.Retrieve(brandDataGridView, lbl_p5_3_total);
            // 查詢人員
            db_category.Retrieve(staffDataGridView, lbl_p6_total);
            // 查詢攜出入資料
            db_bring.Retrieve(bringDataGridView, lbl_p3_total);
            // 查詢貨料資料(進貨頁面p4_1)
            db_goods.Retrieve(purchaseGoodsDataGridView, lbl_p4_1_total);
            purchaseGoodsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // 查詢進貨資料(進貨查詢頁面p4_2)
            db_purchase.Retrieve(purchaseDataGridView, lbl_p4_2_total);

            // 建立page2之條碼物件
            bc_p2 = new Barcode();

            // 初始textBox設定及限制
            setAllControllerInitialization();
            setTip(StatusType.NONE);

            // 初始化comboBox
            refreshComboBox();

            //this.tabControl1.SelectedTab = this.tabPage4;
            //this.tabControl2.SelectedTab = this.tabPage5_3;
        }
        ////////////////////////// 自定義方法 ///////////////////////////////////////////////////

        private void refreshComboBox()
        {
            db_category.InitCategoryCombobox(cbo_p2_search_category);
            db_brand.InitBrandCombobox(cbo_p2_search_brand);
            db_staff.InitStaffCombobox(cbo_p3_search_staff);
            db_category.InitAddCategoryCombobox(cbo_p5_1_category);
            db_brand.InitAddBrandCombobox(cbo_p5_1_brand);
            db_category.InitCategoryCombobox(cbo_p4_1_search_category);
            db_brand.InitBrandCombobox(cbo_p4_1_search_brand);
            db_category.InitCategoryCombobox(cbo_p4_2_search_category);
            db_brand.InitBrandCombobox(cbo_p4_2_search_brand);
            tabControl3.TabPages.Remove(tabPage4_3);
        }

        // 調整dataGridView隔行換色
        private DataGridView gridViewStyleSetting(DataGridView dataGridView)
        {
            // DataGridView Style Setting
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.White;
                if ((i % 2) == 1)
                    dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Gainsboro;
            }
            return dataGridView;
        }

        // 設定txtbox限制
        private void setAllControllerInitialization()
        {

            // 將只能數字的textBox設定為無法右鍵快捷
            txt_p2_quantity.ShortcutsEnabled = false;
            txt_p2_search_quantity.ShortcutsEnabled = false;

            txt_p5_1_quantity.ShortcutsEnabled = false;

            // 設定最大textbox長度
            txt_p2_quantity.MaxLength = 6;
            txt_p2_spec.MaxLength = 140;
            txt_p2_remark.MaxLength = 40;
            txt_p2_search_string.MaxLength = 20;
            txt_p2_search_quantity.MaxLength = 4;


            txt_p5_1_type.MaxLength = 40;
            txt_p5_1_quantity.MaxLength = 6;
            txt_p5_1_spec.MaxLength = 140;
            txt_p5_1_remark.MaxLength = 40;
            txt_p5_2_categoryName.MaxLength = 40;
            txt_p5_2_engAlias.MaxLength = 20;
            txt_p5_2_remark.MaxLength = 40;
            txt_p5_3_brandName.MaxLength = 40;
            txt_p5_3_remark.MaxLength = 40;
            txt_p6_identity.MaxLength = 10;
            txt_p6_name.MaxLength = 15;


            // 設定底字浮水印
            txt_p2_quantity.SetWatermark("必填，輸入數量");
            txt_p5_1_type.SetWatermark("必填，輸入型號，至多40個字");
            txt_p5_1_quantity.SetWatermark("必填，輸入數量");
            txt_p5_2_categoryName.SetWatermark("必填，輸入種類名稱，至多40字");
            txt_p5_2_engAlias.SetWatermark("必填，種類代號(大寫英文或數字)至多20字");
            txt_p5_3_brandName.SetWatermark("必填，輸入廠牌名稱，至多40字");
            txt_p6_identity.SetWatermark("必填，輸入身分證字號");
            txt_p6_name.SetWatermark("必填，輸入姓名");

            // dateTimePicker初始化
            dtp_p3_start.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dtp_p3_end.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            dtp_p4_2_start.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dtp_p4_2_end.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

            // 記錄所登入的管理者資料
            lbl_p1_name.Text = magerName;
            lbl_p1_staffID.Text = staffID;
            lbl_manager.Text = magerName;

            // 覆寫dataGridView風格
            DB_purchase.SetDataGridViewProperties(purchasePrepareDataGridView);
        }

        // 檢查格式鍵盤輸入是否為數字
        private Boolean checkNumFormat(KeyPressEventArgs e)
        {
            try
            {
                if (((int)e.KeyChar < 48 | (int)e.KeyChar > 57) & (int)e.KeyChar != 8)
                    return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        // 檢查textbox數值範圍
        private Boolean checkNumRange(TextBox textbox, int min, int max)
        {
            try
            {
                if (textbox.Text == "")
                    return false;
                else if (Int32.Parse(textbox.Text) >= min && Int32.Parse(textbox.Text) <= max)
                {
                    return false;
                }

                textbox.Text = textbox.Text.Substring(0, textbox.Text.Length - 1);
                MessageBox.Show("此欄位輸入的數值必須介於 " + min + " 至 " + max + " 之間", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return true;
        }

        // Page2 進階搜尋選項
        private void searchGoods()
        {
            int quantity;
            string searchStr, category, brand;
            string sql = "";
            try
            {
                if (checkGroupBox_p2.Checked == true)
                {

                    searchStr = txt_p2_search_string.Text;

                    if (txt_p2_search_quantity.Text == "")
                        quantity = -99999;
                    else
                        quantity = Int32.Parse(txt_p2_search_quantity.Text);

                    category = cbo_p2_search_category.SelectedValue.ToString();
                    brand = cbo_p2_search_brand.SelectedValue.ToString();

                    if (searchStr != "")
                    {
                        sql += String.Format(" AND (goodsID like '%{0}%' OR type like '%{1}%' OR categoryName like '%{2}%' OR " +
                            "engAlias like '%{3}%' OR spec like '%{4}%' OR brandName like '%{5}%' OR quantity like '%{6}%' OR goods.remark like '%{7}%') "
                            , searchStr, searchStr, searchStr, searchStr, searchStr, searchStr, searchStr, searchStr);
                    }

                    sql += " AND quantity >=" + quantity;
                    sql += " AND category.categoryID like '" + category + "'";
                    sql += " AND brand.brandID like '" + brand + "'";


                    db_goods.Retrieve(goodsDataGridView, sql, lbl_p2_total);
                    setTip(StatusType.SELECT);

                    if (lbl_p2_total.Text == "0")
                    {
                        panel_p2.Enabled = false;

                        lbl_p2_status.BackColor = Color.Silver;
                        lbl_p2_status.ForeColor = Color.Black;
                        btn_p2_delete.BackColor = Color.FromArgb(224, 224, 224);
                        btn_p2_update.BackColor = Color.FromArgb(224, 224, 224);
                        lbl_p2_status.Text = "系統鎖定中";

                        txt_p2_goodsID.Text = "";
                        txt_p2_type.Text = "";
                        txt_p2_categoryName.Text = "";
                        txt_p2_engAlias.Text = "";
                        txt_p2_spec.Text = "";
                        txt_p2_brandName.Text = "";
                        picBox_p2_barcode.Image = null;
                        txt_p2_quantity.Text = "";
                        txt_p2_remark.Text = "";

                        btn_p2_export.Enabled = false;
                        btn_p2_print.Enabled = false;
                        setTip(StatusType.NULL);
                    }
                    else
                    {
                        btn_p2_export.Enabled = true;
                        btn_p2_print.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Page3 進階搜尋選項
        private void searchBring()
        {
            string searchStr, staff, start_date, end_date;
            string sql = "";
            try
            {
                if (checkGroupBox_p3.Checked == true)
                {
                    searchStr = txt_p3_search_string.Text;
                    start_date = dtp_p3_start.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    end_date = dtp_p3_end.Value.ToString("yyyy-MM-dd HH:mm:ss");

                    if (!(chk_p3_in.Checked && chk_p3_out.Checked))
                    {
                        if ((chk_p3_in.Checked || chk_p3_out.Checked) == false)
                            sql += " AND 0 ";
                        if (chk_p3_in.Checked) sql += " AND bring_type LIKE '攜回' ";
                        if (chk_p3_out.Checked) sql += " AND bring_type LIKE '攜出' ";
                    }

                    if (cbo_p3_search_staff.SelectedValue.ToString() == "%")
                        staff = "%";
                    else
                        staff = cbo_p3_search_staff.Text.ToString();

                    if (searchStr != "")
                    {
                        sql += String.Format(" AND (bring_type like '%{0}%' OR type like '%{1}%' OR categoryName like '%{2}%' OR " +
                            "brandName like '%{3}%' OR quantity like '%{4}%' OR created_date like '%{5}%' OR name like '%{6}%') "
                            , searchStr, searchStr, searchStr, searchStr, searchStr, searchStr, searchStr);
                    }

                    sql += " AND name like '" + staff + "'";
                    sql += " AND created_date BETWEEN '" + start_date + "' AND '" + end_date + "' ";

                    db_bring.Retrieve(bringDataGridView, sql, lbl_p3_total);
                    setTip(StatusType.SELECT);

                    if (lbl_p3_total.Text == "0")
                    {
                        panel_p2.Enabled = false;
                        btn_p2_export.Enabled = false;
                        setTip(StatusType.NULL);
                    }
                    else
                    {
                        btn_p2_export.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Page4_1 進階搜尋選項
        private void searchPurchaseGoods()
        {
            string searchStr, category, brand;
            string sql = "";
            try
            {
                if (checkGroupBox_p4_1.Checked == true)
                {
                    searchStr = txt_p4_1_search_string.Text;

                    category = cbo_p4_1_search_category.SelectedValue.ToString();
                    brand = cbo_p4_1_search_brand.SelectedValue.ToString();

                    if (searchStr != "")
                    {
                        sql += String.Format(" AND (goodsID like '%{0}%' OR type like '%{1}%' OR categoryName like '%{2}%' OR " +
                            "engAlias like '%{3}%' OR spec like '%{4}%' OR brandName like '%{5}%' OR quantity like '%{6}%' OR goods.remark like '%{7}%') "
                            , searchStr, searchStr, searchStr, searchStr, searchStr, searchStr, searchStr, searchStr);
                    }

                    sql += " AND category.categoryID like '" + category + "'";
                    sql += " AND brand.brandID like '" + brand + "'";


                    db_goods.Retrieve(purchaseGoodsDataGridView, sql, lbl_p4_1_total);
                    purchaseGoodsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    setTip(StatusType.SELECT);

                    if (lbl_p4_1_total.Text == "0")
                        setTip(StatusType.NULL);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Page4_2 進階搜尋選項
        private void searchPurchase()
        {
            string searchStr, category, brand, start_date, end_date;
            string sql = "";
            try
            {
                if (checkGroupBox_p4_2.Checked == true)
                {
                    searchStr = txt_p4_2_search_string.Text;

                    start_date = dtp_p4_2_start.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    end_date = dtp_p4_2_end.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    category = (cbo_p4_2_search_category.SelectedValue.ToString() == "%") ? "%" : cbo_p4_2_search_category.Text.ToString();
                    brand = (cbo_p4_2_search_brand.SelectedValue.ToString() == "%") ? "%" : cbo_p4_2_search_brand.Text.ToString();


                    if (searchStr != "")
                    {
                        sql += String.Format(" AND (goodsID like '%{0}%' OR type like '%{1}%' OR categoryName like '%{2}%' OR " +
                            "engAlias like '%{3}%' OR spec like '%{4}%' OR brandName like '%{5}%' OR barcode like '%{6}%' OR quantity like '%{7}%') "
                            , searchStr, searchStr, searchStr, searchStr, searchStr, searchStr, searchStr, searchStr);
                    }

                    sql += " AND categoryName like '" + category + "'";
                    sql += " AND brandName like '" + brand + "'";
                    sql += " AND created_date BETWEEN '" + start_date + "' AND '" + end_date + "' ";

                    db_purchase.Retrieve(purchaseDataGridView, sql, lbl_p4_2_total);

                    setTip(StatusType.SELECT);

                    if (lbl_p4_2_total.Text == "0")
                        setTip(StatusType.NULL);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Page5_1 進階搜尋選項 (新增貨料)
        private void searchAddGoods()
        {
            string searchStr;
            string sql = "";

            try
            {
                searchStr = txt_p5_1_search_string.Text;

                if (searchStr != "")
                {
                    sql += String.Format(" AND (goodsID like '%{0}%' OR type like '%{1}%' OR categoryName like '%{2}%' OR " +
                        "engAlias like '%{3}%' OR spec like '%{4}%' OR brandName like '%{5}%' OR quantity like '%{6}%' OR created_date like '%{7}%' OR updated_date like '%{8}%' OR remark like '%{9}%') "
                        , searchStr, searchStr, searchStr, searchStr, searchStr, searchStr, searchStr, searchStr, searchStr, searchStr);
                }

                db_goods.RetrieveAddSelectPage(addGoodsDataGridView, sql, lbl_p5_1_total);
                setTip(StatusType.SELECT);

                if (lbl_p5_1_total.Text == "0")
                {
                    btn_p5_1_export.Enabled = false;
                    setTip(StatusType.NULL);
                }
                else
                {
                    btn_p5_1_export.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                setTip(StatusType.ERROR);
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Page5_2 進階搜尋選項 (新增種類)
        private void searchAddCategory()
        {
            string searchStr;
            string sql = "";

            try
            {
                searchStr = txt_p5_2_search_string.Text;

                if (searchStr != "")
                {
                    sql += String.Format(" AND (categoryID like '%{0}%' OR categoryName like '%{1}%' OR engAlias like '%{2}%' OR " +
                        "remark like '%{3}%' OR created_date like '%{4}%') "
                        , searchStr, searchStr, searchStr, searchStr, searchStr);
                }

                db_category.RetrieveAddSelectPage(categoryDataGridView, sql, lbl_p5_2_total);
                setTip(StatusType.SELECT);

                if (lbl_p5_2_total.Text == "0")
                {
                    btn_p5_2_export.Enabled = false;
                    setTip(StatusType.NULL);
                }
                else
                {
                    btn_p5_2_export.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                setTip(StatusType.ERROR);
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Page5_3 進階搜尋選項 (新增種類)
        private void searchAddBrand()
        {
            string searchStr;
            string sql = "";

            try
            {
                searchStr = txt_p5_3_search_string.Text;

                if (searchStr != "")
                {
                    sql += String.Format(" AND (brandID like '%{0}%' OR brandName like '%{1}%' OR remark like '%{2}%' OR created_date like '%{3}%') "
                        , searchStr, searchStr, searchStr, searchStr);
                }

                db_brand.RetrieveAddSelectPage(brandDataGridView, sql, lbl_p5_3_total);
                setTip(StatusType.SELECT);

                if (lbl_p5_3_total.Text == "0")
                {
                    btn_p5_3_export.Enabled = false;
                    setTip(StatusType.NULL);
                }
                else
                {
                    btn_p5_3_export.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                setTip(StatusType.ERROR);
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Page6 進階搜尋選項 (進出人員)
        private void searchStaff()
        {
            string searchStr;
            string sql = "";

            try
            {
                searchStr = txt_p6_search_string.Text;

                if (searchStr != "")
                {
                    sql += String.Format(" AND (id like '%{0}%' OR identity like '%{1}%' OR name like '%{2}%' OR " +
                        "remark like '%{3}%' OR created_date like '%{4}%') "
                        , searchStr, searchStr, searchStr, searchStr, searchStr);
                }

                db_staff.Retrieve(staffDataGridView, sql, lbl_p6_total);
                setTip(StatusType.SELECT);

                if (lbl_p6_total.Text == "0")
                {
                    btn_p6_export.Enabled = false;
                    setTip(StatusType.NULL);
                }
                else
                {
                    btn_p6_export.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                setTip(StatusType.ERROR);
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 設定狀態動作，給予提示
        private void setTip(StatusType statusType)
        {
            switch (statusType)
            {
                case StatusType.SELECT:
                    lblStatus.Text = DateTime.Now.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) +
                                                            " - 搜尋成功";
                    lblStatus.BackColor = Color.FromArgb(128, 128, 255);
                    lblStatus.ForeColor = Color.Yellow;
                    break;
                case StatusType.UPDATE:
                    lblStatus.Text = DateTime.Now.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) +
                                                            " - 資料修改成功";
                    lblStatus.BackColor = Color.FromArgb(0, 64, 64);
                    lblStatus.ForeColor = Color.FromArgb(192, 255, 255);
                    break;
                case StatusType.INSERT:
                    lblStatus.Text = DateTime.Now.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) +
                                                            " - 資料新增成功";
                    lblStatus.BackColor = Color.Green;
                    lblStatus.ForeColor = Color.FromArgb(192, 255, 255);
                    break;
                case StatusType.DELETE:
                    lblStatus.Text = DateTime.Now.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) +
                                                            " - 資料刪除成功";
                    lblStatus.BackColor = Color.FromArgb(192, 0, 0);
                    lblStatus.ForeColor = Color.Yellow;
                    break;
                case StatusType.REFRESH:
                    lblStatus.Text = DateTime.Now.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) +
                                                            " - 資料重新載入成功";
                    lblStatus.BackColor = Color.LawnGreen;
                    lblStatus.ForeColor = Color.DarkSlateGray;
                    break;
                case StatusType.NULL:
                    lblStatus.Text = DateTime.Now.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) +
                                                            " - 查無資料";
                    lblStatus.BackColor = Color.Olive;
                    lblStatus.ForeColor = Color.White;
                    break;
                case StatusType.BLANKWARN:
                    lblStatus.Text = DateTime.Now.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) +
                                                            " - 執行失敗，必填欄位不得空白";
                    lblStatus.BackColor = Color.Red;
                    lblStatus.ForeColor = Color.Yellow;
                    break;
                case StatusType.EXPORTSCCESS:
                    lblStatus.Text = DateTime.Now.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) +
                                                            " - Excel匯出成功";
                    lblStatus.BackColor = Color.FromArgb(0, 0, 128);
                    lblStatus.ForeColor = Color.White;
                    break;
                case StatusType.PRINTSCCESS:
                    lblStatus.Text = DateTime.Now.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) +
                                                            " - 列印成功";
                    lblStatus.BackColor = Color.Navy;
                    lblStatus.ForeColor = Color.Snow;
                    break;
                case StatusType.ERROR:
                    lblStatus.Text = DateTime.Now.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) +
                                                            " - 發生錯誤，執行失敗";
                    lblStatus.BackColor = Color.FromArgb(139, 0, 0);
                    lblStatus.ForeColor = Color.Yellow;
                    break;
                case StatusType.CANCEL:
                    lblStatus.Text = DateTime.Now.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) +
                                                            " - 動作取消";
                    lblStatus.BackColor = Color.FromArgb(192, 255, 192);
                    lblStatus.ForeColor = Color.FromArgb(0, 0, 64);
                    break;
                case StatusType.NUMCOLUMNONLY:
                    lblStatus.Text = DateTime.Now.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) +
                                                            " - 只允許輸入進貨數量";
                    lblStatus.BackColor = Color.FromArgb(139, 0, 0);
                    lblStatus.ForeColor = Color.Yellow;
                    break;
                case StatusType.NONE:
                    lblStatus.Text = DateTime.Now.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) +
                                                            " - 提示窗口";
                    lblStatus.BackColor = Color.FromArgb(192, 255, 192);
                    lblStatus.ForeColor = Color.FromArgb(0, 0, 64);
                    break;
            }
        }

        // 匯出EXCEL方法
        private void exportExcel(DataGridView dataGridView, string title)
        {
            string saveFileName = "";
            string fileName = "";
            string titleFull = "";

            progressBar.Value = 0;

            fileName = title + " - " + DateTime.Now.ToString("yyyy-MM-dd HH時mm分ss秒");
            titleFull = "【秋田機械股份有限公司】(" + title + ") (製表日期：" + DateTime.Now.ToString("yyyy-MM-dd ddd HH:mm:ss") + ")";

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xlsx";
            saveDialog.Filter = "Excel文件|*.xlsx";
            saveDialog.FileName = fileName;
            saveDialog.ShowDialog();
            saveFileName = saveDialog.FileName;
            if (saveFileName.IndexOf(":") < 0) return; //被點了取消
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("無法建立Excel物件，可能您的電腦未安裝Excel", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                setTip(StatusType.ERROR);
                return;
            }
            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1

            worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, dataGridView.ColumnCount]].Merge();
            worksheet.Cells[1, 1] = titleFull;
            worksheet.Name = "Report";

            // 創造單元格
            Microsoft.Office.Interop.Excel.Range range;
            // 起始單元格(titleFull)
            range = worksheet.Cells[1, 1];
            range.Font.Bold = true;
            range.Font.Size = 12;
            range.Font.Name = "Consolas";
            // range.Interior.ColorIndex = 16;

            //水平居中
            range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //垂直居中
            range.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

            // 調整欄位最適寬度
            if (title == "貨料記錄表")
            {
                range = worksheet.Cells[2, 1];  // 編號
                range.ColumnWidth = 6.75;
                range = worksheet.Cells[2, 2];  // 型號
                range.ColumnWidth = 18;
                range = worksheet.Cells[2, 3];  // 種類名稱
                range.ColumnWidth = 16;
                range = worksheet.Cells[2, 4];  // 種類代號
                range.ColumnWidth = 11;
                range = worksheet.Cells[2, 5];  // 規格
                range.ColumnWidth = 25;
                range = worksheet.Cells[2, 6];  // 廠牌
                range.ColumnWidth = 13.5;
                range = worksheet.Cells[2, 7];  // 條碼號
                range.ColumnWidth = 13;
                range = worksheet.Cells[2, 8];  // 數量
                range.ColumnWidth = 7;
                range = worksheet.Cells[2, 9];  // 備註
                range.ColumnWidth = 18;
            }

            //寫入標題
            for (int i = 0; i < dataGridView.ColumnCount; i++)
            {
                worksheet.Cells[2, i + 1] = dataGridView.Columns[i].HeaderText;
                range = worksheet.Cells[2, i + 1];
                range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                range.Font.Bold = true;
                range.Font.Size = 14;
                range.Font.Name = "Consolas";
                range.Interior.Color = Color.LightGray;
                range.Borders.LineStyle = 1;
            }

            progressBar.Visible = true;
            progressBar.Maximum = dataGridView.Rows.Count * dataGridView.ColumnCount;


            //寫入數值
            for (int r = 0; r < dataGridView.Rows.Count; r++)
            {
                for (int i = 0; i < dataGridView.ColumnCount; i++)
                {
                    range = worksheet.Cells[r + 3, i + 1];
                    range.Font.Name = "Consolas";
                    range.Borders.LineStyle = 1; // 表格框線
                    range.WrapText = true; // 自動換行

                    if ((r + 1) % 3 == 0)
                        range.Interior.Color = Color.FromArgb(224, 255, 255);

                    worksheet.Cells[r + 3, i + 1] = dataGridView.Rows[r].Cells[i].Value;

                    // 進度條
                    progressBar.Value += 1;
                }
                System.Windows.Forms.Application.DoEvents();
            }

            worksheet.Columns.EntireColumn.AutoFit();//列寬自適應
                                                     //worksheet.Columns.NumberFormatLocal = "@";
            progressBar.Visible = false;

            if (saveFileName != "")
            {
                try
                {
                    workbook.Saved = true;
                    workbook.SaveCopyAs(saveFileName);
                }
                catch (Exception ex)
                {
                    setTip(StatusType.ERROR);
                    MessageBox.Show("匯出文件時出錯,文件可能正被開啟！\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            xlApp.Quit();
            worksheet = null;
            GC.Collect();//強行銷燬

            setTip(StatusType.EXPORTSCCESS);
            MessageBox.Show("文件： " + fileName + ".xls 儲存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 列印功能
        private void printDoc(DataGridView dataGridView, string title)
        {
            PrintHelper.GetTempXlsx(dataGridView, title, progressBar);

            Workbook workbook = new Workbook();
            workbook.LoadFromFile("C:\\ct_temp\\printTemp.xlsx");

            Worksheet sheet = workbook.Worksheets[0];

            //sheet.PageSetup.PrintArea = "A7:T8";
            //sheet.PageSetup.PrintTitleRows = "$1:$1";
            sheet.PageSetup.FitToPagesWide = 1;
            sheet.PageSetup.FitToPagesTall = 1;
            sheet.PageSetup.Orientation = PageOrientationType.Landscape;
            //sheet.PageSetup.PaperSize = PaperSizeType.PaperA3;

            PrintDialog dialog = new PrintDialog();
            dialog.AllowPrintToFile = true;
            dialog.AllowCurrentPage = true;
            dialog.AllowSomePages = true;
            dialog.AllowSelection = true;
            dialog.UseEXDialog = true;
            dialog.PrinterSettings.Duplex = Duplex.Simplex;
            dialog.PrinterSettings.FromPage = 0;
            dialog.PrinterSettings.ToPage = 8;
            dialog.PrinterSettings.PrintRange = PrintRange.SomePages;
            workbook.PrintDialog = dialog;
            PrintDocument pd = workbook.PrintDocument;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
                setTip(StatusType.PRINTSCCESS);
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////

        private void 關於本程式ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("開發人員：羅仕宏\n版本號：" + toolStripStatusLabel5.Text +
                            "\n條碼格式：code 39\n程式最後修正時間：" + FINAL_UPDATED +
                            "\n\n秋田機械股份有限公司 版權所有\nCopyright © 2018 Chiu-Tian Machinery CO., Ltd. All Rights Reserved",
                            "關於本程式", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            InitialSetting.RefreshStatusStrip(statusStrip1);
        }

        private void btn_p2_refresh_Click(object sender, EventArgs e)
        {
            if (checkGroupBox_p2.Checked)
                searchGoods();
            else
                db_goods.Retrieve(goodsDataGridView, lbl_p2_total);

            setTip(StatusType.REFRESH);
        }

        private void goodsDataGridView_Sorted(object sender, EventArgs e)
        {
            // 點擊dataGridView上的欄位排序時重新隔行刷色
            gridViewStyleSetting(goodsDataGridView);
        }

        private void goodsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            panel_p2.Enabled = true;

            setTip(StatusType.NONE);

            lbl_p2_status.BackColor = Color.LightGray;
            lbl_p2_status.ForeColor = Color.Black;
            btn_p2_delete.BackColor = Color.FromArgb(212, 63, 58);
            btn_p2_delete.ForeColor = Color.White;
            btn_p2_update.BackColor = Color.MediumSeaGreen;
            btn_p2_update.ForeColor = Color.White;

            lbl_p2_status.Text = "資料編輯";

            txt_p2_goodsID.Text = goodsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_p2_type.Text = goodsDataGridView.CurrentRow.Cells[1].Value.ToString();
            txt_p2_categoryName.Text = goodsDataGridView.CurrentRow.Cells[2].Value.ToString();
            txt_p2_engAlias.Text = goodsDataGridView.CurrentRow.Cells[3].Value.ToString();
            txt_p2_spec.Text = goodsDataGridView.CurrentRow.Cells[4].Value.ToString();
            txt_p2_brandName.Text = goodsDataGridView.CurrentRow.Cells[5].Value.ToString();

            bc_p2.IncludeLabel = true;//是否顯示標籤Label
            bc_p2.LabelFont = new Font("Verdana", 8f);//標籤字型與大小
            bc_p2.Width = 299;//標籤寬度
            bc_p2.Height = 69;//標籤高度
            try
            {
                Image img = bc_p2.Encode(TYPE.CODE39, goodsDataGridView.CurrentRow.Cells[6].Value.ToString());
                picBox_p2_barcode.Image = img;
            }
            catch (Exception ex)
            {
                MessageBox.Show("條碼太長" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txt_p2_quantity.Text = goodsDataGridView.CurrentRow.Cells[7].Value.ToString();
            txt_p2_remark.Text = goodsDataGridView.CurrentRow.Cells[8].Value.ToString();
        }

        private void txt_p2_quantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = checkNumFormat(e);
        }

        private void txt_p2_quantity_TextChanged(object sender, EventArgs e)
        {
            checkNumRange(txt_p2_quantity, 0, 999999);
        }

        private void btn_p2_update_Click(object sender, EventArgs e)
        {
            // 所修改的goods編號
            int goodsID = Int32.Parse(txt_p2_goodsID.Text);
            int currentRow = goodsDataGridView.CurrentRow.Index;

            if (txt_p2_quantity.Text == "")
            {
                setTip(StatusType.BLANKWARN);
                txt_p2_quantity.Focus();
            }
            else
            {
                db_goods.Update(goodsID, txt_p2_spec.Text.Trim(), txt_p2_remark.Text.Trim(), Int32.Parse(txt_p2_quantity.Text));

                if (checkGroupBox_p2.Checked)
                    searchGoods();
                else
                    db_goods.Retrieve(goodsDataGridView, lbl_p2_total);

                goodsDataGridView.Rows[currentRow].Selected = true;
                txt_p2_goodsID.Text = goodsDataGridView.Rows[currentRow].Cells[0].Value.ToString();
                txt_p2_type.Text = goodsDataGridView.Rows[currentRow].Cells[1].Value.ToString();
                txt_p2_categoryName.Text = goodsDataGridView.Rows[currentRow].Cells[2].Value.ToString();
                txt_p2_engAlias.Text = goodsDataGridView.Rows[currentRow].Cells[3].Value.ToString();
                txt_p2_spec.Text = goodsDataGridView.Rows[currentRow].Cells[4].Value.ToString();
                txt_p2_brandName.Text = goodsDataGridView.Rows[currentRow].Cells[5].Value.ToString();

                Image img = bc_p2.Encode(TYPE.CODE39, goodsDataGridView.Rows[currentRow].Cells[6].Value.ToString());
                picBox_p2_barcode.Image = img;

                txt_p2_quantity.Text = goodsDataGridView.Rows[currentRow].Cells[7].Value.ToString();
                txt_p2_remark.Text = goodsDataGridView.Rows[currentRow].Cells[8].Value.ToString();

                setTip(StatusType.UPDATE);
            }
        }

        private void btn_p2_delete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("確認要刪除編號: " + txt_p2_goodsID.Text + " 的數據嗎？", "刪除確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (dialogResult == DialogResult.OK)
            {
                int goodsID = Int32.Parse(txt_p2_goodsID.Text);

                if (db_goods.CheckQuantity(goodsID))
                {
                    db_goods.Delete(goodsID);

                    if (checkGroupBox_p2.Checked)
                        searchGoods();
                    else
                        db_goods.Retrieve(goodsDataGridView, lbl_p2_total);

                    setTip(StatusType.DELETE);
                }
                else
                    setTip(StatusType.ERROR);
            }
            else
                setTip(StatusType.CANCEL);
        }

        private void txt_p2_search_quantity_TextChanged(object sender, EventArgs e)
        {
            searchGoods();
        }

        private void txt_p2_search_quantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = checkNumFormat(e);
        }

        private void cbo_p2_search_category_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchGoods();
        }

        private void cbo_p2_search_brand_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchGoods();
        }

        private void txt_p2_search_string_TextChanged(object sender, EventArgs e)
        {
            searchGoods();
        }

        // 換分頁時初始化 (主頁)
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.tabControl1.SelectedIndex + 1)
            {
                case 1:
                    // TODO
                    break;
                case 2:
                    if (checkGroupBox_p2.Checked)
                        searchGoods();
                    else
                        db_goods.Retrieve(goodsDataGridView, lbl_p2_total);
                    setTip(StatusType.REFRESH);
                    break;
                case 3:
                    if (checkGroupBox_p3.Checked)
                        searchBring();
                    else
                        db_bring.Retrieve(bringDataGridView, lbl_p3_total);

                    setTip(StatusType.REFRESH);
                    break;
                case 4:
                    if (checkGroupBox_p4_1.Checked)
                        searchPurchaseGoods();
                    else
                        db_goods.Retrieve(purchaseGoodsDataGridView, lbl_p4_1_total);
                    purchaseGoodsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    setTip(StatusType.REFRESH);
                    if (checkGroupBox_p4_2.Checked)
                        searchPurchase();
                    else
                        db_purchase.Retrieve(purchaseDataGridView, lbl_p4_2_total);
                    setTip(StatusType.REFRESH);
                    // TODO 4_3
                    break;
                case 5:
                    if (txt_p5_1_search_string.Text != "")
                        searchAddGoods();
                    else
                        db_goods.Retrieve(addGoodsDataGridView, lbl_p5_1_total);

                    if (txt_p5_2_search_string.Text != "")
                        searchAddCategory();
                    else
                        db_category.Retrieve(categoryDataGridView, lbl_p5_2_total);

                    if (txt_p5_3_search_string.Text != "")
                        searchAddBrand();
                    else
                        db_brand.Retrieve(brandDataGridView, lbl_p5_3_total);
                    setTip(StatusType.REFRESH);
                    break;
                case 6:
                    if (txt_p6_search_string.Text == "")
                        db_staff.Retrieve(staffDataGridView, lbl_p6_total);
                    else
                        searchStaff();
                    setTip(StatusType.REFRESH);
                    break;
                case 7:
                    System.Environment.Exit(System.Environment.ExitCode);
                    break;

            }
        }


        // 換分頁時初始化 (4-*頁)
        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.tabControl3.SelectedIndex + 1)
            {
                case 1:
                    if (checkGroupBox_p4_1.Checked)
                        searchPurchaseGoods();
                    else
                        db_goods.Retrieve(purchaseGoodsDataGridView, lbl_p4_1_total);
                    purchaseGoodsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    setTip(StatusType.REFRESH);
                    break;
                case 2:
                    if (checkGroupBox_p4_2.Checked)
                        searchPurchase();
                    else
                        db_purchase.Retrieve(purchaseDataGridView, lbl_p4_2_total);
                    setTip(StatusType.REFRESH);
                    break;
                case 3:
                    // TODO
                    break;
            }
        }


        // 換分頁時初始化 (5-*頁)
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.tabControl2.SelectedIndex + 1)
            {
                case 1:
                    if (txt_p5_1_search_string.Text != "")
                        searchAddGoods();
                    else
                        db_goods.Retrieve(addGoodsDataGridView, lbl_p5_1_total);
                    setTip(StatusType.REFRESH);
                    break;
                case 2:
                    if (txt_p5_2_search_string.Text != "")
                        searchAddCategory();
                    else
                        db_category.Retrieve(categoryDataGridView, lbl_p5_2_total);
                    setTip(StatusType.REFRESH);
                    break;
                case 3:
                    if (txt_p5_3_search_string.Text != "")
                        searchAddBrand();
                    else
                        db_brand.Retrieve(brandDataGridView, lbl_p5_3_total);
                    setTip(StatusType.REFRESH);
                    break;
            }
        }


        private void txt_p2_quantity_KeyDown(object sender, KeyEventArgs e)
        {
            // 按下ENTER觸發更新事件
            if (e.KeyCode == Keys.Enter)
                btn_p2_update.PerformClick();
        }

        // 匯出EXCEL (Page 2)
        private void btn_p2_export_Click(object sender, EventArgs e)
        {
            exportExcel(goodsDataGridView, "庫存記錄表");
        }

        // 列印 (Page 2)
        private void btn_p2_print_Click(object sender, EventArgs e)
        {
            printDoc(goodsDataGridView, "庫存記錄表");
        }

        private void checkGroupBox_p2_CheckedChanged(object sender, EventArgs e)
        {
            btn_p2_refresh.PerformClick();
        }

        private void 資料庫說明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("資料庫版本：MySQL-community-8.0.12.0 \n" +
                            "雲端備份時間：每日晚上22:00 \n" +
                            "Google雲端帳號：test@gmail.com\n  (備份時間前後10分鐘，21:50~22:10 請勿操作本軟體)",
                            "資料庫說明", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void addGoodsDataGridView_Sorted(object sender, EventArgs e)
        {
            // 點擊dataGridView上的欄位排序時重新隔行刷色
            gridViewStyleSetting(addGoodsDataGridView);
        }

        private void txt_p5_1_quantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = checkNumFormat(e);
        }

        private void txt_p5_1_quantity_KeyDown(object sender, KeyEventArgs e)
        {
            // 按下ENTER觸發更新事件
            if (e.KeyCode == Keys.Enter)
                btn_p5_1_add.PerformClick();
        }
        private void txt_p5_1_type_KeyDown(object sender, KeyEventArgs e)
        {
            // 按下ENTER觸發更新事件
            if (e.KeyCode == Keys.Enter)
                btn_p5_1_add.PerformClick();
        }

        private void txt_p5_1_quantity_TextChanged(object sender, EventArgs e)
        {
            checkNumRange(txt_p5_1_quantity, 0, 999999);
        }

        private void btn_p5_1_add_Click(object sender, EventArgs e)
        {
            if ((txt_p5_1_type.Text == "") || (txt_p5_1_quantity.Text == ""))
            {
                setTip(StatusType.BLANKWARN);
                return;
            }
            DialogResult dialogResult = MessageBox.Show("確認要新增貨料嗎？", "新增確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            DialogResult dialogCheckType;
            if (dialogResult == DialogResult.OK)
            {
                string type, spec, remark;
                int categoryID, brandID, quantity;

                type = txt_p5_1_type.Text;
                categoryID = Int32.Parse((cbo_p5_1_category.SelectedValue.ToString()));
                spec = txt_p5_1_spec.Text;
                brandID = Int32.Parse((cbo_p5_1_brand.SelectedValue.ToString()));
                remark = txt_p5_1_remark.Text;
                quantity = Int32.Parse(txt_p5_1_quantity.Text);

                if (db_goods.CheckType(type))
                {
                    dialogCheckType = MessageBox.Show("系統在資料庫中已存在相同型號的貨料資訊，請問你還是要新增嗎？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    if (dialogCheckType == DialogResult.OK)
                    {
                        db_goods.Add(type, categoryID, spec, brandID, remark, quantity);

                        if (txt_p5_1_search_string.Text == "")
                            db_goods.Retrieve(addGoodsDataGridView, lbl_p5_1_total);
                        else
                            searchAddGoods();
                        setTip(StatusType.INSERT);

                        txt_p5_1_type.Text = "";
                        txt_p5_1_spec.Text = "";
                        txt_p5_1_remark.Text = "";
                        txt_p5_1_quantity.Text = "0";
                    }
                    else
                        setTip(StatusType.CANCEL);
                }
                else
                {
                    db_goods.Add(type, categoryID, spec, brandID, remark, quantity);

                    if (txt_p5_1_search_string.Text == "")
                        db_goods.Retrieve(addGoodsDataGridView, lbl_p5_1_total);
                    else
                        searchAddGoods();
                    setTip(StatusType.INSERT);

                    txt_p5_1_type.Text = "";
                    txt_p5_1_spec.Text = "";
                    txt_p5_1_remark.Text = "";
                    txt_p5_1_quantity.Text = "0";
                }
            }
            else
                setTip(StatusType.CANCEL);
        }

        private void txt_p5_1_search_string_TextChanged(object sender, EventArgs e)
        {
            searchAddGoods();
        }

        private void btn_p5_1_refresh_Click(object sender, EventArgs e)
        {
            if (txt_p5_1_search_string.Text == "")
                db_goods.Retrieve(addGoodsDataGridView, lbl_p5_1_total);
            else
                searchAddGoods();

            setTip(StatusType.REFRESH);
        }

        private void btn_p5_1_export_Click(object sender, EventArgs e)
        {
            exportExcel(addGoodsDataGridView, "貨料資訊表");
        }

        private void txt_p5_2_search_string_TextChanged(object sender, EventArgs e)
        {
            searchAddCategory();
        }

        private void btn_p5_2_refresh_Click(object sender, EventArgs e)
        {
            if (txt_p5_2_search_string.Text == "")
                db_category.Retrieve(categoryDataGridView, lbl_p5_2_total);
            else
                searchAddCategory();

            setTip(StatusType.REFRESH);
        }

        private void btn_p5_2_export_Click(object sender, EventArgs e)
        {
            exportExcel(categoryDataGridView, "種類資訊表");
        }

        private void btn_p5_2_add_Click(object sender, EventArgs e)
        {

            if ((txt_p5_2_categoryName.Text == "") || (txt_p5_2_engAlias.Text == ""))
            {
                setTip(StatusType.BLANKWARN);
                return;
            }
            DialogResult dialogResult = MessageBox.Show("確認要新增種類嗎？", "新增確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            DialogResult dialogCheckDuplicate;
            if (dialogResult == DialogResult.OK)
            {
                string categoryName, engAlias, remark;

                categoryName = txt_p5_2_categoryName.Text;
                engAlias = txt_p5_2_engAlias.Text;
                remark = txt_p5_2_remark.Text;

                // 防止兩個欄位資料一模一樣
                if (db_category.CheckSame(categoryName, engAlias))
                {
                    MessageBox.Show("資料庫中已存在相同【種類名稱】與【種類代號】完全相同的資料，新增失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    setTip(StatusType.ERROR);
                    return;
                }

                if (db_category.CheckDuplicate(categoryName, engAlias))
                {
                    dialogCheckDuplicate = MessageBox.Show("系統在資料庫中已存在相同【種類名稱】或【種類代號】的資料，請問你還是要新增嗎？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    if (dialogCheckDuplicate == DialogResult.OK)
                    {
                        db_category.Add(categoryName, engAlias, remark);

                        if (txt_p5_2_search_string.Text == "")
                            db_category.Retrieve(categoryDataGridView, lbl_p5_2_total);
                        else
                            searchAddCategory();

                        setTip(StatusType.INSERT);

                        txt_p5_2_categoryName.Text = "";
                        txt_p5_2_engAlias.Text = "";
                        txt_p5_2_remark.Text = "";


                        db_category.InitAddCategoryCombobox(cbo_p5_1_category);
                        db_category.InitCategoryCombobox(cbo_p2_search_category);
                        db_category.InitCategoryCombobox(cbo_p4_1_search_category);
                        db_category.InitCategoryCombobox(cbo_p4_2_search_category);
                    }
                    else
                        setTip(StatusType.CANCEL);
                }
                else
                {
                    db_category.Add(categoryName, engAlias, remark);

                    if (txt_p5_2_search_string.Text == "")
                        db_category.Retrieve(categoryDataGridView, lbl_p5_2_total);
                    else
                        searchAddCategory();

                    setTip(StatusType.INSERT);

                    txt_p5_2_categoryName.Text = "";
                    txt_p5_2_engAlias.Text = "";
                    txt_p5_2_remark.Text = "";

                    db_category.InitAddCategoryCombobox(cbo_p5_1_category);
                    db_category.InitCategoryCombobox(cbo_p2_search_category);
                    db_category.InitCategoryCombobox(cbo_p4_1_search_category);
                    db_category.InitCategoryCombobox(cbo_p4_2_search_category);
                }
            }
            else
                setTip(StatusType.CANCEL);

        }

        private void txt_p5_2_categoryName_KeyDown(object sender, KeyEventArgs e)
        {
            // 按下ENTER觸發更新事件
            if (e.KeyCode == Keys.Enter)
                btn_p5_2_add.PerformClick();
        }

        private void txt_p5_2_engAlias_KeyDown(object sender, KeyEventArgs e)
        {
            // 按下ENTER觸發更新事件
            if (e.KeyCode == Keys.Enter)
                btn_p5_2_add.PerformClick();
        }

        private void categoryDataGridView_Sorted(object sender, EventArgs e)
        {
            // 點擊dataGridView上的欄位排序時重新隔行刷色
            gridViewStyleSetting(categoryDataGridView);
        }

        private void txt_p5_2_engAlias_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (((int)e.KeyChar < 65 | (int)e.KeyChar > 90) & ((int)e.KeyChar < 48 | (int)e.KeyChar > 57) & (int)e.KeyChar != 8)
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void categoryDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                try
                {
                    int categoryID = Int32.Parse(categoryDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());

                    DialogResult dialogResult = MessageBox.Show("確認要刪除種類編號: " + categoryID + " 的數據嗎？", "刪除確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                    if (dialogResult == DialogResult.OK)
                    {
                        if (db_category.Delete(categoryID))
                        {
                            if (txt_p5_2_search_string.Text == "")
                                db_category.Retrieve(categoryDataGridView, lbl_p5_2_total);
                            else
                                searchAddCategory();

                            db_category.InitAddCategoryCombobox(cbo_p5_1_category);
                            db_category.InitCategoryCombobox(cbo_p2_search_category);
                            db_category.InitCategoryCombobox(cbo_p4_1_search_category);
                            db_category.InitCategoryCombobox(cbo_p4_2_search_category);

                            setTip(StatusType.DELETE);
                        }
                        else
                            setTip(StatusType.ERROR);
                    }
                    else
                        setTip(StatusType.CANCEL);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("請按下欲刪除的資料所對應的刪除按鈕\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    setTip(StatusType.ERROR);
                    return;
                }
            }
        }

        private void txt_p5_3_search_string_TextChanged(object sender, EventArgs e)
        {
            searchAddBrand();
        }

        private void btn_p5_3_add_Click(object sender, EventArgs e)
        {
            if (txt_p5_3_brandName.Text == "")
            {
                setTip(StatusType.BLANKWARN);
                return;
            }
            DialogResult dialogResult = MessageBox.Show("確認要新增廠牌嗎？", "新增確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            if (dialogResult == DialogResult.OK)
            {
                string brandName, remark;

                brandName = txt_p5_3_brandName.Text;
                remark = txt_p5_3_remark.Text;

                if (db_brand.Add(brandName, remark))
                {
                    if (txt_p5_3_search_string.Text == "")
                        db_brand.Retrieve(brandDataGridView, lbl_p5_3_total);
                    else
                        searchAddBrand();

                    txt_p5_3_brandName.Text = "";
                    txt_p5_3_remark.Text = "";

                    setTip(StatusType.INSERT);

                    db_brand.InitBrandCombobox(cbo_p2_search_brand);
                    db_brand.InitAddBrandCombobox(cbo_p5_1_brand);
                    db_brand.InitBrandCombobox(cbo_p4_1_search_brand);
                    db_brand.InitBrandCombobox(cbo_p4_2_search_brand);
                }
                else
                    setTip(StatusType.ERROR);
            } else
                setTip(StatusType.CANCEL);
        }

        private void btn_p5_3_refresh_Click(object sender, EventArgs e)
        {
            if (txt_p5_3_search_string.Text == "")
                db_brand.Retrieve(brandDataGridView, lbl_p5_3_total);
            else
                searchAddBrand();

            setTip(StatusType.REFRESH);
        }

        private void btn_p5_3_export_Click(object sender, EventArgs e)
        {
            exportExcel(brandDataGridView, "廠牌資訊表");
        }

        private void brandDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                try
                {
                    int brandID = Int32.Parse(brandDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());

                    DialogResult dialogResult = MessageBox.Show("確認要刪除廠牌編號: " + brandID + " 的數據嗎？", "刪除確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                    if (dialogResult == DialogResult.OK)
                    {
                        if (db_brand.Delete(brandID))
                        {
                            if (txt_p5_3_search_string.Text == "")
                                db_brand.Retrieve(brandDataGridView, lbl_p5_3_total);
                            else
                                searchAddBrand();

                            db_brand.InitBrandCombobox(cbo_p2_search_brand);
                            db_brand.InitAddBrandCombobox(cbo_p5_1_brand);
                            db_brand.InitBrandCombobox(cbo_p4_1_search_brand);
                            db_brand.InitBrandCombobox(cbo_p4_2_search_brand);

                            setTip(StatusType.DELETE);
                        }
                        else
                            setTip(StatusType.ERROR);
                    }
                    else
                        setTip(StatusType.CANCEL);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("請按下欲刪除的資料所對應的刪除按鈕\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    setTip(StatusType.ERROR);
                    return;
                }
            }
        }

        private void txt_p5_3_brandName_KeyDown(object sender, KeyEventArgs e)
        {
            // 按下ENTER觸發更新事件
            if (e.KeyCode == Keys.Enter)
                btn_p5_3_add.PerformClick();
        }

        private void brandDataGridView_Sorted(object sender, EventArgs e)
        {
            // 點擊dataGridView上的欄位排序時重新隔行刷色
            gridViewStyleSetting(brandDataGridView);
        }

        private void categoryDataGridView_DoubleClick(object sender, EventArgs e)
        {
            CategoryForm categoryForm = new CategoryForm();

            categoryForm.Txt_categoryID = categoryDataGridView.CurrentRow.Cells[0].Value.ToString();
            categoryForm.Txt_categoryName = categoryDataGridView.CurrentRow.Cells[1].Value.ToString();
            categoryForm.Txt_engAlias = categoryDataGridView.CurrentRow.Cells[2].Value.ToString();
            categoryForm.Txt_remark = categoryDataGridView.CurrentRow.Cells[3].Value.ToString();
            categoryForm.Txt_created_date = categoryDataGridView.CurrentRow.Cells[4].Value.ToString();

            categoryForm.ShowDialog();

            btn_p5_2_refresh.PerformClick();
        }

        private void brandDataGridView_DoubleClick(object sender, EventArgs e)
        {
            BrandForm brandForm = new BrandForm();

            brandForm.Txt_brandID = brandDataGridView.CurrentRow.Cells[0].Value.ToString();
            brandForm.Txt_brandName = brandDataGridView.CurrentRow.Cells[1].Value.ToString();
            brandForm.Txt_remark = brandDataGridView.CurrentRow.Cells[2].Value.ToString();
            brandForm.Txt_created_date = brandDataGridView.CurrentRow.Cells[3].Value.ToString();

            brandForm.ShowDialog();

            btn_p5_3_refresh.PerformClick();
        }

        private void txt_p6_search_string_TextChanged(object sender, EventArgs e)
        {
            searchStaff();
        }

        private void btn_p6_refresh_Click(object sender, EventArgs e)
        {
            if (txt_p6_search_string.Text == "")
                db_staff.Retrieve(staffDataGridView, lbl_p6_total);
            else
                searchStaff();

            setTip(StatusType.REFRESH);

        }

        private void btn_p6_export_Click(object sender, EventArgs e)
        {
            exportExcel(staffDataGridView, "進出人員資訊表");
        }

        private void btn_p6_add_Click(object sender, EventArgs e)
        {
            string identity = txt_p6_identity.Text;

            if ((txt_p6_identity.Text == "") || (txt_p6_name.Text == ""))
            {
                setTip(StatusType.BLANKWARN);
                return;
            } else if (db_staff.CheckDuplicate(identity))
            {
                MessageBox.Show("資料庫已存在此【身分證字號】，不得重複，新增失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                setTip(StatusType.ERROR);
                return;
            } else if(!db_staff.IsIdentificationId(identity))
            {
                MessageBox.Show("身分證字號格式有誤，執行失敗！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                setTip(StatusType.ERROR);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("確認要新增人員嗎？", "新增確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            if (dialogResult == DialogResult.OK)
            {
                string name, remark;
                name = txt_p6_name.Text;
                remark = txt_p6_remark.Text;


                db_staff.Add(identity, name, remark);

                if (txt_p6_search_string.Text == "")
                    db_staff.Retrieve(staffDataGridView, lbl_p6_total);
                else
                    searchStaff();

                setTip(StatusType.INSERT);

                txt_p6_identity.Text = "";
                txt_p6_name.Text = "";
                txt_p6_remark.Text = "";
            }
            else
                setTip(StatusType.CANCEL);
        }

        private void txt_p6_identity_KeyDown(object sender, KeyEventArgs e)
        {
            // 按下ENTER觸發更新事件
            if (e.KeyCode == Keys.Enter)
                btn_p6_add.PerformClick();

        }

        private void txt_p6_name_KeyDown(object sender, KeyEventArgs e)
        {
            // 按下ENTER觸發更新事件
            if (e.KeyCode == Keys.Enter)
                btn_p6_add.PerformClick();
        }

        private void staffDataGridView_Sorted(object sender, EventArgs e)
        {
            gridViewStyleSetting(staffDataGridView);
        }

        private void staffDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                try
                {
                    int id = Int32.Parse(staffDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
                    string identity, name;
                    identity = staffDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                    name = staffDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();

                    DialogResult dialogResult = MessageBox.Show("確認要刪除人員\n 編號: " + id + "\n 身分證字號: " + identity + "\n 姓名: " + name + "\n的數據嗎？", "刪除確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                    if (dialogResult == DialogResult.OK)
                    {
                        if (db_staff.Delete(id))
                        {
                            if (txt_p6_search_string.Text == "")
                                db_staff.Retrieve(staffDataGridView, lbl_p6_total);
                            else
                                searchStaff();

                            setTip(StatusType.DELETE);
                        }
                        else
                            setTip(StatusType.ERROR);
                    }
                    else
                        setTip(StatusType.CANCEL);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("請按下欲刪除的資料所對應的刪除按鈕\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    setTip(StatusType.ERROR);
                    return;
                }
            }

        }

        private void staffDataGridView_DoubleClick(object sender, EventArgs e)
        {
            StaffForm staffForm = new StaffForm();

            staffForm.Txt_id = staffDataGridView.CurrentRow.Cells[0].Value.ToString();
            staffForm.Txt_identity = staffDataGridView.CurrentRow.Cells[1].Value.ToString();
            staffForm.Txt_name = staffDataGridView.CurrentRow.Cells[2].Value.ToString();
            staffForm.Txt_remark = staffDataGridView.CurrentRow.Cells[3].Value.ToString();
            staffForm.Txt_created_date = staffDataGridView.CurrentRow.Cells[4].Value.ToString();

            staffForm.ShowDialog();

            btn_p6_refresh.PerformClick();

        }

        private void btn_p3_refresh_Click(object sender, EventArgs e)
        {
            if (checkGroupBox_p3.Checked)
                searchBring();
            else
                db_bring.Retrieve(bringDataGridView, lbl_p3_total);

            setTip(StatusType.REFRESH);
        }

        private void checkGroupBox_p3_CheckedChanged(object sender, EventArgs e)
        {
            btn_p3_refresh.PerformClick();
        }

        private void txt_p3_search_string_TextChanged(object sender, EventArgs e)
        {
            searchBring();
        }

        private void cbo_p3_search_staff_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchBring();
        }

        private void chk_p3_out_CheckedChanged(object sender, EventArgs e)
        {
            searchBring();
        }

        private void chk_p3_in_CheckedChanged(object sender, EventArgs e)
        {
            searchBring();
        }

        private void bringDataGridView_Sorted(object sender, EventArgs e)
        {
            // 點擊dataGridView上的欄位排序時重新隔行刷色
            gridViewStyleSetting(bringDataGridView);
        }

        private void dtp_p3_start_ValueChanged(object sender, EventArgs e)
        {
            searchBring();
        }

        private void dtp_p3_end_ValueChanged(object sender, EventArgs e)
        {
            searchBring();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void btn_p4_1_refresh_Click(object sender, EventArgs e)
        {
            if (checkGroupBox_p4_1.Checked)
                searchPurchaseGoods();
            else
                db_goods.Retrieve(purchaseGoodsDataGridView, lbl_p4_1_total);
            purchaseGoodsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            setTip(StatusType.REFRESH);
        }

        private void cbo_p4_1_search_category_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchPurchaseGoods();
        }

        private void cbo_p4_1_search_brand_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchPurchaseGoods();
        }

        private void txt_p4_1_search_string_TextChanged(object sender, EventArgs e)
        {
            searchPurchaseGoods();
        }

        private void purchaseGoodsDataGridView_Sorted(object sender, EventArgs e)
        {
            gridViewStyleSetting(purchaseGoodsDataGridView);
        }

        private void purchaseGoodsDataGridView_DoubleClick(object sender, EventArgs e)
        {
            int goodsID = Int32.Parse(purchaseGoodsDataGridView.CurrentRow.Cells[0].Value.ToString());

            if (purchasePrepareDataGridView.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in purchasePrepareDataGridView.Rows)
                {
                    if(goodsID == Int32.Parse(row.Cells[0].Value.ToString()))
                    {
                        MessageBox.Show("該貨料已加入於下方預備進貨表格了，\n請勿重複加入。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        setTip(StatusType.CANCEL);
                        return;
                    }
                }
            }

            purchasePrepareDataGridView.Rows.Add(purchaseGoodsDataGridView.CurrentRow.Cells[0].Value.ToString(),
                                                 purchaseGoodsDataGridView.CurrentRow.Cells[1].Value.ToString(),
                                                 purchaseGoodsDataGridView.CurrentRow.Cells[2].Value.ToString(),
                                                 purchaseGoodsDataGridView.CurrentRow.Cells[3].Value.ToString(),
                                                 purchaseGoodsDataGridView.CurrentRow.Cells[4].Value.ToString(),
                                                 purchaseGoodsDataGridView.CurrentRow.Cells[5].Value.ToString(),
                                                 purchaseGoodsDataGridView.CurrentRow.Cells[6].Value.ToString(),
                                                 purchaseGoodsDataGridView.CurrentRow.Cells[7].Value.ToString(),
                                                 purchaseGoodsDataGridView.CurrentRow.Cells[8].Value.ToString(),
                                                 "0");
        }


        string textBeforeEdit = "0";
        private void purchasePrepareDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string text = (purchasePrepareDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? "").ToString();

            if(text == "")
            {
                MessageBox.Show("不得為空值", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                purchasePrepareDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = textBeforeEdit;
                return;
            }

            if (text.Length > 4)
            {
                MessageBox.Show("輸入不可超過4個字元", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //復原編輯前狀態
                purchasePrepareDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = textBeforeEdit;
            }
        }

        private void purchasePrepareDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 9) //只允許修改第9個Column(存貨數量)
            {
                //儲存編輯前的文字，可以用來復原編輯前的狀態
                //若Value為null，則會設為空字串
                textBeforeEdit = (purchasePrepareDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? "0").ToString();
                return;
            }
            else
            {
                setTip(StatusType.NUMCOLUMNONLY);
                e.Cancel = true; //讓使用者無法繼續進行修改
            }
        }

        private void purchasePrepareDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = (TextBox)e.Control;
            tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
        }
        // 加入KeyPress事件確保cell內只能輸入數字
        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            char key = e.KeyChar;
            int value = (int)key;
            if ((value >= 48 && value <= 57) || value == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void btn_p4_1_purchase_Click(object sender, EventArgs e)
        {
            string sqlValue = "";
            try
            {
                foreach (DataGridViewRow row in purchasePrepareDataGridView.Rows)
                {
                    int goodsID = Int32.Parse(row.Cells[0].Value.ToString());
                    int quantity = Int32.Parse(row.Cells[9].Value.ToString().Trim());
                    if (quantity > 0)
                        sqlValue += "(" + goodsID + ", " + quantity + "),";
                }
                sqlValue = sqlValue.Substring(0, sqlValue.Length - 1);
                sqlValue += ";";

                db_purchase.Add(sqlValue);
                db_purchase.UpdatePurchase();
                purchasePrepareDataGridView.Rows.Clear();

                if (checkGroupBox_p4_1.Checked)
                    searchPurchaseGoods();
                else
                    db_goods.Retrieve(purchaseGoodsDataGridView, lbl_p4_1_total);
                purchaseGoodsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                setTip(StatusType.INSERT);
            }
            catch (Exception ex)
            {
                setTip(StatusType.ERROR);
                MessageBox.Show("請確定所輸入的【進貨數量】有大於0！\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void purchasePrepareDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (purchasePrepareDataGridView.Rows.Count > 0)
                btn_p4_1_purchase.Enabled = true;
            else
                btn_p4_1_purchase.Enabled = false;

            lbl_p4_1_total_prepare.Text = "" + purchasePrepareDataGridView.Rows.Count;
        }

        private void purchasePrepareDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (purchasePrepareDataGridView.Rows.Count > 0)
                btn_p4_1_purchase.Enabled = true;
            else
                btn_p4_1_purchase.Enabled = false;
            lbl_p4_1_total_prepare.Text = "" + purchasePrepareDataGridView.Rows.Count;
        }

        private void purchaseDataGridView_Sorted(object sender, EventArgs e)
        {
            gridViewStyleSetting(purchaseDataGridView);
        }

        private void btn_p4_2_refresh_Click(object sender, EventArgs e)
        {
            if (checkGroupBox_p4_2.Checked)
                searchPurchase();
            else
                db_purchase.Retrieve(purchaseDataGridView, lbl_p4_2_total);
            setTip(StatusType.REFRESH);
        }

        private void btn_p4_2_export_Click(object sender, EventArgs e)
        {
            exportExcel(purchaseDataGridView, "進貨記錄表");
        }

        private void checkGroupBox_p4_1_CheckedChanged(object sender, EventArgs e)
        {
            btn_p4_1_refresh.PerformClick();
        }

        private void checkGroupBox_p4_2_CheckedChanged(object sender, EventArgs e)
        {
            btn_p4_2_refresh.PerformClick();
        }

        private void cbo_p4_2_search_category_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchPurchase();
        }

        private void cbo_p4_2_search_brand_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchPurchase();
        }

        private void txt_p4_2_search_string_TextChanged(object sender, EventArgs e)
        {
            searchPurchase();
        }

        private void dtp_p4_2_start_ValueChanged(object sender, EventArgs e)
        {
            searchPurchase();
        }

        private void dtp_p4_2_end_ValueChanged(object sender, EventArgs e)
        {
            searchPurchase();
        }

        private void btn_p3_export_Click(object sender, EventArgs e)
        {
            exportExcel(bringDataGridView, "攜出入記錄表");
        }

    }
}