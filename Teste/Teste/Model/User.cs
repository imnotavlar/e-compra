using System;

namespace Teste.Model
{
    public class User
    {
        private static int contador = 1;

        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telefone { get; set; } = "";
        public string Senha { get; set; } = "";
        public string FotoPerfil { get; set; } = "";
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public Endereco? Endereco { get; set; }

        public User()
        {
            Id = contador++;
        }

        public User(int id)
        {
            Id = id;

            if (id >= contador)
                contador = id + 1;
        }

        public bool IsAdmin => Id == 1;
    }
}