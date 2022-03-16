using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Domain.Models.RoleViewModel;

namespace ULMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [Route("AddRole")]
        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] CreateRoleRequestModel model)
        {
            return Ok(await _roleService.AddRole(model));         
        }

        [Route("GetRoleByName/{name}")]
        [HttpGet]
        public async Task<IActionResult> GetRoleByName([FromRoute] string name)
        {
            return Ok(await _roleService.GetRoleByName(name));
        }
    }
}
