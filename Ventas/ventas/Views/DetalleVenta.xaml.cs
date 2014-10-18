using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Ventas.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkID=390556

namespace Ventas.Views
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class DetalleVenta : Page
    {
        public DetalleVenta()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Se invoca cuando esta página se va a mostrar en un objeto Frame.
        /// </summary>
        /// <param name="e">Datos de evento que describen cómo se llegó a esta página.
        /// Este parámetro se usa normalmente para configurar la página.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            venta curVenta = new venta();
            curVenta = e.Parameter as venta;
            txtFolio.Text = curVenta.folio.ToString();
            lblFecha.Text = "Fecha y Hora de Venta: " + curVenta.fecha.ToString();
            subtotal.Text = curVenta.subtotal.ToString("0.00");
            iva.Text = curVenta.impuesto.ToString("0.00");
            total.Text = curVenta.total.ToString("0.00");
            pagado.Text = curVenta.pago.ToString("0.00");
            cambio.Text = curVenta.cambio.ToString("0.00");
            NumToLet numToLet = new NumToLet();
            lblLetras.Text = "Son: " + numToLet.enletras(curVenta.total.ToString());
            int nFol = curVenta.folio;
            loadetalles(nFol);
        }

        private void txtBack_Click(object sender, RoutedEventArgs e)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "dbarticulos.sqlite"), true);
            Frame.Navigate(typeof(MainPage));
        }

        public async void loadetalles(int nFol)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "dbarticulos.sqlite"), true);
            var query = conn.Table<detalle>().Where(d=>d.folio_venta.Equals(nFol)); ;
            var result = await query.ToListAsync();
            tbldetalles.ItemsSource = result.OrderByDescending(i => i.producto).ToList();
        }
    }
}
