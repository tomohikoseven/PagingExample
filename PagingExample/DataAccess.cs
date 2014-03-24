using System.Linq;
using System.Collections.ObjectModel;

namespace PagingExample
{
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
