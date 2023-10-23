using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp.database_structures
{
    public class TestStructure
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }  
    }
}
