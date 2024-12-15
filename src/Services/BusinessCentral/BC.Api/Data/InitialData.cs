namespace BC.Api.Data
{
    internal class InitialData
    {
        public static IEnumerable<Product> Products =>
        new List<Product>
           {
              Product.Create("5334c996-8457-4cf0-815c-ed2b77c4ff61","10000","Test Product","", 10),
              Product.Create("5334c996-8457-4cf0-815c-ed2b77c4ff62","10001","BiCycle","", 100)
           };
    }
}
