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

        Dictionary<Type, List<Type>> loadedFromDatabase = new();

        public DatabaseControl()
        {
            OpenDatabase();
        }

        public async Task SaveData(params object[] data)
        {
            Debug.WriteLine("save clicked ------");
            foreach (object arg in data)
            {
                Debug.WriteLine("first foreach ran");
                if (arg is IEnumerable<object> argList)
                {
                    foreach (var item in argList)
                    {
                        Debug.WriteLine(item);
                        await database.CreateTableAsync(item.GetType());
                        await database.InsertAsync(item);                        
                    }
                }
                else
                {
                    Debug.WriteLine(arg);
                    await database.CreateTableAsync(arg.GetType());
                    await database.InsertAsync(arg);                                       
                }
            }
        }

        void OpenDatabase()
        {
            var localDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var fullPath = Path.Combine(localDataPath, fileName);

            Debug.WriteLine(fullPath);

            database = new SQLiteAsyncConnection(fullPath);
        }

        public List<TestStructure> LoadTestStructure()
        {
            return database.Table<TestStructure>().ToListAsync().Result;
        }
    }
}