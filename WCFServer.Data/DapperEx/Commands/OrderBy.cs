namespace WCFServer.Data.DapperEx.Commands
{
    public class OrderBy
    {
        public static int Desc = 1;

        public static int Asc = 2;
        public string[] Properties { get; set; }

        public OrderBy(params string[] properties)
        {
            Properties = properties;
        }
        public OrderBy()
        {
            Properties = new string[] { };
        }
    }
}
