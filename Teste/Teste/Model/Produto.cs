using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Teste.Model
{
    public class Produto : INotifyPropertyChanged
    {
        public string Nome { get; set; } = "";
        public string Marca { get; set; } = "";
        public string Categoria { get; set; } = "";
        public decimal Preco { get; set; }
        public int QuantidadeFixa { get; set; }

        private int _quantidadeSelecionada = 1;
        public int QuantidadeSelecionada
        {
            get => _quantidadeSelecionada;
            set
            {
                if (_quantidadeSelecionada != value)
                {
                    _quantidadeSelecionada = value;
                    OnPropertyChanged();
                    
                    OnPropertyChanged(nameof(SubtotalItem));
                }
            }
        }

        
        private string _peso = "";
        public string Peso
        {
            get => _peso;
            set
            {
                if (_peso != value)
                {
                    _peso = value;
                    OnPropertyChanged(); 
                }
            }
        }

        public decimal SubtotalItem => Preco * QuantidadeSelecionada;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}