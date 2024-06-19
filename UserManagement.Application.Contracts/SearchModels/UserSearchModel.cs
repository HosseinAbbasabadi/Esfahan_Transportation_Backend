namespace UserManagement.Application.Contracts.SearchModels
{
    public class UserSearchModel
    {
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Mobile { get; set; }
        public int RoleId { get; set; }
        public long SchoolId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
