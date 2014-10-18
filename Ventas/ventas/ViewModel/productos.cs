using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas.ViewModel
{

    class productos
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int id_producto { get; set; }
        public string descripcion { get; set; }
        public string clave { get; set; }
        public double precio { get; set; }
    }
}
