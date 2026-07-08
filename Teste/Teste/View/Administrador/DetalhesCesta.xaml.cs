using System.Windows;
using System.Linq; 
using Teste.Model;

namespace Teste.View
{
    public partial class DetalhesCestaWindow : Window
    {
        public DetalhesCestaWindow(Cesta cesta)
        {
            InitializeComponent();

            TituloCesta.Text = $"Cesta: {cesta.Nome}";

         
            var itensAgrupados = cesta.Itens
                .GroupBy(i => new { i.Nome, i.Marca, i.Peso, i.Preco })
                .Select(grupo => new
                {
                    Quantidade = grupo.Count(),
                    Nome = grupo.Key.Nome,
                    Marca = grupo.Key.Marca,
                    Peso = grupo.Key.Peso,
                    Preco = grupo.Key.Preco
                })
                .ToList();

            ListaProdutosCesta.ItemsSource = itensAgrupados;
        }

        private void Fechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}