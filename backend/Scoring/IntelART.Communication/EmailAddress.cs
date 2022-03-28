namespace IntelART.Communication
{
    public class EmailAddress
    {
        public string FullName { get; private set; }
        public string Address { get; private set; }

        public EmailAddress(string address)
            : this(null, address)
        {
        }

        public EmailAddress(string fullName, string address)
        {
            this.FullName = fullName;
            this.Address = address;
        }
    }
}
