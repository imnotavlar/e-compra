using System;
using Teste.Model;
using Teste.Repository;

namespace Teste.ViewModel
{
    public class CadastroViewModel
    {
        public void CriarConta(string nome, string email, string telefone, string senha, string confirmarSenha)
        {
            if (senha != confirmarSenha)
            {
                throw new Exception("As senhas não coincidem!");
            }

            User usuario = new User()
            {
                Nome = nome,
                Email = email,
                Telefone = telefone,
                Senha = senha,
                DataCriacao = DateTime.Now
            };

            UserRepository repo = new UserRepository();

            if (!repo.Salvar(usuario, out string mensagemErro))
            {
                throw new Exception(mensagemErro);
            }
        }
    }
}