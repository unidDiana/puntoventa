using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Ventas.ViewModel;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
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
    public sealed partial class Ventanueva : Page
    {
        productos curPro = new productos();
        ObservableCollection<detalle> datDet = new ObservableCollection<detalle>();
        string sMsj = "";
        public Ventanueva()
        {
            this.InitializeComponent();
            comboload();
        }

        /// <summary>
        /// Se invoca cuando esta página se va a mostrar en un objeto Frame.
        /// </summary>
        /// <param name="e">Datos de evento que describen cómo se llegó a esta página.
        /// Este parámetro se usa normalmente para configurar la página.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Venta_Click(object sender, RoutedEventArgs e)
        {
            int newFol = 0;
            sMsj = "";
            venta newventa = new venta { fecha = DateTime.Now, subtotal = Convert.ToDouble(subtotal.Text), impuesto = Convert.ToDouble(iva.Text), total = Convert.ToDouble(total.Text), pago = Convert.ToDouble(pagado.Text), cambio = Convert.ToDouble(cambio.Text) };
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "dbarticulos.sqlite"), true);
            conn.InsertAsync(newventa).ContinueWith(t =>
            {
                newFol = newventa.folio;
                sMsj += "Folio: " + newFol +"\n";
                sMsj += "Detalle de venta" + "\n";
                sMsj += "-----------------------------------------" + "\n";
                sMsj += "Descripción | Precio | Cantidad | Importe" + "\n";
                for (int i = 0; i < tbldetalles.Items.Count(); i++)
                {
                    detalle oDet = new detalle();
                    oDet = (detalle)tbldetalles.Items[i];
                    detalle newdet = new detalle { folio_venta = newFol, cantidad = oDet.cantidad, producto = oDet.producto, precio = oDet.precio, importe = oDet.importe };
                    sMsj += newdet.producto + " | " + newdet.precio + " | " + newdet.cantidad + " | " + newdet.importe + "\n";
                    conn.InsertAsync(newdet).ContinueWith(tx =>
                    {
                        //newFol = newdet.id_detalle;
                    });
                }
                sMsj += "-----------------------------------------" + "\n\n";
                sMsj += "Subtotal: " + newventa.subtotal.ToString("0.00") + "\n";
                sMsj += "IVA: " + newventa.impuesto.ToString("0.00") + "\n";
                sMsj += "-----------------------------------------" + "\n";
                sMsj += "Total: " + newventa.total.ToString("0.00") + "\n";
                sMsj += "Pagado: " + newventa.pago.ToString("0.00") + "\n";
                sMsj += "Cambio: " + newventa.cambio.ToString("0.00") + "\n";
                carga();

            });
        }

        public async void carga()
        {
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

            await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Frame.Navigate(typeof(MainPage));
                send();
            });
        }

        public async void send() {
            Windows.ApplicationModel.Email.EmailMessage mail = new Windows.ApplicationModel.Email.EmailMessage();
            mail.Subject = "Nota de venta";
            mail.Body = sMsj;
            mail.To.Add(new Windows.ApplicationModel.Email.EmailRecipient("xapg78@gmail.com", "Xochitl Puerto"));
            await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(mail);
        }

        public async void comboload()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "dbarticulos.sqlite"), true);
            var query = conn.Table<productos>();
            var result = await query.ToListAsync();
            cbxProductos.ItemsSource = result.OrderByDescending(i => i.descripcion).ToList();
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double nCan = Convert.ToDouble(txtCantidad.Text);
                tbldetalles.ItemsSource = datDet;
                detalle oDet = new detalle();
                oDet.cantidad = nCan;
                oDet.producto = curPro.descripcion;
                oDet.precio = curPro.precio;
                oDet.importe = curPro.precio * nCan;
                datDet.Add(oDet);
                calcula();
                txtCantidad.Text = "";
            }
            catch (Exception) { }
        }

        private void cbxProductos_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            curPro = ((productos)cbxProductos.SelectedItem);
        }

        public void calcula() {

            double nSubTol = 0;
            double nIva = 0;
            double nTotal = 0;
            double nPago = 0;
            double nCambio = 0;
            try
            {
                for (int i = 0; i < tbldetalles.Items.Count(); i++)
                {
                    detalle oDet = new detalle();
                    oDet = (detalle)tbldetalles.Items[i];
                    nSubTol += oDet.importe;
                }
                nIva = nSubTol * 0.16;
                nTotal = nSubTol + nIva;
                nPago = Convert.ToDouble(pagado.Text);
                nCambio = nPago - nTotal;
                nCambio = (nCambio > 0) ? nCambio : 0;
                subtotal.Text = nSubTol.ToString("0.00");
                iva.Text = nIva.ToString("0.00");
                total.Text = nTotal.ToString("0.00");
                cambio.Text = nCambio.ToString("0.00");
                NumToLet numToLet = new NumToLet();
                lblLetras.Text = "Son: " + numToLet.enletras(nTotal.ToString());
            }
            catch (Exception) { }
        }

        private void pagado_TextChanged(object sender, TextChangedEventArgs e)
        {
            calcula();
        }
    }
}

