using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas.ViewModel
{
    class venta
    {
        //The Id property is marked as the Primary Key
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int folio { get; set; }
        public DateTime fecha { get; set; }
        public double subtotal { get; set; }
        public double impuesto { get; set; }
        public double total { get; set; }
        public double pago { get; set; }
        public double cambio { get; set; }

        public venta()
        {
            //empty constructor
        }
        /*public venta(int Folio, double subtotal, double iva, double total, double pagado, double cambio)
        {
            folio = Folio;
            FechaAlta = DateTime.Now.ToString();
            Subtotal = subtotal;
            Iva = iva;
            Total = total;
            Pagado = pagado;
            Cambio = cambio;
        }*/
    }
}
