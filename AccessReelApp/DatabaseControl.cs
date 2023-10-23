using AccessReelApp.database_structures;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessReelApp
{
    public class DatabaseControl
    {
        // database
        const string fileName = "AccessReelAppDatabase.db3";
        SQLiteAsyncConnection database;
        ServerControl serverControl = new ServerControl();

        public DatabaseControl()
        {
            OpenDatabase();
        }

        /// <summary>
        /// saves your data to a database. You will most likely need to create a seperate class under the database structures folder (just to keep things together) in the solution and make a class. It will
        /// need a unique primary key (there is a testclass there to see - also can't have an initiliser method). Then add an argument to the save data and add a new insert async for your class. Will also
        /// need to find each function call and fix up their errors (most likely only found in DataAccess class). Before saving you will need to add a table for your new class so it has a place
        /// to save
        /// </summary>
        public void SaveData(List<TestStructure> testStructure)
        {
            foreach (var testStructureItem in testStructure)
            {
                database.InsertAsync(testStructureItem).Wait();
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
            CreateTablesForDatabase();
        }

        /// <summary>
        /// add each structure you will need as a table (will need to alter the SaveData method to include your class as an argument if you haven't already
        /// </summary>
        void CreateTablesForDatabase()
        {
            database.CreateTableAsync<TestStructure>();
        }

        /// <summary>
        /// returns the data wanted from the database
        /// </summary>
        public void LoadData() 
        { 
            List<TestStructure> retrievedTestStructure = database.Table<TestStructure>().ToListAsync().Result;

            foreach (var testStructure in retrievedTestStructure)
            {
                Debug.WriteLine(testStructure.name);
            }
        }
    }
}
