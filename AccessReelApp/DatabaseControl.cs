using AccessReelApp.database_structures;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp
{
    /// <summary>
    /// a class for saving and loading from a database. In future will allow for server access aswell. You will need make a class structure that holds your information. this structure will need to hold
    /// anything you need to be saved such as movie names, user login information, location, etc. You will then need to create a table in the CreateTablesForDatabases method following the same logic as the TestStructure (trying to find a way to 
    /// make it more dynamic). Then to save all you have to do is call the SaveData function. this function is meant to take in a list of class instances and iterate over each item and insert it but works for single items to.
    /// doesnt work for list of lists or anything like that. still working on a way to retrieve data dynamically
    /// </summary>

    public class DatabaseControl
    {
        // database
        const string fileName = "TestDatabaseAccessReel1.db3";
        SQLiteAsyncConnection database;

        public DatabaseControl()
        {
            OpenDatabase();
        }

        public void SaveData(params object[] data)
        {
            Debug.WriteLine("save clicked ------");
            // goes through each item entered as an argument
            foreach (object arg in data)
            {
                Debug.WriteLine("first foreach ran");
                if (arg is IEnumerable<object> argList)
                {
                    foreach (var item in argList)
                    {
                        Debug.WriteLine(item);
                        database.InsertAsync(item);
                    }
                }
                else
                {
                    Debug.WriteLine(arg);
                }
            }
        }

        void OpenDatabase()
        {
            var localDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var fullPath = Path.Combine(localDataPath, fileName);

            Debug.WriteLine(fullPath);

            database = new SQLiteAsyncConnection(fullPath);
            CreateTablesForDatabase();
        }

        void CreateTablesForDatabase()
        {
            database.CreateTableAsync<TestStructure>().Wait();
        }

        public void LoadTestStructure() 
        {
            List<TestStructure> contactList = database.Table<TestStructure>().ToListAsync().Result;

            foreach (var contact in contactList)
            {
                Debug.WriteLine(contact.name);
            }
        }
    }
}