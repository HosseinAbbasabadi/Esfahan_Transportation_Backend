using PhoenixFramework.Dapper;
using System.Collections.Generic;
using PhoenixFramework.Application.Query;
using UserManagement.Query.Contracts.Setting;

namespace UserManagement.Query;

public class SettingQueryHandler :
    IQueryHandler<List<SettingViewModel>>
{
    private readonly BaseDapperRepository _repository;

    public SettingQueryHandler(BaseDapperRepository repository)
    {
        _repository = repository;
    }

    public List<SettingViewModel> Handle()
    {
        //var calibrationRequestAnswerDeadLine = _settingService.Fetch<CalibrationRequestSettings>();
        return _repository.Select<SettingViewModel>($@"
                        SELECT 
	                        S.Id,
	                        S.Description,
	                        S.FieldType,
	                        SD.Value,
	                        S.MinValue,
	                        S.MaxValue,
	                        S.ListName
                        FROM tbSetting AS S 
	                        LEFT JOIN tbSettingDetail AS SD ON 
		                        S.Id = SD.SettingId
                        WHERE S.FieldType IS NOT NULL 
                        ORDER BY S.Sort");
    }
}