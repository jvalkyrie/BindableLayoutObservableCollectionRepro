using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BindableLayoutObservableCollectionRepro.Services
{
    /// <summary>
    /// A mock service class that manages a list of items outside of the UI thread.
    /// </summary>
    public class ItemSourceService
    {
        private ObservableCollection<string> itemSource;

        public ItemSourceService()
        {
            itemSource = new ObservableCollection<string>();
        }

        public ObservableCollection<string> GetItems()
        {
            return itemSource;
        }

        public async Task AddItemAsync()
        {
            await Task.Delay(100).ConfigureAwait(false);
            itemSource.Add($"Item {itemSource.Count + 1}");
            
        }

        public async Task RemoveItemAsync()
        {
            await Task.Delay(100).ConfigureAwait(false);
            if(itemSource.Count > 0)
            {
                itemSource.RemoveAt(itemSource.Count - 1);
            }
        }

        public async Task ClearItemsAsync()
        {
            await Task.Delay(100).ConfigureAwait(false);
            itemSource.Clear();
        }
    }
}
