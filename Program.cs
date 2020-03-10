using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    static class Program
    {
        public static string[] Ders_saatleri = { "10:50:00", "11:10:00", "13:00:00", "13:30:00", "14:00:00", "14:30:00", "17:00:00", "17:30:00" };
        public static string[] Ders_gunlerii = { "Pazartesi", "Sali", "Çarşamba", "Persembe", "Cuma", "Cumartesi" };
        public static string[] Brans_Isimleri = { "Ingilizce", "Biyoloji", "Kimya", "Fizik", "Matematik", "Turkce", "Edebiyat" };
        public static string[] Siniflar = { "12", "11", "10", "9" };
        public static string[] Kullanici_Tipleri = {"Ogrenci", "Ogretmen" };
        public static string select = @"select ";
        public static string select_with_where = @"select {0} from {1} where {2}";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
