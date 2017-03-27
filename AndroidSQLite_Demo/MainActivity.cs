using Android.App;
using Android.Widget;
using Android.OS;
using SQLite;
using System.IO;

namespace AndroidSQLite_Demo
{
    [Activity(Label = "AndroidSQLite_Demo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button createDB;
        Button GetDBInfo;
        Button InsertDBInfo;
        TextView DBInfo;
        string path;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            var sqliteFilename = "ToDoItemDB.db3";
            string libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            path = Path.Combine(libraryPath, sqliteFilename);
            createDB = FindViewById<Button>(Resource.Id.button1);
            GetDBInfo = FindViewById<Button>(Resource.Id.button2);
            InsertDBInfo = FindViewById<Button>(Resource.Id.button3);
            DBInfo = FindViewById<TextView>(Resource.Id.textView1);
            createDB.Click += CreateDB_Click;
            InsertDBInfo.Click += InsertDBInfo_Click;
            GetDBInfo.Click += GetDBInfo_Click;
        }

        private void GetDBInfo_Click(object sender, System.EventArgs e)
        {
            DBInfo.Text =""+ findNumberRecords(path);
        }

        private void InsertDBInfo_Click(object sender, System.EventArgs e)
        {
            Person p1 = new Person();
            p1.FirstName = "Mike";
            p1.LastName = "Ma";
            insertUpdateData(p1, path);
        }

        private void CreateDB_Click(object sender, System.EventArgs e)
        {
           
            createDatabase(path);
        }

        private string createDatabase(string path)
        {
            try
            {
                var connection1 = new SQLiteConnection(path);
                connection1.CreateTable<Person>();
                return "Database created";
                //var connection = new SQLiteAsyncConnection(path);
                //{
                //    connection.CreateTableAsync<Person>();

                //    return "Database created";
                //}
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
        private string insertUpdateData(Person data, string path)
        {
            try
            {
                var db = new SQLiteAsyncConnection(path);

                
                if (db.InsertAsync(data).Result != 0)
                    db.UpdateAsync(data);
                return "Single data file inserted or updated";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        private int findNumberRecords(string path)
        {
            try
            {
                var db = new SQLiteAsyncConnection(path);
                // this counts all records in the database, it can be slow depending on the size of the database
                var count = db.ExecuteScalarAsync<int>("SELECT Count(*) FROM Person").Result;

                // for a non-parameterless query
                // var count = db.ExecuteScalar<int>("SELECT Count(*) FROM Person WHERE FirstName="Amy");

                return count;
            }
            catch (SQLiteException ex)
            {
                return -1;
            }
        }
    }

    


}

