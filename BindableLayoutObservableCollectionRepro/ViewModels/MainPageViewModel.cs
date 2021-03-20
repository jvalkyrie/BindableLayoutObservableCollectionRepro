using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using BindableLayoutObservableCollectionRepro.Services;
using Xamarin.Forms;

namespace BindableLayoutObservableCollectionRepro.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Fields
        private ItemSourceService _bindableLayoutItemSourceService;
        private ItemSourceService _listViewItemSourceService;
        private ItemSourceService _collectionViewItemSourceService;

        private ObservableCollection<string> _bindableLayoutItemSource;
        private ObservableCollection<string> _listViewItemSource;
        private ObservableCollection<string> _collectionViewItemSource;
        #endregion

        #region Properties
        public ObservableCollection<string> BindableLayoutItemSource
        {
            get => _bindableLayoutItemSource;
            set
            {
                _bindableLayoutItemSource = value;
                OnPropertyChanged(nameof(BindableLayoutItemSource));
            }
        }

        public ObservableCollection<string> ListViewItemSource
        {
            get => _listViewItemSource;
            set
            {
                _listViewItemSource = value;
                OnPropertyChanged(nameof(ListViewItemSource));
            }
        }

        public ObservableCollection<string> CollectionViewItemSource
        {
            get => _collectionViewItemSource;
            set
            {
                _collectionViewItemSource = value;
                OnPropertyChanged(nameof(CollectionViewItemSource));
            }
        }
        #endregion

        #region Commands
        public ICommand BindableLayoutAddItemCommand => new Command(async () => await BindableLayoutAddItemAsync());
        public ICommand BindableLayoutRemoveItemCommand => new Command(async () => await BindableLayoutRemoveItemAsync());
        public ICommand BindableLayoutClearItemsCommand => new Command(async () => await BindableLayoutClearItemsAsync());

        public ICommand ListViewAddItemCommand => new Command(async () => await ListViewAddItemAsync());
        public ICommand ListViewRemoveItemCommand => new Command(async () => await ListViewRemoveItemAsync());
        public ICommand ListViewClearItemsCommand => new Command(async () => await ListViewClearItemsAsync());

        public ICommand CollectionViewAddItemCommand => new Command(async () => await CollectionViewAddItemAsync());
        public ICommand CollectionViewRemoveItemCommand => new Command(async () => await CollectionViewRemoveItemAsync());
        public ICommand CollectionViewClearItemsCommand => new Command(async () => await CollectionViewClearItemsAsync());
        #endregion

        #region Methods
        public MainPageViewModel()
        {
            // Initialize an instance of a mock Service for each type of list and get the item source.
            _bindableLayoutItemSourceService = new ItemSourceService();
            _listViewItemSourceService = new ItemSourceService();
            _collectionViewItemSourceService = new ItemSourceService();

            BindableLayoutItemSource = _bindableLayoutItemSourceService.GetItems();
            ListViewItemSource = _listViewItemSourceService.GetItems();
            CollectionViewItemSource = _collectionViewItemSourceService.GetItems();
        }

        private async Task BindableLayoutAddItemAsync()
        {
            await _bindableLayoutItemSourceService.AddItemAsync();
        }

        private async Task BindableLayoutClearItemsAsync()
        {
            await _bindableLayoutItemSourceService.ClearItemsAsync();
        }

        private async Task BindableLayoutRemoveItemAsync()
        {
            await _bindableLayoutItemSourceService.RemoveItemAsync();
        }

        private async Task ListViewAddItemAsync()
        {
            await _listViewItemSourceService.AddItemAsync();
        }

        private async Task ListViewRemoveItemAsync()
        {
            await _listViewItemSourceService.RemoveItemAsync();
        }

        private async Task ListViewClearItemsAsync()
        {
            await _listViewItemSourceService.ClearItemsAsync();
        }

        private async Task CollectionViewAddItemAsync()
        {
            await _collectionViewItemSourceService.AddItemAsync();
        }

        private async Task CollectionViewRemoveItemAsync()
        {
            await _collectionViewItemSourceService.RemoveItemAsync();
        }

        private async Task CollectionViewClearItemsAsync()
        {
            await _collectionViewItemSourceService.ClearItemsAsync();
        }
        #endregion
    }
}
