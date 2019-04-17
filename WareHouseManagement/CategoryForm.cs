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
    public partial class CategoryForm : Form
    {
        DB_category db_category;

        public CategoryForm()
        {
            InitializeComponent();
        }

        public string Txt_categoryID
        {
            get { return this.txt_categoryID.Text; }
            set { this.txt_categoryID.Text = value; }
        }
        public string Txt_categoryName
        {
            get { return this.txt_categoryName.Text; }
            set { this.txt_categoryName.Text = value; }
        }
        public string Txt_engAlias
        {
            get { return this.txt_engAlias.Text; }
            set { this.txt_engAlias.Text = value; }
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

        private void btn_update_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("確認更新嗎？", "新增確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (dialogResult == DialogResult.OK)
            {
                int categoryID = Int32.Parse(txt_categoryID.Text);
                string remark = txt_remark.Text.Trim();

                db_category.Update(categoryID, remark);

                this.Close();
            }
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {
            db_category = new DB_category();
            txt_remark.MaxLength = 40;
            this.KeyPreview = true;
            txt_remark.Focus();
        }

        private void CategoryForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
                this.Close();
        }
    }
}
