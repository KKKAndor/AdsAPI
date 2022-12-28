using Ads.Application.User.Commands.CreateUser;
using Ads.Application.User.Queries.GetUserList;
using Ads.Domain.Models;
using Ads.WebApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ads.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IMapper _mapper;

        public UserController(IMapper mapper) => _mapper = mapper;

        /// <summary>
        /// Gets the list of Users
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /User/GetAllUsers
        ///             ?Contain=Some
        ///             &PageNumber=1
        ///             &PageSize=5
        ///             &OrderBy=Name - desc
        /// </remarks>
        /// <returns>Returns AdListVm</returns>
        /// <param name="userParameters">User parameters for sorting, searching, filtering</param>
        /// <response code="200">Success</response>
        [HttpGet("GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDataListVm>> GetAllUsers([FromQuery] UserParameters userParameters)
        {
            var query = new GetUserListQuery 
            {
                userParameters = userParameters
            };

            var vm = await Mediator.Send(query);

            var metadata = new
            {
                vm.UserList.TotalCount,
                vm.UserList.PageSize,
                vm.UserList.CurrentPage,
                vm.UserList.TotalPages,
                vm.UserList.HasNext,
                vm.UserList.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(vm);
        }

        /// <summary>
        /// Creates User
        /// </summary> 
        /// <remarks>
        /// Sample request:
        /// POST /User/CreateUser
        /// {
        ///     "name": "string",
        ///     "isAdmin": true
        /// }
        /// </remarks>
        /// <param name="createUserDto">createAdDto object</param>
        /// <returns>Returns id (Guid)</returns>
        /// <response code="201">Success</response>
        [HttpPost("CreateUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Guid>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            var command = _mapper.Map<CreateUserCommand>(createUserDto);
            var id = await Mediator.Send(command);
            return StatusCode(201, id);
        }
    }
}
