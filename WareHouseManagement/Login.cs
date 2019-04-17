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
    public partial class Login : Form
    {
        DB_manager db_manager;
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txt_account_number.MaxLength = 35;
            txt_password.MaxLength = 35;
            db_manager = new DB_manager();

            txt_account_number.Text = "8956";
            txt_password.Text = "1234";
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            string account_number, password;

            account_number = txt_account_number.Text;
            password = txt_password.Text;

            if (account_number.Trim() == "" || password.Trim() == "")
            {
                MessageBox.Show("帳號或密碼不得為空！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (db_manager.CheckAccount(account_number, password))
            {
                Form1 form1 = new Form1(db_manager.StaffID, db_manager.Name);
                form1.Visible = true;
                this.Visible = false;
            }
            else
            {
                MessageBox.Show("帳號或密碼錯誤！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                btn_OK.PerformClick();
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                txt_account_number.Text = "8956";
                txt_password.Text = "1234";
                btn_OK.PerformClick();
            }
        }
    }
}
