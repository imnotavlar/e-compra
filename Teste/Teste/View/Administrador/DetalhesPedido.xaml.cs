using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Teste.Model;
using Teste.Repository;

namespace Teste.View
{
    public partial class DetalhesPedidoWindow : Window
    {
        private Pedido _pedidoAtual;

        public DetalhesPedidoWindow(Pedido pedido)
        {
            InitializeComponent();

            _pedidoAtual = pedido;

            this.DataContext = pedido;

            var listaItensFinais = new List<object>();

            foreach (var itemPedido in pedido.Itens)
            {
                var cestaOriginal = MemoriaCestas.Lista.FirstOrDefault(c => c.Nome == itemPedido.Nome);

                string nomeProduto = cestaOriginal != null ? cestaOriginal.ResumoItens : "Produtos indisponíveis (Cesta excluída/alterada).";

                listaItensFinais.Add(new
                {
                    Quantidade = itemPedido.Quantidade,
                    Nome = itemPedido.Nome
                });
            }

            GridItensFinais.ItemsSource = listaItensFinais;
            ConfigurarBadges(pedido);
         
            if ((pedido.Status != null && pedido.Status.Equals("Modificado", StringComparison.OrdinalIgnoreCase)) ||
                (pedido.TipoComposicao != null && pedido.TipoComposicao.ToLower().Contains("modificad")))
            {
                BtnPreparada.Visibility = Visibility.Visible;
            }
        }

        private void ConfigurarBadges(Pedido pedido)
        {
            if (string.IsNullOrEmpty(pedido.TipoComposicao))
            {
                BadgeComposicao.Visibility = Visibility.Collapsed;
            }
            else
            {
                BadgeComposicao.Visibility = Visibility.Visible;
                TxtBadge.Text = pedido.TipoComposicao;

                string composicaoLower = pedido.TipoComposicao.ToLower();

                // Se virar "Preparada", aplica o azul de sucesso/completa
                if (composicaoLower.Contains("preparada"))
                {
                    BadgeComposicao.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#E0F2FE")); // Azul suave (Sky-100)
                    TxtBadge.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#0369A1")); // Azul escuro executivo (Sky-700)
                }
                else if (composicaoLower.Contains("personalizad") || composicaoLower.Contains("completa"))
                {
                    // Mantém ou adapta o tom azul clássico do sistema
                    BadgeComposicao.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#DBEAFE")); // Azul claro
                    TxtBadge.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#1E40AF")); // Azul escuro
                }
                else
                {
                    // Amarelo para quando ainda estiver como "Modificada"
                    BadgeComposicao.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FEF3C7")); // Amarelo claro
                    TxtBadge.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#92400E")); // Amarelo escuro
                }
            }
        }

        private void MarcarPreparada_Click(object sender, RoutedEventArgs e)
        {
            if (_pedidoAtual != null)
            {
                var resultado = MessageBox.Show("Deseja confirmar que esta cesta modificada já foi montada e está PRONTAMENTE PREPARADA?",
                                                "Confirmar Preparo",
                                                MessageBoxButton.YesNo,
                                                MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    //  O Status continua "Pendente", mas registramos que a montagem foi feita!
                    _pedidoAtual.StatusMontagem = "Pronta";

                    MessageBox.Show("Cesta marcada como Montada/Pronta com sucesso!",
                                    "Sucesso",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);

                    BtnPreparada.Visibility = Visibility.Collapsed;
                    this.Close();
                }
            }
        }

        private void GerarPdf_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Gerando relatório PDF do resumo logístico...", "PDF", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Fechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}