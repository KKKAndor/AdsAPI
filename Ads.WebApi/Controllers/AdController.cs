using Ads.Application.Ads.Commands.CreateAd;
using Ads.Application.Ads.Commands.DeleteAd;
using Ads.Application.Ads.Commands.UpdateAd;
using Ads.Application.Ads.Queries.GetAdDetails;
using Ads.Application.Ads.Queries.GetAdList;
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
    public class AdController : BaseController
    {
        private readonly IMapper _mapper;

        public AdController(IMapper mapper) => _mapper = mapper;

        /// <summary>
        /// Gets the list of Ads
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /Ad/GetAllAds?UserId=b8da677c-11b5-4895-b523-149233e10568
        ///                   &Contain=Some
        ///                   &MinRating=1
        ///                   &MaxRating=77
        ///                   &MinCreationDate=2022-12-18T20:04:00
        ///                   &MaxCreationDate=2022-12-18T20:04:00
        ///                   &PageNumber=1
        ///                   &PageSize=5&OrderBy=Number - desc
        /// </remarks>
        /// <returns>Returns AdListVm</returns>
        /// <param name="AdsParameters">User parameters for sorting, searching, filtering</param>
        /// <response code="200">Success</response>
        [HttpGet("GetAllAds")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedList<AdLookUpDto>>> GetAllAds([FromQuery] AdsParameters AdsParameters)
        {
            var query = new GetAdListQuery 
            {
                AdsParameters = AdsParameters
            };            

            var vm = await Mediator.Send(query);

            var metadata = new
            {
                vm.Ads.TotalCount,
                vm.Ads.PageSize,
                vm.Ads.CurrentPage,
                vm.Ads.TotalPages,
                vm.Ads.HasNext,
                vm.Ads.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(vm);
        }

        /// <summary>
        /// Gets the list of Ads
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /Ad/GetAd/b8da677c-11b5-4895-b523-149233e10568?UserId=b8da677c-11b5-4895-b523-149233e10568
        /// </remarks>
        /// <returns>Returns AdListVm</returns>
        /// <param name="Id">ID (Guid)</param>
        /// <response code="200">Success</response>
        [HttpGet("GetAd/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AdDetailsVm>> GetAd(Guid Id, [FromQuery]Guid UserId)
        {
            var query = new GetAdDetailsQuery
            {
                Id = Id,
                UserId = UserId
            };

            var vm = await Mediator.Send(query);

            return Ok(vm);
        }

        /// <summary>
        /// Creates the Ad
        /// </summary> 
        /// <remarks>
        /// Sample request:
        /// POST /Ad/CreateAd
        /// {
        ///     "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///     "number": 0,
        ///     "description": "string",
        ///     "imagePath": "string",
        ///     "rating": 0,
        ///     "expirationDate": "2022-12-18T17:57:37.462Z"
        /// }
        /// </remarks>
        /// <param name="createAdDto">createAdDto object</param>
        /// <returns>Returns id (Guid)</returns>
        /// <response code="201">Success</response>
        /// <response code="400">Bad request</response>
        [HttpPost("CreateAd")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Guid>> CreateAd([FromBody] CreateAdDto createAdDto)
        {
            var command = _mapper.Map<CreateAdCommand>(createAdDto);

            var id = await Mediator.Send(command);
            
            return StatusCode(201, id);
        }

        /// <summary>
        /// Updates the Ad
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT /Ad/UpdateAd
        /// {
        ///     "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///     "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///     "number": 0,
        ///     "description": "string",
        ///     "imagePath": "string",
        ///     "rating": 0,
        ///     "expirationDate": "2022-12-18T17:57:37.462Z"
        /// }
        /// </remarks> 
        /// <param name="updateAdDto">UpdateAdDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        [HttpPut("UpdateAd")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAd([FromBody] UpdateAdDto updateAdDto)
        {
            var command = _mapper.Map<UpdateAdCommand>(updateAdDto);

            await Mediator.Send(command);
            
            return NoContent();
        }

        /// <summary>
        /// Deletes the Ad
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        /// Delete /Ad/DeleteAd/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///                     &3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// </remarks>
        /// <param name="Id">Ad id (Guid)</param>
        /// <param name="UserId">UserId (Guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad request</response>
        [HttpDelete("DeleteAd/{Id}&{UserId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAd(Guid Id, Guid UserId)
        {
            var command = new DeleteAdCommand
            {
                UserId = UserId,
                Id = Id
            };

            await Mediator.Send(command);

            return NoContent();
        }
    }
}
