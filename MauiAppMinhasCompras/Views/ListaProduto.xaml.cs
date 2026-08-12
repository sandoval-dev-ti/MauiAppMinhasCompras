using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views
{
    public partial class ListaProduto : ContentPage
    {
        readonly SQLiteDatabaseHelper _databaseHelper;

        public ListaProduto(SQLiteDatabaseHelper databaseHelper)
        {
            InitializeComponent();
            _databaseHelper = databaseHelper;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarProdutos();
        }

        async Task CarregarProdutos()
        {
            var produtos = await _databaseHelper.GetAll();
            ProdutosCollectionView.ItemsSource = produtos;
        }

        async void OnNovoClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NovoProduto(_databaseHelper));
        }

        async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = e.NewTextValue;

            if (string.IsNullOrWhiteSpace(texto))
            {
                await CarregarProdutos();
                return;
            }

            var resultado = await _databaseHelper.Search(texto);
            ProdutosCollectionView.ItemsSource = resultado;
        }

        async void OnProdutoSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not Produto produtoSelecionado)
                return;

            await Navigation.PushAsync(new EditarProduto(_databaseHelper, produtoSelecionado));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}