using System.Collections.ObjectModel;
using TelaClientes;
using Teste.Model;

namespace TelaClientes.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<User> Clientes { get; set; }

        public MainViewModel()
        {
         
        }
    }
}