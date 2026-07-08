using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using Teste.Model;
using Teste.Repository;

namespace Teste
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            UserRepository repoUsers = new UserRepository();
            repoUsers.CarregarDoArquivo();

            ProdutoRepository repoProdutos = new ProdutoRepository();
            repoProdutos.CarregarDoArquivo();

            CestaRepository repoCestas = new CestaRepository();
            repoCestas.CarregarDoArquivo();

            CarrinhoRepository repoCarrinho = new CarrinhoRepository();
            repoCarrinho.CarregarDoArquivo();

            PedidoRepository repoPedidos = new PedidoRepository();
            repoPedidos.CarregarDoArquivo();

        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {

                if (Sessao.UsuarioLogado != null)
                {
                    UserRepository repoUsers = new UserRepository();
                    repoUsers.Atualizar(Sessao.UsuarioLogado);
                }

                UserRepository repoFinal = new UserRepository();
                repoFinal.SalvarArquivo();

                PedidoRepository repoPedido = new PedidoRepository();
                repoPedido.AtualizarArquivoTxt();

                ProdutoRepository repoProdutos = new ProdutoRepository();
                repoProdutos.AtualizarArquivoTxt();

                CestaRepository repoCestas = new CestaRepository();
                repoCestas.AtualizarArquivoTxt();

                PedidoRepository repoPedidos = new PedidoRepository();
                repoPedidos.AtualizarArquivoTxt();
                UserRepository repo = new UserRepository();
                repo.Atualizar(Sessao.UsuarioLogado);
                CarrinhoRepository repoCarrinho = new CarrinhoRepository();
                repoCarrinho.AtualizarArquivoTxt();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar dados ao fechar o aplicativo: " + ex.Message, "Erro no Fechamento", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            base.OnExit(e);
        }
    }
}