using System.Collections.Generic;
using PhoenixFramework.Application.Command;

namespace UserManagement.Application.Contracts.Setting;

public class UpdateSetting : ICommand
{
    public List<SettingItem> Items { get; set; }
}