using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Ventas.ViewModel;
using Ventas.Views;
using Windows.ApplicationModel;
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

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=391641

namespace Ventas{
    public sealed partial class MainPage : Page
    {
        private string dataRead;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            CopyDatabase();
        }

               
        /// <summary>
        /// Se invoca cuando esta página se va a mostrar en un objeto Frame.
        /// </summary>
        /// <param name="e">Datos de evento que describen cómo se llegó a esta página.
        /// Este parámetro se usa normalmente para configurar la página.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Preparar la página que se va a mostrar aquí.

            // TODO: Si la aplicación contiene varias páginas, asegúrese de
            // controlar el botón para retroceder del hardware registrándose en el
            // evento Windows.Phone.UI.Input.HardwareButtons.BackPressed.
            // Si usa NavigationHelper, que se proporciona en algunas plantillas,
            // el evento se controla automáticamente.
        }

        private async Task CopyDatabase()
        {
            bool isDatabaseExisting = false;

            try
            {
                StorageFile storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync("dbarticulos.sqlite");
                isDatabaseExisting = true;
            }
            catch
            {
                isDatabaseExisting = false;
            }

            if (!isDatabaseExisting)
            {
                StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync(@"database\dbarticulos.sqlite");
                await databaseFile.CopyAsync(ApplicationData.Current.LocalFolder);
            }   
        }

       /* public class productos
        {
            [PrimaryKey]
            public int id_producto { get; set; }

            [MaxLength(256)]
            public string descripcion { get; set; }

            [MaxLength(256)]
            public string clave { get; set; }

            [MaxLength(256)]
            public string precio { get; set; }
        }*/

       
        private void listBoxobj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxobj.SelectedIndex != -1)
            {
               venta listitem = listBoxobj.SelectedItem as venta;//Get slected listbox item contact ID
                Frame.Navigate(typeof(DetalleVenta), listitem);
            }
        }
        private async void tableload()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "dbarticulos.sqlite"), true);
            var query = conn.Table<venta>();
            var result = await query.ToListAsync();
            listBoxobj.ItemsSource = result.OrderByDescending(i => i.folio).ToList();
      
      
        }

        private async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "dbarticulos.sqlite"), true);
            Frame.Navigate(typeof(Ventanueva));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tableload();
        }

    }}
