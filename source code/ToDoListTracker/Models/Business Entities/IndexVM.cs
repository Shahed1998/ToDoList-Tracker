namespace ToDoListTracker.Models.Business_Entities
{
    public class IndexVM
    {
        public bool IsSaved { get; set; } = false;
        public bool Notify { get; set; } = false;
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 5;
        public int count { get; set; } = 0;
        public bool prevPage { get; set; } = false;
        public string message { get; set; } = "";
    }
}
