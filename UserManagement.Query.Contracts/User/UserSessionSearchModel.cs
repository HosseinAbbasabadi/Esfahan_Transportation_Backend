using System;

namespace UserManagement.Query.Contracts.User
{
    public class UserSessionSearchModel
    {
        public Guid? UserGuid { get; set; }
        public string StartDatePer { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string EndDatePer { get; set; }
        public string? ClientIpAddress { get; set; }
    }
}
