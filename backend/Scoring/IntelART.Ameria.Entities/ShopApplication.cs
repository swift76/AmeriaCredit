namespace IntelART.Ameria.Entities
{
    /// <summary>
    /// Basic information about loan applications
    /// for shop users and shop managers
    /// </summary>
    public class ShopApplication : Application
    {
        public string SHOP_USER_NAME { get; set; }
        public string NAME           { get; set; }
    }
}
