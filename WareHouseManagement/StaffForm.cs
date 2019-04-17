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
    public partial class StaffForm : Form
    {
        DB_staff db_staff;

        public StaffForm()
        {
            InitializeComponent();
        }

        public string Txt_id
        {
            get { return this.txt_id.Text; }
            set { this.txt_id.Text = value; }
        }
        public string Txt_identity
        {
            get { return this.txt_identity.Text; }
            set { this.txt_identity.Text = value; }
        }
        public string Txt_name
        {
            get { return this.txt_name.Text; }
            set { this.txt_name.Text = value; }
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

        private void StaffForm_Load(object sender, EventArgs e)
        {
            db_staff = new DB_staff();
            txt_remark.MaxLength = 40;
            this.KeyPreview = true;
            txt_remark.Focus();
        }

        private void StaffForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
                this.Close();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("確認更新嗎？", "新增確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (dialogResult == DialogResult.OK)
            {
                int id = Int32.Parse(txt_id.Text);
                string remark = txt_remark.Text.Trim();

                db_staff.Update(id, remark);

                this.Close();
            }
        }
    }
}
