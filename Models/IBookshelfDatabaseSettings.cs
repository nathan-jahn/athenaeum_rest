using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Athenaeum_REST_API.Models
{
    //dataclass which stores values for the mongodb driver
    //this class is implemented as a singleton and its values set in the startup.cs file
    public interface IBookshelfDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
