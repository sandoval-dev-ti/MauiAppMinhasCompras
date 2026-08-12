using System.Globalization;
using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views
{
    public partial class EditarProduto : ContentPage
    {
        readonly SQLiteDatabaseHelper _databaseHelper;
        readonly Produto _produto;

        public EditarProduto(SQLiteDatabaseHelper databaseHelper, Produto produto)
        {
            InitializeComponent();
            _databaseHelper = databaseHelper;
            _produto = produto;

            DescricaoEntry.Text = produto.Descricao;
            QuantidadeEntry.Text = produto.Quantidade.ToString();
            PrecoEntry.Text = produto.Preco.ToString(CultureInfo.InvariantCulture);
        }

        async void OnSalvarClicked(object sender, EventArgs e)
        {
            int.TryParse(QuantidadeEntry.Text, out int quantidade);
            double.TryParse(PrecoEntry.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double preco);

            _produto.Descricao = DescricaoEntry.Text;
            _produto.Quantidade = quantidade;
            _produto.Preco = preco;

            await _databaseHelper.Update(_produto);
            await Navigation.PopAsync();
        }

        async void OnExcluirClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert("Confirmar", "Deseja excluir este produto?", "Sim", "Não");
            if (!confirmar) return;

            await _databaseHelper.Delete(_produto.Id);
            await Navigation.PopAsync();
        }
    }
}