using System;
using System.Collections.Generic;
using UserManagement.Application.Contracts.ViewModels;
using UserManagement.Application.Contracts.Commands.User;

namespace UserManagement.Application.Contracts.Contracts
{
    public interface IUserApplication
    {
        UserViewModel Login(Login command);
        void ChangePassword(ChangePassword command);
        void Create(CreateUser command);
        void Edit(EditUser command);
        EditUser GetBy(Guid guid);
        List<UserViewModel> GetList();
        void Delete(Guid id);
        void Lock(Guid guid);
        void Unlock(Guid guid);
        void OpenSession(OpenSession command);
        void CloseSession(Guid guid);
    }
}