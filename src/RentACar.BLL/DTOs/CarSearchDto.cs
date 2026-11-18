namespace RentACar.BLL.DTOs
{
    public class CarSearchDto
    {
        public string? SearchTerm { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public int? MinYear { get; set; }

        public int? MaxYear { get; set; }
    }
}
