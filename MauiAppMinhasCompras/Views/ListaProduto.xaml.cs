using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views
{
    public partial class ListaProduto : ContentPage
    {
        readonly SQLiteDatabaseHelper _databaseHelper;
        public ObservableCollection<Produto> Produtos { get; } = new ObservableCollection<Produto>();

        public ListaProduto(SQLiteDatabaseHelper databaseHelper)
        {
            InitializeComponent();
            _databaseHelper = databaseHelper;
            ProdutosCollectionView.ItemsSource = Produtos;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarProdutos();
        }

        async Task CarregarProdutos()
        {
            var produtos = await _databaseHelper.GetAll();

            Produtos.Clear();
            foreach (var p in produtos)
                Produtos.Add(p);
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

            Produtos.Clear();
            foreach (var p in resultado)
                Produtos.Add(p);
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