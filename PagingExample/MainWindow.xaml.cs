using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PagingExample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private DataGridColumn currentSortColumn;

        private ListSortDirection currentSortDirection;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            RefreshProducts();
        }

        private void ProductsDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;

            // The current sorted column must be specified in XAML.
            currentSortColumn = dataGrid.Columns.Where(c => c.SortDirection.HasValue).Single();
            currentSortDirection = currentSortColumn.SortDirection.Value;
        }

        /// <summary>
        /// Sets the sort direction for the current sorted column since the sort direction
        /// is lost when the DataGrid's ItemsSource property is updated.
        /// </summary>
        /// <param name="sender">The parts data grid.</param>
        /// <param name="e">Ignored.</param>
        private void ProductsDataGrid_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (currentSortColumn != null)
            {
                currentSortColumn.SortDirection = currentSortDirection;
            }
        }

        /// <summary>
        /// Custom sort the datagrid since the actual records are stored in the
        /// server, not in the items collection of the datagrid.
        /// </summary>
        /// <param name="sender">The parts data grid.</param>
        /// <param name="e">Contains the column to be sorted.</param>
        private void ProductsDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;
            string sortField = String.Empty;

            // Use a switch statement to check the SortMemberPath
            // and set the sort column to the actual column name. In this case,
            // the SortMemberPath and column names match.
            switch (e.Column.SortMemberPath)
            {
                case ("Id"):
                    sortField = "Id";
                    break;
                case ("Name"):
                    sortField = "Name";
                    break;
            }

            ListSortDirection direction = (e.Column.SortDirection != ListSortDirection.Ascending) ?
              ListSortDirection.Ascending : ListSortDirection.Descending;
            bool sortAscending = direction == ListSortDirection.Ascending;
            Sort(sortField, sortAscending);
            currentSortColumn.SortDirection = null;
            e.Column.SortDirection = direction;
            currentSortColumn = e.Column;
            currentSortDirection = direction;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Product> products;
        private int start = 0;
        private int itemCount = 5;
        private string sortColumn = "Id";
        private bool ascending = true;
        private int totalItems = 0;
        private ICommand firstCommand;
        private ICommand previousCommand;
        private ICommand nextCommand;
        private ICommand lastCommand;

        public ObservableCollection<Product> Products
        {
            get
            {
                return products;
            }
            private set
            {
                if (object.ReferenceEquals(products, value) != true)
                {
                    products = value;
                    NotifyPropertyChanged("Products");
                }
            }
        }

        /// <summary>
        /// Gets the index of the first item in the products list.
        /// </summary>
        public int Start { get { return start + 1; } }

        /// <summary>
        /// Gets the index of the last item in the products list.
        /// </summary>
        public int End { get { return start + itemCount < totalItems ? start + itemCount : totalItems; } }

        /// <summary>
        /// The number of total items in the data store.
        /// </summary>
        public int TotalItems { get { return totalItems; } }

        /// <summary>
        /// Gets the command for moving to the first page of products.
        /// </summary>
        public ICommand FirstCommand
        {
            get
            {
                if (firstCommand == null)
                {
                    firstCommand = new RelayCommand
                    (
                      param =>
                      {
                          start = 0;
                          RefreshProducts();
                      },
                      param =>
                      {
                          return start - itemCount >= 0 ? true : false;
                      }
                    );
                }

                return firstCommand;
            }
        }

        /// <summary>
        /// Gets the command for moving to the previous page of products.
        /// </summary>
        public ICommand PreviousCommand
        {
            get
            {
                if (previousCommand == null)
                {
                    previousCommand = new RelayCommand
                    (
                      param =>
                      {
                          start -= itemCount;
                          RefreshProducts();
                      },
                      param =>
                      {
                          return start - itemCount >= 0 ? true : false;
                      }
                    );
                }

                return previousCommand;
            }
        }

        /// <summary>
        /// Gets the command for moving to the next page of products.
        /// </summary>
        public ICommand NextCommand
        {
            get
            {
                if (nextCommand == null)
                {
                    nextCommand = new RelayCommand
                    (
                      param =>
                      {
                          start += itemCount;
                          RefreshProducts();
                      },
                      param =>
                      {
                          return start + itemCount < totalItems ? true : false;
                      }
                    );
                }

                return nextCommand;
            }
        }

        /// <summary>
        /// Gets the command for moving to the last page of products.
        /// </summary>
        public ICommand LastCommand
        {
            get
            {
                if (lastCommand == null)
                {
                    lastCommand = new RelayCommand
                    (
                      param =>
                      {
                          start = (totalItems / itemCount - 1) * itemCount;
                          start += totalItems % itemCount == 0 ? 0 : itemCount;
                          RefreshProducts();
                      },
                      param =>
                      {
                          return start + itemCount < totalItems ? true : false;
                      }
                    );
                }

                return lastCommand;
            }
        }

        /// <summary>
        /// Sorts the list of products.
        /// </summary>
        /// <param name="sortColumn">The column or member that is the basis for sorting.</param>
        /// <param name="ascending">Set to true if the sort</param>
        public void Sort(string sortColumn, bool ascending)
        {
            this.sortColumn = sortColumn;
            this.ascending = ascending;
            RefreshProducts();
        }

        /// <summary>
        /// Refreshes the list of products. Called by navigation commands.
        /// </summary>
        private void RefreshProducts()
        {
            Products = DataAccess.GetProducts(start, itemCount, sortColumn, ascending, out totalItems);

            NotifyPropertyChanged("Start");
            NotifyPropertyChanged("End");
            NotifyPropertyChanged("TotalItems");
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    internal class RelayCommand : ICommand
    {
        private readonly Action<object> execute;

        private readonly Predicate<object> canExecute;

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        #endregion
    }

    public class Product
    {
        private readonly int id;

        private readonly string name;

        public Product(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int Id { get { return id; } }

        public string Name { get { return name; } }
    }

    public static class DataAccess
    {
        private static ObservableCollection<Product> products = new ObservableCollection<Product>
    {
      new Product(1, "Book"),
      new Product(2, "Desktop Computer"),
      new Product(3, "Notebook"),
      new Product(4, "Netbook"),
      new Product(5, "Business Software"),
      new Product(6, "Antivirus Software"),
      new Product(7, "Game Console"),
      new Product(8, "Handheld Game Console"),
      new Product(9, "Mobile Phone"),
      new Product(10, "Multimedia Software"),
      new Product(11, "PC Game")      
    };

        public static ObservableCollection<Product> GetProducts(int start, int itemCount, string sortColumn, bool ascending, out int totalItems)
        {
            totalItems = products.Count;
            ObservableCollection<Product> sortedProducts = new ObservableCollection<Product>();
            switch (sortColumn)
            {
                case ("Id"):
                    sortedProducts = new ObservableCollection<Product>
                    (
                      from p in products
                      orderby p.Id
                      select p
                    );
                    break;
                case ("Name"):
                    sortedProducts = new ObservableCollection<Product>
                    (
                      from p in products
                      orderby p.Name
                      select p
                    );
                    break;
            }

            sortedProducts = ascending ? sortedProducts : new ObservableCollection<Product>(sortedProducts.Reverse());
            ObservableCollection<Product> filteredProducts = new ObservableCollection<Product>();
            for (int i = start; i < start + itemCount && i < totalItems; i++)
            {
                filteredProducts.Add(sortedProducts[i]);
            }
            return filteredProducts;
        }
    }

}
