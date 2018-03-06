using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Ninject;
using Wkz.Bgs.MasterCodex.App.Helper;
using Wkz.Bgs.MasterCodex.ViewModel;
using Wkz.Bgs.MasterCodex.ViewModel.Components.Responsibility;
using Wkz.Bgs.MasterCodexEditor.Domain.Interfaces;
using Wkz.Bgs.MasterCodexEditor.Persistence.Models;

namespace Wkz.Bgs.MasterCodex.App
{
    [RoutePrefix("api/responsibility")]
    public class ResponsibilityController : ApiController
    {
        private readonly IBgsResponsibilityService _responsibilityService;

        public ResponsibilityController(IBgsResponsibilityService responsibilityService)
        {
            _responsibilityService = responsibilityService;
        }



        [Route("")]
        [HttpPost()]
        public IHttpActionResult Post(
               ResponsibilityViewModel responsibility)
        {
            IHttpActionResult ret = null;
            if (ModelState.IsValid)
            {
                var savedResponsibility = _responsibilityService.AddResponsibility(responsibility);
                ret = Created<ResponsibilityViewModel>(
                        Request.RequestUri +
                        savedResponsibility.Id.ToString(),
                          responsibility);

            }
            else
            {
                System.Web.Http.ModelBinding.ModelStateDictionary errors =
                   BgsHelper.ConvertToModelState(ModelState);

                ret = BadRequest(errors);
            }

            return ret;
        }

        [HttpPut()]
        [Route("{id}")]
        public IHttpActionResult Put(int id,
                    ResponsibilityViewModel responsibility)
        {
            IHttpActionResult ret = null;

            if (ModelState.IsValid)
            {
                responsibility.Id = id;
                _responsibilityService.UpdateResponsibility(responsibility);
                ret = Ok(responsibility);
            }
            else
            {
                System.Web.Http.ModelBinding.ModelStateDictionary errors =
                   BgsHelper.ConvertToModelState(ModelState);

                ret = BadRequest(errors);
            }

            return ret;
        }

        [Route("{id}")]
        [HttpDelete()]
        public IHttpActionResult Delete(int id)
        {
            IHttpActionResult ret = null;

            var responsibility = _responsibilityService.GetResponsibility(id,true);

            if (responsibility.Id > 0)
            {
                _responsibilityService.DeleteResponsibility(id);
                ret = Ok(true);
            }
            else
            {
                ret = NotFound();
            }

            return ret;
        }
    }
}