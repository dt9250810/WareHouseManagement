using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WareHouseManagement
{
    public partial class BrandForm : Form
    {
        DB_brand db_brand;

        public string Txt_brandID
        {
            get { return this.txt_brandID.Text; }
            set { this.txt_brandID.Text = value; }
        }
        public string Txt_brandName
        {
            get { return this.txt_brandName.Text; }
            set { this.txt_brandName.Text = value; }
        }
        public string Txt_remark
        {
            get { return this.txt_remark.Text; }
            set { this.txt_remark.Text = value; }
        }
        public string Txt_created_date
        {
            get { return this.txt_created_date.Text; }
            set { this.txt_created_date.Text = value; }
        }

        public BrandForm()
        {
            InitializeComponent();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("確認更新嗎？", "新增確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (dialogResult == DialogResult.OK)
            {
                int brandID = Int32.Parse(txt_brandID.Text);
                string remark = txt_remark.Text.Trim();

                db_brand.Update(brandID, remark);

                this.Close();
            }
        }

        private void BrandForm_Load(object sender, EventArgs e)
        {
            db_brand = new DB_brand();
            txt_remark.MaxLength = 40;
            this.KeyPreview = true;
            txt_remark.Focus();
        }

        private void BrandForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
                this.Close();
        }
    }
}
