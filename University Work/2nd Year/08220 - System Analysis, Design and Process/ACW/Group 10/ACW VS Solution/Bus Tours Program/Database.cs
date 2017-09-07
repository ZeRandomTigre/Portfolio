using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLibrary;


namespace Bus_Tours_Program
{
    /// <summary>
    /// Shim for quickly reaccessing the database object without the massive startup delay assoiciated with new WynneDatabase();
    /// </summary>
    public class Database
    {
        private static WynneDatabase instance = null;


        public static WynneDatabase Instance
        {
            get { return instance ?? (instance = new WynneDatabase()); }
        }
    }
}
