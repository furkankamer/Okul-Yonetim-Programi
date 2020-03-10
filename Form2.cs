using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WindowsFormsApp1.Helpers;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Person person = JsonConvert.DeserializeObject<Person>(Settings.GeneralSettings);
            kullaniciadi_text.Text = person.User;
            sifre_text.Text = Sqlexecuter($"select sifre from Kisiler where personid = '{person.Id}'", 1);
            email_text.Text = person.Email;
            eskisifre_text.Text = sifre_text.Text;
            eskisifre_text.Hide();
            eskisifre.Hide();
        }

        private void Kaydet_Click(object sender, EventArgs e)
        {
            if (eskisifre_text.Text != sifre_text.Text)
            {
                MessageBox.Show("Eski şifreyi doğru girdiğinizden emin olunuz");
                return;
            }
            Person person = JsonConvert.DeserializeObject<Person>(Settings.GeneralSettings);
            Sqlexecuter($"Update Kisiler set kullaniciadi = '{kullaniciadi_text.Text}', sifre = '{sifre_text.Text}', " +
                $"mail = '{email_text.Text}' where Personid = '{person.Id}'",0);
        }

        private void Sifre_text_TextChanged(object sender, EventArgs e)
        {

            if (!eskisifre.Visible)
                eskisifre_text.Clear();
            eskisifre.Show();
            eskisifre_text.Show();
        }
    }
}
