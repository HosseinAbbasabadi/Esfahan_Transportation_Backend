using Microsoft.AspNetCore.Mvc;
using PhoenixFramework.Application.Query;
using UserManagement.Query.Contracts.ClassificationLevel;

namespace Um.Presentation.RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassificationLevelController : ControllerBase
    {
        private readonly IQueryBus _queryBus;

        public ClassificationLevelController(IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }

        [HttpGet("GetForCombo")]
        public IActionResult GetForCombo()
        {
            return new JsonResult(_queryBus.Dispatch<List<ClassificationLevelViewModel>>());
        }
    }
}