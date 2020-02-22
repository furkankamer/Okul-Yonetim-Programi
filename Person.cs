using Newtonsoft.Json;
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
        public string Id { get; set; }
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
            Id = persondict["Personid"][0];
        }

        [JsonConstructor]
        public Person(string username,string name,string sname, string mail, string unvan,string snf, string id)
        {
            User = username;
            Name = name;
            Secondname = sname;
            Email = mail;
            Unvan = unvan;
            Sinif = snf;
            Id = id;
        }
    }
}
