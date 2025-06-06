namespace Warehouse.App.MVVM.Models
{
    public class ApiResult<T>
    {
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsSuccess => ErrorMessage == null;
    }
}
