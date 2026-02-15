namespace AdminPanel.ETicaretMVC.Models.Wrappers
{
    public class PagedResponse<T> : ServiceResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, int totalRecords)
        {
            this.Data = data;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalRecords = totalRecords;
            this.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            this.Success = true;
            this.Message = "Veriler başarıyla getirildi.";
        }
    }
}