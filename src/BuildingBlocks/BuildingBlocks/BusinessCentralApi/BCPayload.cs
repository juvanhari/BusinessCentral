namespace BuildingBlocks.BusinessCentralApi
{
    public class ODataResponse
    {
        public string OdataContext { get; set; } = default!;
        public string Value { get; set; } = default!;
    }

    public class Item
    {
        public string ItemNo { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal UnitPrice { get; set; } = default!;
        public string ItemCategory { get; set; } = default!;
    }
}
