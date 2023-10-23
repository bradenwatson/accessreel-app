using AccessReelApp.database_structures;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

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
        const string fileName = "AccessReelAppDatabase3.db3";
        SQLiteAsyncConnection database;

        public DatabaseControl()
        {
            OpenDatabase();
        }

        /// <summary>
        /// saves your data to a database. Enter is any many arguments as you want. Works for list of class instances and single items. Will be a silent error if the table doesnt exist (need to alter my logic in the CreateTablesForDatabase method)
        /// </summary>
        public void SaveData(params object[] toInsertIntoDatabase)
        {
            foreach (var lists in toInsertIntoDatabase)
            {
                if (toInsertIntoDatabase is IEnumerable<object> objectList)
                {                   
                    foreach (var item in objectList)
                    {  
                        bool tableExists = database.TableMappings.Any(m => m.TableName == item.GetType().Name);
                        if (!tableExists)
                        {
                            CreateTablesForDatabase();
                        }  
                        
                        database.InsertAsync(item);
                    }
                }
                else
                {
                    bool tableExists = database.TableMappings.Any(m => m.TableName == lists.GetType().Name);
                    if (!tableExists)
                    {
                        CreateTablesForDatabase();
                    }

                    database.InsertAsync(lists).Wait();
                }
            }            
        }

        /// <summary>
        /// creates a connection to the database
        /// </summary>
        void OpenDatabase()
        {
            var localDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var fullPath = Path.Combine(localDataPath, fileName);

            Debug.WriteLine(fullPath);

            database = new SQLiteAsyncConnection(fullPath);
        }

        /// <summary>
        /// add each structure you will need as a table (will need to alter the SaveData method to include your class as an argument if you haven't already
        /// </summary>
        void CreateTablesForDatabase()
        {
            database.CreateTableAsync<TestStructure>().Wait();
        }

        /// <summary>
        /// A test for seeing if can retrieve data from database. Will need to implement own logic for needed class
        /// </summary>
        public void LoadTestStructure() 
        { 
            List<TestStructure> retrievedTestStructure = database.Table<TestStructure>().ToListAsync().Result;

            foreach (var testStructure in retrievedTestStructure)
            {
                Debug.WriteLine(testStructure.name);
            }
        }
    }
}
