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
    /// <summary>
    /// a class for saving and loading from a database. In future will allow for server access aswell. You will need make a class structure that holds your information. this structure will need to hold
    /// anything you need to be saved such as movie names, user login information, location, etc. Then you will have to make an argument in SaveData so you can save your data and add in logic to interact
    /// with that argument to save it (there is a TestStructure already made to see how to do it). You will then need to add logic into the function called CreateTablesForDatabase so it has a place to save
    /// your data. You can see TestStructure for how to set it up. To load your data you will need to create a new function that interacts with the database using your class. You can see TestStructure for
    /// how to interact with the database
    /// </summary>

    public class DatabaseControl
    {
        // database
        const string fileName = "AccessReelAppDatabase2.db3";
        SQLiteAsyncConnection database;

        List<Type> differentClassNamesForDatabase = new List<Type>
        {
            typeof(TestStructure), 
        };

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
        public void SaveData(params object[] toInsertIntoDatabase)
        {
            foreach (var lists in toInsertIntoDatabase)
            {
                if (toInsertIntoDatabase is IEnumerable<object> objectList)
                {                   
                    foreach (var item in objectList)
                    {   
                        //bool tableExists = database.TableMappings.Any(m => m.TableName == objectList.GetType().Name);
                        //if (!tableExists)
                        //{
                        //    database.CreateTableAsync(lists.GetType()).Wait();
                        //}                     
                        database.InsertAsync(item);
                    }
                }
                else
                {
                    bool tableExists = database.TableMappings.Any(m => m.TableName == lists.GetType().Name);
                    if (!tableExists)
                    {
                        database.CreateTableAsync(lists.GetType()).Wait();
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
            CreateTablesForDatabase();
        }

        /// <summary>
        /// add each structure you will need as a table (will need to alter the SaveData method to include your class as an argument if you haven't already
        /// </summary>
        void CreateTablesForDatabase()
        {           
            foreach (var type in differentClassNamesForDatabase)
            {
                database.CreateTableAsync(type).Wait();
            }
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
