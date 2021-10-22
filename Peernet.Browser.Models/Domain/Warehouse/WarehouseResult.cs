namespace Peernet.Browser.Models.Domain.Warehouse
{
    public class WarehouseResult
    {
        public int Status { get; set; } // should be enum but it is not defined in API yet

        public byte[] Hash { get; set; }
    }
}