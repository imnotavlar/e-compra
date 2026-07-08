using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Teste.Model;
using Teste.Repository;

namespace Teste.View
{
    public partial class CadastroProduto : UserControl
    {
        // Variável para guardar o produto que está sendo editado no momento
        private Produto _produtoEmEdicao = null;

        public CadastroProduto()
        {
            InitializeComponent();
            AtualizarLista();
        }

        private void SalvarProduto_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validações básicas
            if (string.IsNullOrWhiteSpace(NomeProdutoBox.Text) ||
                string.IsNullOrWhiteSpace(MarcaBox.Text) ||
                CategoriaBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(PrecoBox.Text))
            {
                MessageBox.Show("Preencha todos os campos obrigatórios!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ProdutoRepository repo = new ProdutoRepository();

            // 2. Verifica se estamos EDITANDO ou CRIANDO novo
            if (_produtoEmEdicao != null)
            {
                // MODO EDIÇÃO: Atualiza o objeto que já está na memória
                _produtoEmEdicao.Nome = NomeProdutoBox.Text;
                _produtoEmEdicao.Marca = MarcaBox.Text;
                _produtoEmEdicao.Categoria = (CategoriaBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                _produtoEmEdicao.Preco = decimal.Parse(PrecoBox.Text);
                _produtoEmEdicao.Peso = PesoBox.Text;

                // Salva a alteração no arquivo TXT
                repo.AtualizarArquivoTxt();

                MessageBox.Show("Produto atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                _produtoEmEdicao = null; // Sai do modo de edição
            }
            else
            {
                // MODO NOVO CADASTRO
                Produto novoProduto = new Produto
                {
                    Nome = NomeProdutoBox.Text,
                    Marca = MarcaBox.Text,
                    Categoria = (CategoriaBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    Preco = decimal.Parse(PrecoBox.Text),
                    Peso = PesoBox.Text
                };

                if (!repo.Salvar(novoProduto, out string erro))
                {
                    MessageBox.Show(erro, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("Produto cadastrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            LimparCampos();
            AtualizarLista();
        }

        private void EditarProduto_Click(object sender, RoutedEventArgs e)
        {
            // O botão Editar apenas CARREGA os dados nos campos de texto
            Button btn = sender as Button;
            Produto produtoClicado = btn.DataContext as Produto;

            if (produtoClicado != null)
            {
                _produtoEmEdicao = produtoClicado; // Define quem vamos editar

                NomeProdutoBox.Text = produtoClicado.Nome;
                MarcaBox.Text = produtoClicado.Marca;
                PrecoBox.Text = produtoClicado.Preco.ToString();
                PesoBox.Text = produtoClicado.Peso;

                // Seleciona a categoria correta no ComboBox
                foreach (ComboBoxItem item in CategoriaBox.Items)
                {
                    if (item.Content.ToString() == produtoClicado.Categoria)
                    {
                        CategoriaBox.SelectedItem = item;
                        break;
                    }
                }
         
            }
        }

        private void ExcluirProduto_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Produto produtoClicado = btn.DataContext as Produto;

            if (produtoClicado != null)
            {
                MessageBoxResult resposta = MessageBox.Show($"Deseja excluir '{produtoClicado.Nome}'?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resposta == MessageBoxResult.Yes)
                {
                    MemoriaProdutos.Lista.Remove(produtoClicado);
                    ProdutoRepository repo = new ProdutoRepository();
                    repo.AtualizarArquivoTxt();
                   AtualizarLista();


                    if (_produtoEmEdicao == produtoClicado)
                    {
                        _produtoEmEdicao = null;
                        LimparCampos();
                    }
                }
            }
        }

        private void AtualizarLista()
        {
            ListaProdutos.ItemsSource = null;
            ListaProdutos.ItemsSource = MemoriaProdutos.Lista;
        }

        private void LimparCampos()
        {
            NomeProdutoBox.Clear();
            MarcaBox.Clear();
            PrecoBox.Clear();
            PesoBox.Clear();
            CategoriaBox.SelectedIndex = -1;
        }
    }
}