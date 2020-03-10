using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using static WindowsFormsApp1.Helpers;
using static WindowsFormsApp1.Program;
namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        int ccol = 0;
        int crow = 0;
        public Form1()
        {
            InitializeComponent();
            Settings.GeneralSettings = string.Empty;
            DateTimePickerFormatter(Tarih_Secici);
            sifre.PasswordChar = '*';
            sifre.MaxLength = 10;
            Siniff_Menu.Items.AddRange(Siniflar);
            Sınıf_Menu.Items.AddRange(Siniflar);
            Branslar_Menu.Items.AddRange(Brans_Isimleri);
            Brans_Menu.Items.AddRange(Brans_Isimleri);
            ComboBox[] cmb = { kullanici_tipi,Branslar_Menu, Brans_Hocalar_Menu , Gunler_Menu, Brans_Menu, Saat_Menu, Sınıf_Menu };
            Combobox_dropdown(cmb);
            kullanici_tipi.Items.AddRange(Kullanici_Tipleri);
        }
        
        private bool blnButtonDown = false;
        public SoundPlayer Player { get; } = new SoundPlayer(Properties.Resources.dad2);
        public Color Renk { get; set; }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            if (Tarih_Secici.Value.Date > DateTime.Now && Branslar_Menu.Text != "" && Sınıf_Menu.Text != "" && Saat_Menu.Text != "" && (hoca_from_list.Text != "" || yeni_hoca_text.Text != ""))
            {
                string name;
                if (listeden_sec.Checked) name = hoca_from_list.Text;
                else name = yeni_hoca_text.Text;
                DateTime myDate = Tarih_Secici.Value.Date;
                string gun = Hangi_Gun(Tarih_Secici);
                string date = myDate.ToString("yyyy - MM - dd ");
                date += Saat_Menu.Text;
                string yeniders = $@"insert into Dersler(Sınıf,DersAdi,date2,DersGünü,DersHocasi,quota,enrolled)
                                     values('{Sınıf_Menu.Text}','{Branslar_Menu.Text}','{date}','{gun}','{name}','1','0')";
                if(Sqlexecuter(yeniders,0) == "null")
                    MessageBox.Show("Ders Saatleri Çakışıyor. Lütfen Ders Programını Kontrol Ediniz!");
            }
            else
            {
                MessageBox.Show("Lütfen Bütün Bilgileri Eksiksiz Doldurunuz");
            }
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Saat_Menu.Enabled = true;
            Combobox_clear(new ComboBox[]{ Saat_Menu}, true, true);
            Gun_Saat_Duzenleyici(Tarih_Secici, Saat_Menu);
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Combobox_clear(new ComboBox[] { hoca_from_list }, true, true);
            string hocalar = $@"select Hocalar.isim [Hocalar] from Hocalar
            inner join Dersler on Dersler.hoca_id = Hocalar.hoca_id
            where Dersler.DersAdi = '{Branslar_Menu.Text}'";
            Dictionary<string, List<string>> mydict = Sqlreaderexecuter(hocalar);
            FillComboBoxWithList(hoca_from_list, mydict["hocalar"]);
            AutoCompleteStringCollection allowedTypes = new AutoCompleteStringCollection();
            allowedTypes.AddRange(mydict["hocalar"].Distinct().ToList().ToArray());
            yeni_hoca_text.AutoCompleteCustomSource = allowedTypes;
            yeni_hoca_text.AutoCompleteMode = AutoCompleteMode.Suggest;
            yeni_hoca_text.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }
        private void Giris_Click(object sender, EventArgs e)
        {
            Player.Play();
            Control_hide(new Control[] { Kayit_Paneli, email_paneli });
            Control_show(new Control[] { giriss_paneli , tamam });
        }

        private void Kayit_Click(object sender, EventArgs e)
        {
            Player.Play();
            Control_hide(new Control[] { email_paneli, tamam });
            Control_show(new Control[] { Kayit_Paneli, giriss_paneli });
        }

        private void Tamam_Click(object sender, EventArgs e)
        {
           string checker = $@"select sifre from Kisiler where kullaniciadi = '{kullanici_adi.Text}'";
           string sifre = Sqlexecuter(checker, 1);
           if (sifre == this.sifre.Text)
           {
                MessageBox.Show("Giriş Başarılı!");
                string personget = string.Format(select_with_where,"*","Kisiler",$"kullaniciadi = '{kullanici_adi.Text}'");
                Dictionary<string, List<string>> persondict = Sqlreaderexecuter(personget);
                Person person = new Person(persondict, kullanici_adi.Text);
                Settings.GeneralSettings = JsonConvert.SerializeObject(person);
               if (persondict["unvan"].Contains("Ogretmen"))
                    ogretmen_loggin_paneli.Show();
               else
                    ogrenci_loggin_paneli.Show();
                Control_hide(new Control[] { giriss_paneli, Giris_Paneli, Kayit_Paneli });
                Ortak_Panel.Show();
           }
           else if (sifre == "null")
                MessageBox.Show("böyle bir kullanici adi yok");
           else
                MessageBox.Show("Şifre Yanlış Lütfen Tekrar Deneyiniz");
        }
        private void Button6_Click(object sender, EventArgs e)
        {
            Hocalar_Menu.SelectedIndex = -1;
            Datagridviewformatter(dersprogrami, null, null, Color.DarkGray,false);
            Player.Play();
            excel_paneli.Visible = true;
            Ders_Olusturma_Paneli.Visible = false;
            string str = $@"select Hocalar.isim [Hocalar] from Hocalar
                            inner join Dersler on Dersler.hoca_id = Hocalar.hoca_id";
            Dictionary<string, List<string>> mydict = Sqlreaderexecuter(str);
            FillComboBoxWithList(Hocalar_Menu, mydict["Hocalar"]);
        }
        private void Button7_Click(object sender, EventArgs e)
        {
            Player.Play();
            listeden_sec.Checked = true;
            yeni_hoca_ekle.Checked = false;
            Ders_Olusturma_Paneli.Show();
            excel_paneli.Hide();
            Control_disable(new Control[] { Saat_Menu });
            Combobox_clear(new ComboBox[] { Saat_Menu }, true, false);
            Datagridviewformatter(dersprogrami, null, null, Color.DarkGray, false);
            Gun_Saat_Duzenleyici(Tarih_Secici, Saat_Menu);
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            Player.Play();
            MessageBox.Show("Lütfen belirtilen alana kayıt olduğunuz mail adresinizi giriniz");
            Control_hide(new Control[] { giriss_paneli, Kayit_Paneli });
            Control_show(new Control[] { email_paneli });
        }
        private void Button9_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format(select_with_where, "sifre", "Kisiler", $"mail = '{textBox7.Text}'");
            string obj = Sqlexecuter(sql1, 1);
            if (obj == "null")
            {
                MessageBox.Show("email adresi yanlış lütfen tekrar deneyiniz");
            }
            else
            {
                if (Email(obj.ToString(), "sifreniz", textBox7.Text))
                    MessageBox.Show("sifreniz email adresinize gönderilmiştir");
                else
                    MessageBox.Show("Email gönderiminde bir sorun oluştu lütfen tekrar deneyiniz");
            }
        }
        private void Button10_Click(object sender, EventArgs e)
        {
            Giris_Paneli.Show();
            Player.Play();
            Settings.GeneralSettings = string.Empty;
            Combobox_clear(new ComboBox[] { Saat_Menu, Brans_Hocalar_Menu, Gunler_Menu,
                hoca_from_list, Hocalar_Menu }, true, true);
            Control_hide(new Control[]{ Ortak_Panel,dersprogrami,ogrenci_loggin_paneli , ogretmen_loggin_paneli ,
                Ders_Olusturma_Paneli,Ders_Secim_Paneli, excel_paneli,giriss_paneli,Kayit_Paneli,email_paneli});
        }
        private void Button15_Click(object sender, EventArgs e)
        {
            if (Settings.GeneralSettings == string.Empty)
                return;
            Player.Play();
            Ders_Secim_Paneli.Hide();
            Datagridviewformatter(dersprogrami, Ders_gunlerii, Ders_saatleri,Color.DarkGray);
            Person person = JsonConvert.DeserializeObject<Person>(Settings.GeneralSettings);
            string dersler = $@"select Dersler.DersGünü,Dersler.DersAdi, Dersler.hoca_id[hocaid], 
                                cast(Dersler.date2 as time(0))[time] from Dersler inner join DersKayit 
                                on DersKayit.ders_id = Dersler.Ders_ID where derskayit.student_id = '{person.Id}' ";
            Dictionary<string, List<string>> mydict = Sqlreaderexecuter(dersler);
            FillDataGridView(dersprogrami, mydict, true);
            MessageBox.Show("Bırakmak İstediğiniz Dersin Üzerine Çift Tıklayınız");
        }
        private void Button16_Click(object sender, EventArgs e)
        {
            Player.Play();
            dersprogrami.Hide();
            Ders_Secim_Paneli.Show();
            Combobox_clear(new ComboBox[] { Brans_Menu, Brans_Hocalar_Menu, Gunler_Menu }, false, true);
            Control_disable(new Control[] { Gunler_Menu, Brans_Hocalar_Menu });
        }
        private void Ders_Ara_Click(object sender, EventArgs e)
        {
            Player.Play();
            if (Brans_Hocalar_Menu.Text != "" && Gunler_Menu.Text != "")
            {
                string[] headerscol = { Gunler_Menu.Text };
                Datagridviewformatter(dersprogrami,Ders_saatleri,headerscol,Color.Red);
                string str1 = $"select cast(date2 as time(0))[date] from Dersler where Quota != Enrolled " +
                        $"and Hoca_id = (select hoca_id from Hocalar where isim = '{Brans_Hocalar_Menu.Text}') " +
                        $"and DersGünü = '{Gunler_Menu.Text}'";
                Dictionary<string,List<string>> mydict = Sqlreaderexecuter(str1);
                foreach(string hour in mydict["date"])
                {
                    DataGridViewCell cell = Datagridcellreturner(dersprogrami, hour, Gunler_Menu.Text);
                    if (cell != null)
                    {
                        cell.Value = "Seç";
                        cell.Style.BackColor = Color.Green;
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen listeden hoca ve günü seçiniz");
            }
        }
        private void Brans_Menu_SelectedIndexChanged(object sender, EventArgs e)
        {
            Combobox_clear(new ComboBox[] {Brans_Hocalar_Menu,Gunler_Menu }, true, false);
            string str = $@"select Hocalar.isim [Hocalar] from Hocalar
                        inner join Dersler on Dersler.hoca_id = Hocalar.hoca_id
                        where Dersler.DersAdi = '{Brans_Menu.Text}' and Dersler.Enrolled != Dersler.Quota";
            Dictionary<string, List<string>> mydict = Sqlreaderexecuter(str);
            FillComboBoxWithList(Brans_Hocalar_Menu,mydict["Hocalar"]);
            if(Brans_Hocalar_Menu.Items.Count == 0)
            {
                Combobox_clear(new ComboBox[] { Brans_Hocalar_Menu, Gunler_Menu }, true, false);
                Control_disable(new Control[] { Brans_Hocalar_Menu, Gunler_Menu });
                MessageBox.Show("Sectiginiz Branşta Açık Ders Bulunmamakta Lütfen Farklı Bir Branş Seçiniz!");
            }
            else
            {
                Control_enable(new Control[] { Brans_Hocalar_Menu });
            }
        }

        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Combobox_clear(new ComboBox[] { Gunler_Menu }, true, true);
            string str = $"Select DersGünü from Dersler where Quota != Enrolled and" +
                $" DersAdi = '{Brans_Menu.Text}' and hoca_id = (select Hoca_id from Hocalar" +
                $" where isim ='{Brans_Hocalar_Menu.Text}')";
            Dictionary<string, List<string>> mydict = Sqlreaderexecuter(str);
            FillComboBoxWithList(Gunler_Menu, mydict["DersGünü"]);
            if (Gunler_Menu.Items.Count == 0)
            {
                Gunler_Menu.Enabled = false;
                Gunler_Menu.ResetText();
            }
            else
            {
                Gunler_Menu.Enabled = true;
            }
        }
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dersprogrami.Enabled = false;
            int rowind = dersprogrami.CurrentCell.RowIndex;
            int colind = dersprogrami.CurrentCell.ColumnIndex;
            if (dersprogrami.CurrentCell.Style.BackColor == Color.Green)
            {
                string dersgünü = dersprogrami.Rows[rowind].HeaderCell.Value.ToString();
                string derssaati = dersprogrami.Columns[colind].HeaderText;
                Person person = JsonConvert.DeserializeObject<Person>(Settings.GeneralSettings);
                string ders_id = $@"select Ders_ID from Dersler where hoca_id = (select Hoca_id from Hocalar where isim = '{Brans_Hocalar_Menu.Text}')
                                    and DersGünü = '{dersgünü}' and DersAdi = '{Brans_Menu.Text}'
                                    and cast(date2 as time(0)) = '{derssaati}'";
                ders_id = Sqlexecuter(ders_id, 1);
                if(ders_id == "null")
                    return;
                string checkcollision = $@"select Dersler.DersGünü, cast(date2 as time(0))[time] from
                                           Dersler inner join Derskayit on Derskayit.ders_id = Dersler.Ders_ID
                                           where derskayit.student_id = '{person.Id}'";
                Dictionary<string, List<string>> mydict = Sqlreaderexecuter(checkcollision);
                int countt = mydict["DersGünü"].Count;
                for (int i=0;i<countt;i++)
                {
                    if(mydict["DersGünü"][i] == dersgünü && mydict["time"][i] == derssaati)
                    {
                        MessageBox.Show("Ders Saatleriniz Çakışıyor");
                        dersprogrami.Enabled = true;
                        return;
                    }
                }
                string insertders = $@"insert into derskayit(student_id,ders_id) values('{person.Id}','{ders_id}')";
                string updateenroll = $@"update Dersler set enrolled = '1' where Ders_ID = '{ders_id}'";
                if (Sqlexecuter(insertders, 0) != "null")
                    Sqlexecuter(updateenroll, 0);
                else
                    MessageBox.Show("Basarisiz!");
            }
            dersprogrami.Enabled = true;
        }


        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dersprogrami.Enabled = false;
            Person person = JsonConvert.DeserializeObject<Person>(Settings.GeneralSettings);
            if (person.Unvan == "Ogrenci")
            { 
                int rowind = dersprogrami.CurrentCell.RowIndex;
                int colind = dersprogrami.CurrentCell.ColumnIndex;
                DialogResult dr = MessageBox.Show("Dersi İptal Et","Ders İptali", MessageBoxButtons.YesNo);
                if(dr == DialogResult.Yes)
                {
                     string cellval = dersprogrami.Rows[rowind].Cells[colind].Value.ToString();
                     string dersadi = cellval.Split('\n')[0];
                     string dershocasi = cellval.Split('\n')[1];
                     string dersgunu = dersprogrami.Columns[colind].HeaderText;
                     string derssaati = dersprogrami.Rows[rowind].HeaderCell.Value.ToString();
                     string unenroll = $@"delete from DersKayit where student_id = {person.Id} and
                                                  ders_id = (select Ders_ID from Dersler where 
                                                  hoca_id = (select Hoca_id from Hocalar where 
                                                  isim = '{dershocasi}') and DersAdi = '{dersadi}'
                                                  and cast(date2 as time(0)) = '{derssaati}'
                                                  and DersGünü = '{dersgunu}')";
                     if (Sqlexecuter(unenroll, 0) == "null")
                     {
                        MessageBox.Show("Basarisiz!");
                        dersprogrami.Enabled = true;
                        return;
                     }
                     dersprogrami.Rows[rowind].Cells[colind].ReadOnly = true;
                     dersprogrami.Rows[rowind].Cells[colind].Value = null;
                     dersprogrami.ClearSelection();
                }
            }
            dersprogrami.Enabled = true;
        }

        private void DataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowc = dersprogrami.Rows.Count;
            int colc = dersprogrami.Columns.Count;
            if (e.RowIndex < rowc && e.ColumnIndex < colc && e.RowIndex >= 0 && e.ColumnIndex >= 0 && dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor != Color.Empty)
            {
                if(e.RowIndex != crow || e.ColumnIndex != ccol)
                {
                    crow = e.RowIndex;
                    ccol = e.ColumnIndex;
                    Renk = dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;
                }
                dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightSkyBlue;
            }
        }

        private void DataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            int rowc = dersprogrami.Rows.Count;
            int colc = dersprogrami.Columns.Count;
            if (e.RowIndex < rowc && e.ColumnIndex < colc && e.RowIndex >= 0 && e.ColumnIndex >= 0 && dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor != Color.Empty)
            {
                if (dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Dolu")
                    dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                else if (dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Seç")
                    dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Green;
                else
                    dersprogrami.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Renk;
            }
        }

        private void Button15_Paint(object sender, PaintEventArgs e)
        {
            if (blnButtonDown == false)
            {
                    ControlPaint.DrawBorder(e.Graphics, (sender as Button).ClientRectangle,
                    SystemColors.ActiveBorder, 1, ButtonBorderStyle.Outset,
                    SystemColors.ActiveBorder, 1, ButtonBorderStyle.Outset,
                    SystemColors.ActiveBorder, 1, ButtonBorderStyle.Outset,
                    SystemColors.ActiveBorder, 1, ButtonBorderStyle.Outset);
            }
            else
            {
                    ControlPaint.DrawBorder(e.Graphics, (sender as Button).ClientRectangle,
                    SystemColors.ActiveBorder, 1, ButtonBorderStyle.Inset,
                    SystemColors.ActiveBorder, 1, ButtonBorderStyle.Inset,
                    SystemColors.ActiveBorder, 1, ButtonBorderStyle.Inset,
                    SystemColors.ActiveBorder, 1, ButtonBorderStyle.Inset);
            }
        }
        private void Button15_MouseDown(object sender, MouseEventArgs e)
        {
            blnButtonDown = true;
            (sender as Button).Invalidate();
        }
        private void Button15_MouseUp(object sender, MouseEventArgs e)
        {
            blnButtonDown = false;
            (sender as Button).Invalidate();
        }
        private void ComboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(Hocalar_Menu.Text !="")
            {
                button3.Enabled = true;
                Datagridviewformatter(dersprogrami, Ders_gunlerii, Ders_saatleri, Color.DarkGray);
                string schedule = $"select cast(date2 as time(0))[time],DersGünü, Enrolled from Dersler where " +
                                  $"Hoca_id = (select hoca_id from Hocalar where isim = '{Hocalar_Menu.Text}')";
                Dictionary<string, List<string>> mydict = Sqlreaderexecuter(schedule);
                FillDataGridView(dersprogrami, mydict, false);
            }
        }

        private void TextBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }
        private void Button3_Click_1(object sender, EventArgs e)
        {
            if (Hocalar_Menu.SelectedIndex == -1) return;
            SaveFileDialog savefile = new SaveFileDialog
            {
                FileName = Hocalar_Menu.Text,
                Filter = "Excel Files(*.xlsx)|"
            };
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                CopyAlltoClipboard(dersprogrami);
                ToExcel(savefile);
            }
        }
        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(listeden_sec.Checked)
            {
                yeni_hoca_ekle.Checked = false;
                yeni_hoca_text.Text = "";
                hoca_from_list.Show();
                hoca_from_list.Items.Clear();
                Dictionary<string, List<string>> mydict = Sqlreaderexecuter($"select isim from Hocalar where Brans = '{Branslar_Menu.Text}'");
                FillComboBoxWithList(hoca_from_list, mydict["isim"]);
                yeni_hoca_text.Hide();
            }
            
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(yeni_hoca_ekle.Checked)
            {
                hoca_from_list.SelectedIndex = -1;
                listeden_sec.Checked = false;
                yeni_hoca_text.Show();
                hoca_from_list.Hide();
            }
            
        }

        private void Kullanici_tipi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(kullanici_tipi.Text == "Ogretmen")
            {
                Control_show(new Control[] { Kod_Text, Kod_Label });
                MessageBox.Show("Öğretmen olarak kayıt olabilmek için lütfen onay kodunu giriniz");
            }
            else
            {
                Control_hide(new Control[] { Kod_Text, Kod_Label });
            }
                
        }

        private void Kayit_tamamla_Click(object sender, EventArgs e)
        {
            if ((kullanici_tipi.Text == "Ogretmen" && Kod_Text.Text == "385") || kullanici_tipi.Text == "Ogrenci")
            {
                string yenikayit = $@"INSERT INTO Kisiler (kullaniciadi,sifre,isim,soyisim,mail,unvan)
                                        values('{kullanici_adi.Text}','{sifre.Text}','{Isim_Text.Text}',
                                        '{textBox5.Text}','{textBox3.Text}','{kullanici_tipi.Text}')";
                string success = Sqlexecuter(yenikayit, 0);
                if (success == "")
                {
                    MessageBox.Show("Kayit Basarili Giris Yapabilirsiniz!");
                    kullanici_adi.Clear();
                    sifre.Clear();
                    Kayit_Paneli.Hide();
                }
                else
                    MessageBox.Show("Kullanici adi alinmis. Lutfen tekrar deneyiniz");
            }
        }

        private void Ogrenci_Liste_Butonu_Click(object sender, EventArgs e)
        {
            string ogrenciler = $@"Select isim,soyisim,Sınıf from Kisiler where unvan = 'Ogrenci'";
            Dictionary<string, List<string>> ogrencidict = Sqlreaderexecuter(ogrenciler);
            int ogrencicount = ogrencidict["isim"].Count;
            string[] rows = new string[ogrencicount];
            for (int i = 0; i < ogrencicount; i++)
                rows[i] = i.ToString();
            string[] columns = {"İsim","Soyisim","Sınıf" };
            Datagridviewformatter(dersprogrami, columns, rows, Color.Empty);
            for (int i=0;i<ogrencicount;i++)
            {
                dersprogrami.Rows[i].Cells[0].Value = ogrencidict["isim"][i];
                dersprogrami.Rows[i].Cells[1].Value = ogrencidict["soyisim"][i];
                dersprogrami.Rows[i].Cells[2].Value = ogrencidict["Sınıf"][i];
            }
        }

        private void Profil_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.ShowDialog();
        }
    }
}
