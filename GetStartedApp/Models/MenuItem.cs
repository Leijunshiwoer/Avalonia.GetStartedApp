using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GetStartedApp.Models
{
    public class MenuItem
    {
        public string? Header { get; set; }
        public string IconIndex { get; set; }
        public bool IsSeparator { get; set; }
        public string? Navigate { get; set; }
        public ICommand NavigationCommand { get; set; }
        public ObservableCollection<MenuItem> Children { get; set; } = new ObservableCollection<MenuItem>();

        public IEnumerable<MenuItem> GetLeaves()
        {
            if (this.Children.Count == 0)
            {
                yield return this;
                yield break;
            }

            foreach (var child in Children)
            {
                var items = child.GetLeaves();
                foreach (var item in items)
                {
                    yield return item;
                }
            }
        }
    }
}
