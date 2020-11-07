namespace WindowsFormsApp1
{
    partial class Form2
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
            this.kullanici_adi = new System.Windows.Forms.Label();
            this.kullaniciadi_text = new System.Windows.Forms.TextBox();
            this.sifre = new System.Windows.Forms.Label();
            this.sifre_text = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.email_text = new System.Windows.Forms.TextBox();
            this.kaydet = new System.Windows.Forms.Button();
            this.eskisifre = new System.Windows.Forms.Label();
            this.eskisifre_text = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // kullanici_adi
            // 
            this.kullanici_adi.AutoSize = true;
            this.kullanici_adi.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.kullanici_adi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.kullanici_adi.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F);
            this.kullanici_adi.ForeColor = System.Drawing.Color.DarkRed;
            this.kullanici_adi.Location = new System.Drawing.Point(-1, 18);
            this.kullanici_adi.Name = "kullanici_adi";
            this.kullanici_adi.Size = new System.Drawing.Size(138, 26);
            this.kullanici_adi.TabIndex = 45;
            this.kullanici_adi.Text = "Kullanıcı Adı:";
            // 
            // kullaniciadi_text
            // 
            this.kullaniciadi_text.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.kullaniciadi_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.kullaniciadi_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.kullaniciadi_text.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.kullaniciadi_text.Location = new System.Drawing.Point(152, 18);
            this.kullaniciadi_text.Name = "kullaniciadi_text";
            this.kullaniciadi_text.Size = new System.Drawing.Size(116, 29);
            this.kullaniciadi_text.TabIndex = 46;
            // 
            // sifre
            // 
            this.sifre.AutoSize = true;
            this.sifre.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.sifre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sifre.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F);
            this.sifre.ForeColor = System.Drawing.Color.DarkRed;
            this.sifre.Location = new System.Drawing.Point(-1, 56);
            this.sifre.Name = "sifre";
            this.sifre.Size = new System.Drawing.Size(63, 26);
            this.sifre.TabIndex = 47;
            this.sifre.Text = "Şifre:";
            // 
            // sifre_text
            // 
            this.sifre_text.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.sifre_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sifre_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.sifre_text.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.sifre_text.Location = new System.Drawing.Point(152, 53);
            this.sifre_text.Name = "sifre_text";
            this.sifre_text.PasswordChar = '*';
            this.sifre_text.Size = new System.Drawing.Size(116, 29);
            this.sifre_text.TabIndex = 48;
            this.sifre_text.TextChanged += new System.EventHandler(this.Sifre_text_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F);
            this.label1.ForeColor = System.Drawing.Color.DarkRed;
            this.label1.Location = new System.Drawing.Point(-1, 139);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 26);
            this.label1.TabIndex = 49;
            this.label1.Text = "Email:";
            // 
            // email_text
            // 
            this.email_text.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.email_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.email_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.email_text.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.email_text.Location = new System.Drawing.Point(152, 136);
            this.email_text.Name = "email_text";
            this.email_text.Size = new System.Drawing.Size(270, 29);
            this.email_text.TabIndex = 50;
            // 
            // kaydet
            // 
            this.kaydet.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.kaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.kaydet.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F);
            this.kaydet.ForeColor = System.Drawing.Color.DarkRed;
            this.kaydet.Location = new System.Drawing.Point(284, 171);
            this.kaydet.Name = "kaydet";
            this.kaydet.Size = new System.Drawing.Size(120, 35);
            this.kaydet.TabIndex = 51;
            this.kaydet.Text = "Kaydet";
            this.kaydet.UseVisualStyleBackColor = false;
            this.kaydet.Click += new System.EventHandler(this.Kaydet_Click);
            // 
            // eskisifre
            // 
            this.eskisifre.AutoSize = true;
            this.eskisifre.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.eskisifre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.eskisifre.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F);
            this.eskisifre.ForeColor = System.Drawing.Color.DarkRed;
            this.eskisifre.Location = new System.Drawing.Point(-1, 97);
            this.eskisifre.Name = "eskisifre";
            this.eskisifre.Size = new System.Drawing.Size(111, 26);
            this.eskisifre.TabIndex = 52;
            this.eskisifre.Text = "Eski Şifre:";
            // 
            // eskisifre_text
            // 
            this.eskisifre_text.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.eskisifre_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.eskisifre_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.eskisifre_text.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.eskisifre_text.Location = new System.Drawing.Point(152, 94);
            this.eskisifre_text.Name = "eskisifre_text";
            this.eskisifre_text.PasswordChar = '*';
            this.eskisifre_text.Size = new System.Drawing.Size(116, 29);
            this.eskisifre_text.TabIndex = 53;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkRed;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.DarkRed;
            this.button1.Location = new System.Drawing.Point(396, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 26);
            this.button1.TabIndex = 54;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(434, 218);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.eskisifre_text);
            this.Controls.Add(this.eskisifre);
            this.Controls.Add(this.kaydet);
            this.Controls.Add(this.email_text);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sifre_text);
            this.Controls.Add(this.sifre);
            this.Controls.Add(this.kullaniciadi_text);
            this.Controls.Add(this.kullanici_adi);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label kullanici_adi;
        private System.Windows.Forms.TextBox kullaniciadi_text;
        private System.Windows.Forms.Label sifre;
        private System.Windows.Forms.TextBox sifre_text;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox email_text;
        private System.Windows.Forms.Button kaydet;
        private System.Windows.Forms.Label eskisifre;
        private System.Windows.Forms.TextBox eskisifre_text;
        private System.Windows.Forms.Button button1;
    }
}