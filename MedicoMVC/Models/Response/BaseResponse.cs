namespace MedicoMVC.Models.Response
{
    public class BaseResponse
    {
        public bool Succsess { get; set; }
        public string ErrorMessage { get; set; } = default!;

    }
    public class BaseResponseGeneric<T>:BaseResponse
    {
        public T Data { get; set; } = default!;
    }

    public class PaginationResponse<T> : BaseResponse
    {
        public ICollection<T> Data { get; set; } = default!;
        public int Total { get; set; }
    }

}
