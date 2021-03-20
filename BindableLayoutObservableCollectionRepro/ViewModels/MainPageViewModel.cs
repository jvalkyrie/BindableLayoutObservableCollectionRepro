using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BindableLayoutObservableCollectionRepro.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Fields
        public ObservableCollection<string> _bindableLayoutItemSource;
        public ObservableCollection<string> _listViewItemSource;
        public ObservableCollection<string> _collectionViewItemSource;
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
        #endregion

        #region Methods
        public MainPageViewModel()
        {
            BindableLayoutItemSource = new ObservableCollection<string>();
            ListViewItemSource = new ObservableCollection<string>();
            CollectionViewItemSource = new ObservableCollection<string>();
        }

        private async Task BindableLayoutAddItemAsync()
        {
            await Task.Delay(500);
            BindableLayoutItemSource.Add($"Item {BindableLayoutItemSource.Count + 1}");
        }

        private async Task BindableLayoutClearItemsAsync()
        {
            await Task.Delay(500);
            BindableLayoutItemSource.Clear();
        }

        private async Task BindableLayoutRemoveItemAsync()
        {
            await Task.Delay(500);
            if (BindableLayoutItemSource.Count > 0)
            {
                BindableLayoutItemSource.RemoveAt(BindableLayoutItemSource.Count - 1);
            }
        }

        private async Task ListViewAddItemAsync()
        {
            await Task.Delay(500);
            ListViewItemSource.Add($"Item {ListViewItemSource.Count + 1}");
        }

        private async Task ListViewRemoveItemAsync()
        {
            await Task.Delay(500);
            if (ListViewItemSource.Count > 0)
            {
                ListViewItemSource.RemoveAt(ListViewItemSource.Count - 1);
            }
        }

        private async Task ListViewClearItemsAsync()
        {
            await Task.Delay(500);
            BindableLayoutItemSource.Clear();
        }
        #endregion
    }
}
