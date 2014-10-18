using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas.ViewModel
{
    class detalle
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int id_detalle { get; set; }
        public int folio_venta { get; set; }
        public double cantidad { get; set; }
        public string producto { get; set; }
        public double precio { get; set; }
        public double importe { get; set; }
    }

}
