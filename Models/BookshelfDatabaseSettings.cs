using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Athenaeum_REST_API.Models
{
    //dataclass which stores values for the mongodb driver
    //this class is implemented as a singleton and its values set in the startup.cs file
    class BookshelfDatabaseSettings : IBookshelfDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
