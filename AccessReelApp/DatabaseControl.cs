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

/*
 * created by George Dinnison
 * date last edited 6/Nov/2023
 * 
 * allowing for saving, updating and deleting from a local sqlite database
*/


namespace AccessReelApp
{
    /// <summary>
    /// use to save and load from a database. use SaveData to save to the database, creates tables on every entry (need to change). to load from database use LoadDataFromDatabaseAsync.
    /// you will most likely need to create a new class to use the database functions since done with the sqlite-net library. the classes you can use can't contain an initiliser and
    /// everything that is registered and saved to the database needs a get method (don't know if needs a set). the pathway the database uses should be printed via debug.writeline().
    /// you will also need a primary key that isn't null. you can autoincrement but will be a problem if you wish to access those items later on through updating (won't matter if not updating
    /// since the primary key will be attached to the class instance when loading)
    /// </summary>
    public class DatabaseControl
    {
        const string fileName = "TestDatabaseAccessReel1.db3";
        SQLiteAsyncConnection database;

        public DatabaseControl()
        {
            OpenDatabase();
        }

        /// <summary>
        /// enter in as many arguments as you want. will save to database assuming it is a valid type. still need to make error checking for if its valid or not. is a task so you can
        /// await completion is needed. is the item wasn't saved it will debug.writeline that item as 'wasn't saved : {item}' 
        /// </summary>
        /// <param name="data">what you want to be saved to the database (can enter entire lists at once aswell - does a double foreach if it can or just adds it if can't)</param>
        /// <returns>Task</returns>
        public async Task SaveData(params object[] data)
        {
            foreach (object arg in data)
            {
                if (arg is IEnumerable<object> argList)
                {
                    foreach (var item in argList)
                    {
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
        /// table and updates that entry. this could be problamatic if you are autoincrementing each primary key. if the item was updated will debug.writeline that item.
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

        /// <summary>
        /// add any item you want to delete from the database and deletes the item with the same primary key. uses the class type as the table name to delete from. will be problimatic if using auto
        /// increment for a primary key. debug.writelines 'wasnt deleted : {item}' if the item wasn't deleted
        /// </summary>
        /// <param name="data">what you wish to be deleted</param>
        /// <returns></returns>
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