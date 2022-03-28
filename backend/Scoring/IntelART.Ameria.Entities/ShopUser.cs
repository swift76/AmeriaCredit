namespace IntelART.Ameria.Entities
{
    public class ShopUser : ApplicationUser
    {
        public string SHOP_CODE { get; set; }

        public bool? IS_MANAGER { get; set; }

        public string SHOP_NAME { get; set; }

        public string MOBILE_PHONE { get; set; }
    }
}
