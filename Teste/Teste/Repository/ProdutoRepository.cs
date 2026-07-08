using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Teste.Model;

namespace Teste.Repository
{
    public class ProdutoRepository
    {
        private string ObterCaminho()
        {
            string pastaProjeto = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\")
            );

            return Path.Combine(pastaProjeto, "cadastroProdutos", "produtos.txt");
        }

        public void AtualizarArquivoTxt()
        {
            try
            {
                
                string caminho = ObterCaminho();
                Directory.CreateDirectory(Path.GetDirectoryName(caminho));

                List<string> linhasParaSalvar = new List<string>();

                foreach (var produto in MemoriaProdutos.Lista)
                {
                
                    string linha = $"Nome:{produto.Nome} | Marca:{produto.Marca} | Categoria:{produto.Categoria} | Preco:{produto.Preco} | Peso:{produto.Peso}";
                    linhasParaSalvar.Add(linha);
                }

                File.WriteAllLines(caminho, linhasParaSalvar);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao atualizar arquivo TXT: " + ex.Message);
            }
        }

        public void CarregarDoArquivo()
        {
            string caminho = ObterCaminho();

            string pasta = Path.GetDirectoryName(caminho)!;
            Directory.CreateDirectory(pasta);

            if (!File.Exists(caminho))
                return;

            MemoriaProdutos.Lista.Clear();

            var linhas = File.ReadAllLines(caminho);

            foreach (var linha in linhas)
            {
                var partes = linha.Split('|');

                if (partes.Length < 5)
                    continue;

                decimal preco = decimal.TryParse(
                    partes[3].Replace("Preco:", "").Trim(),
                    out var p) ? p : 0m;

                string peso = partes[4].Replace("Peso:", "").Trim();

                MemoriaProdutos.Lista.Add(new Produto
                {
                    Nome = partes[0].Replace("Nome:", "").Trim(),
                    Marca = partes[1].Replace("Marca:", "").Trim(),
                    Categoria = partes[2].Replace("Categoria:", "").Trim(),
                    Preco = preco,
                    Peso = peso
                });
            }
        }

        public bool Salvar(Produto produto, out string erro)
        {
            erro = "";

            if (string.IsNullOrWhiteSpace(produto.Nome) ||
                string.IsNullOrWhiteSpace(produto.Marca) ||
                string.IsNullOrWhiteSpace(produto.Categoria))
            {
                erro = "Preencha todos os campos obrigatórios.";
                return false;
            }

            if (MemoriaProdutos.Lista.Any(p =>
                p.Nome.Trim().ToLower() == produto.Nome.Trim().ToLower()))
            {
                erro = "Produto já cadastrado.";
                return false;
            }

            MemoriaProdutos.Lista.Add(produto);

            try
            {
                string caminho = ObterCaminho();
                Directory.CreateDirectory(Path.GetDirectoryName(caminho));
                string linha = $"Nome:{produto.Nome} | Marca:{produto.Marca} | Categoria:{produto.Categoria} | Preco:{produto.Preco} | Peso:{produto.Peso}\n";
                File.AppendAllText(caminho, linha);
            }
            catch { }

            return true;
        }

        public void SalvarTudo()
        {
            AtualizarArquivoTxt();
        }
    }
}