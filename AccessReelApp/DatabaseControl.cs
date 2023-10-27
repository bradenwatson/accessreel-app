using AccessReelApp.database_structures;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
//using static Android.Content.ClipData;

namespace AccessReelApp
{
    /// <summary>
    /// use to save and load from a database. use SaveData to save to the database, creates tables on every entry (need to change). to load from database use LoadDataFromDatabaseAsync.
    /// you will most likely need to create a new class to use the database functions since done with the sqlite-net library. the classes you can use can't contain an initiliser and
    /// everything that is registered and saved to the database needs a get method (don't know if needs a set). the pathway the database uses should be printed via debug.writeline().
    /// you will also need a primary key that isn't null. you can autoincrement
    /// </summary>
    public class DatabaseControl
    {
        const string fileName = "TestDatabaseAccessReel1.db3";
        SQLiteAsyncConnection database;

        Dictionary<Type, List<Type>> loadedFromDatabase = new();

        public DatabaseControl()
        {
            OpenDatabase();
        }

        /// <summary>
        /// enter in as many arguments as you want. will save to database assuming it is a valid type. still need to make error checking for if its valid or not. is a task so you can
        /// await completion is needed
        /// </summary>
        /// <param name="data">what you want to be saved to the database (can enter entire lists at once aswell - does a double foreach if it can or just adds it if can't)</param>
        /// <returns></returns>
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

                        int numberOfRowsAdded = await database.InsertAsync(item);
                        if (numberOfRowsAdded == 0)
                        {
                            Debug.WriteLine($"wasn't added : {item}");
                        }
                    }
                }
                else
                {
                    Debug.WriteLine(arg);
                    await database.CreateTableAsync(arg.GetType());

                    int numberOfRowsAdded = await database.InsertAsync(arg);
                    if (numberOfRowsAdded == 0)
                    {
                        Debug.WriteLine($"wasn't added : {arg}");
                    }
                }
            }
        }

        /// <summary>
        /// opens a link to the database and prints the pathway via debug.writeline()
        /// </summary>
        void OpenDatabase()
        {
            var localDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var fullPath = Path.Combine(localDataPath, fileName);

            Debug.WriteLine(fullPath);

            database = new SQLiteAsyncConnection(fullPath);
        }

        /// <summary>
        /// goes through the database and returns a list of wanted type. use LoadDataFromDatabaseAsync<wantedClassName>(). can be awaited
        /// </summary>
        /// <typeparam name="T">the class type you are trying to recieve from the database</typeparam>
        /// <returns>a list of every class instance saved to the database</returns>
        public async Task<List<T>> LoadDataFromDatabaseAsync<T>() where T : new()
        {
            return await database.Table<T>().ToListAsync();
        }

        /// <summary>
        /// allows you to enter any item as argument and it will update the item in the database. the way it works is it grabs the primary key associated with the class, finds the
        /// table and updates that entry. this could be problamatic if you are autoincrementing each primary key. up to each user to handle that issue
        /// </summary>
        /// <param name="data">each item you wish to update that exists in the database. wont do anything if doesn't exist. it uses the primary key to find each item</param>
        /// <returns>Task</returns>
        public async Task EditAlreadyCreatedItem(params object[] data)
        {
            foreach (var arg in data)
            {
                if (arg is IEnumerable<object> argList)
                {
                    foreach (var item in argList)
                    {
                        int numberOfRowsUpdated = await database.UpdateAsync(item);
                        if (numberOfRowsUpdated == 0)
                        {
                            Debug.WriteLine($"wasn't updated : {item}");
                        }
                    }
                }
                else
                {
                    int numberOfRowsUpdated = await database.UpdateAsync(arg);
                    if (numberOfRowsUpdated == 0)
                    {
                        Debug.WriteLine($"wasn't updated : {arg}");
                    }
                }
            }            
        }

        public async Task DeleteAlreadyCreatedItem(params object[] data)
        {
            foreach (var arg in data)
            {
                if (arg is IEnumerable<object> argList)
                {
                    foreach (var item in argList)
                    {
                        int numberOfRowsDeleted = await database.DeleteAsync(item);
                        if (numberOfRowsDeleted == 0)
                        {
                            Debug.WriteLine($"wasn't deleted : {item}");
                        }
                    }
                }
                else
                {
                    int numberOfRowsDeleted = await database.DeleteAsync(arg);
                    if (numberOfRowsDeleted == 0)
                    {
                        Debug.WriteLine($"wasn't deleted : {arg}");
                    }
                }
            }
        }
    }
}