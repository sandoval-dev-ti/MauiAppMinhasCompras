using System.Globalization;
using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views
{
    public partial class NovoProduto : ContentPage
    {
        readonly SQLiteDatabaseHelper _databaseHelper;

        public NovoProduto(SQLiteDatabaseHelper databaseHelper)
        {
            InitializeComponent();
            _databaseHelper = databaseHelper;
        }

        async void OnSalvarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescricaoEntry.Text))
            {
                await DisplayAlert("Atenção", "Informe a descrição do produto.", "OK");
                return;
            }

            int.TryParse(QuantidadeEntry.Text, out int quantidade);
            double.TryParse(PrecoEntry.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double preco);

            var produto = new Produto
            {
                Descricao = DescricaoEntry.Text,
                Quantidade = quantidade,
                Preco = preco
            };

            await _databaseHelper.Insert(produto);
            await Navigation.PopAsync();
        }
    }
}