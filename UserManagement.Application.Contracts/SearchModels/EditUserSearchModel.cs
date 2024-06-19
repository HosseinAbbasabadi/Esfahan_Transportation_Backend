using System;

namespace UserManagement.Application.Contracts.SearchModels
{
    public class EditUserSearchModel
    {
        public Guid Guid { get; set; }

        public EditUserSearchModel(Guid id)
        {
            Guid = id;
        }
    }
}