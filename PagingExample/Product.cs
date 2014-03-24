
namespace PagingExample
{
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
}
