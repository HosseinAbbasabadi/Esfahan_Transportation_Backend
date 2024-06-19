using PhoenixFramework.Dapper;
using PhoenixFramework.Application.Command;
using UserManagement.Application.Contracts.Setting;

namespace UserManagement.Application;

public class SettingCommandHandler : ICommandHandler<UpdateSetting>
{
    private readonly BaseDapperRepository _repository;

    public SettingCommandHandler(BaseDapperRepository repository)
    {
        _repository = repository;
    }

    public void Handle(UpdateSetting command)
    {
        foreach (var item in command.Items)
        {
            var sql = "UPDATE tbSettingDetail SET VALUE =";

            if (item.FieldType == "bit")
                sql += $" {item.Value} ";
            else
                sql += $" '{item.Value}' ";

            sql += $"WHERE SettingId = {item.SettingId}";

            _repository.Execute(sql);
        }
    }
}