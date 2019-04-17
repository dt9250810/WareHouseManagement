namespace WareHouseManagement
{
    partial class CategoryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CategoryForm));
            this.panel2 = new System.Windows.Forms.Panel();
            this.txt_created_date = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_categoryID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_engAlias = new System.Windows.Forms.TextBox();
            this.btn_update = new System.Windows.Forms.Button();
            this.txt_remark = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.txt_categoryName = new System.Windows.Forms.TextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel2.Controls.Add(this.txt_created_date);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txt_categoryID);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txt_engAlias);
            this.panel2.Controls.Add(this.btn_update);
            this.panel2.Controls.Add(this.txt_remark);
            this.panel2.Controls.Add(this.label37);
            this.panel2.Controls.Add(this.txt_categoryName);
            this.panel2.Controls.Add(this.label38);
            this.panel2.Controls.Add(this.label39);
            this.panel2.Controls.Add(this.label43);
            this.panel2.Location = new System.Drawing.Point(-3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(392, 439);
            this.panel2.TabIndex = 32;
            // 
            // txt_created_date
            // 
            this.txt_created_date.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txt_created_date.Location = new System.Drawing.Point(99, 351);
            this.txt_created_date.Name = "txt_created_date";
            this.txt_created_date.ReadOnly = true;
            this.txt_created_date.Size = new System.Drawing.Size(291, 27);
            this.txt_created_date.TabIndex = 5;
            this.txt_created_date.TabStop = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(17, 346);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 35);
            this.label2.TabIndex = 26;
            this.label2.Text = "建立日期:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_categoryID
            // 
            this.txt_categoryID.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txt_categoryID.Location = new System.Drawing.Point(98, 58);
            this.txt_categoryID.Name = "txt_categoryID";
            this.txt_categoryID.ReadOnly = true;
            this.txt_categoryID.Size = new System.Drawing.Size(291, 27);
            this.txt_categoryID.TabIndex = 1;
            this.txt_categoryID.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(16, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 35);
            this.label1.TabIndex = 24;
            this.label1.Text = "編號:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_engAlias
            // 
            this.txt_engAlias.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txt_engAlias.Location = new System.Drawing.Point(99, 146);
            this.txt_engAlias.Name = "txt_engAlias";
            this.txt_engAlias.ReadOnly = true;
            this.txt_engAlias.Size = new System.Drawing.Size(291, 27);
            this.txt_engAlias.TabIndex = 3;
            this.txt_engAlias.TabStop = false;
            // 
            // btn_update
            // 
            this.btn_update.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btn_update.Font = new System.Drawing.Font("微軟正黑體", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_update.ForeColor = System.Drawing.Color.White;
            this.btn_update.Location = new System.Drawing.Point(292, 384);
            this.btn_update.Name = "btn_update";
            this.btn_update.Size = new System.Drawing.Size(98, 50);
            this.btn_update.TabIndex = 7;
            this.btn_update.Text = "修改";
            this.btn_update.UseVisualStyleBackColor = false;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // txt_remark
            // 
            this.txt_remark.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txt_remark.Location = new System.Drawing.Point(99, 193);
            this.txt_remark.Multiline = true;
            this.txt_remark.Name = "txt_remark";
            this.txt_remark.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_remark.Size = new System.Drawing.Size(291, 144);
            this.txt_remark.TabIndex = 4;
            // 
            // label37
            // 
            this.label37.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label37.Font = new System.Drawing.Font("微軟正黑體", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label37.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label37.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label37.Location = new System.Drawing.Point(3, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(398, 51);
            this.label37.TabIndex = 11;
            this.label37.Text = "編輯種類";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_categoryName
            // 
            this.txt_categoryName.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txt_categoryName.Location = new System.Drawing.Point(99, 105);
            this.txt_categoryName.Name = "txt_categoryName";
            this.txt_categoryName.ReadOnly = true;
            this.txt_categoryName.Size = new System.Drawing.Size(291, 27);
            this.txt_categoryName.TabIndex = 2;
            this.txt_categoryName.TabStop = false;
            // 
            // label38
            // 
            this.label38.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label38.Location = new System.Drawing.Point(17, 100);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(76, 35);
            this.label38.TabIndex = 5;
            this.label38.Text = "種類名稱:";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label39
            // 
            this.label39.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label39.Location = new System.Drawing.Point(17, 188);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(76, 35);
            this.label39.TabIndex = 6;
            this.label39.Text = "備註:";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label43
            // 
            this.label43.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label43.Location = new System.Drawing.Point(17, 141);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(76, 35);
            this.label43.TabIndex = 8;
            this.label43.Text = "種類代號:";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CategoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 454);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CategoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "種類編輯";
            this.Load += new System.EventHandler(this.CategoryForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CategoryForm_KeyPress);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txt_created_date;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_categoryID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_engAlias;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.TextBox txt_remark;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox txt_categoryName;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label43;
    }
}