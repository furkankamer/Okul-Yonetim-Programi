using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Person
    {
        public string User { get; set; }
        public string Name { get; set; }
        public string Secondname { get; set; }
        public string Sinif { get; set; }
        public string İd { get; set; }
        public string Email { get; set; }
        public string Unvan { get; set; }
        
        public Person(Dictionary<string,List<string>> persondict,string username)
        {
            User = username;
            Name = persondict["isim"][0];
            Secondname = persondict["soyisim"][0];
            Email = persondict["mail"][0];
            Unvan = persondict["unvan"][0];
            Sinif = persondict["Sınıf"][0];
            İd = persondict["Personid"][0];
        }
    }
}
