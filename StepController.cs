using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Ninject;
using Wkz.Bgs.MasterCodex.App.Helper;
using Wkz.Bgs.MasterCodex.ViewModel;
using Wkz.Bgs.MasterCodex.ViewModel.Components.Step;
using Wkz.Bgs.MasterCodexEditor.Domain.Interfaces;
using Wkz.Bgs.MasterCodexEditor.Persistence.Models;

namespace Wkz.Bgs.MasterCodex.App
{
    [RoutePrefix("api/step")]
    public class StepController : ApiController
    {
        private readonly IStepService _stepService;

        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult ret = null;

            var steps = _stepService.GetStepsByTaskId(id);
           
            if (steps.Any())
            {
                ret = Ok(steps);
            }
            else
            {
                ret = NotFound();
            }

            return ret;
        }

        public StepController(IStepService stepService)
        {
            _stepService = stepService;
        }



        [Route("")]
        [HttpPost()]
        public IHttpActionResult Post(
               StepViewModel step)
        {
            IHttpActionResult ret = null;
            if (ModelState.IsValid)
            {
                var savedStep = _stepService.AddStep(step);
                ret = Created<StepViewModel>(
                        Request.RequestUri +
                        savedStep.Id.ToString(),
                          step);

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
                    StepViewModel step)
        {
            IHttpActionResult ret = null;

            if (ModelState.IsValid)
            {
                step.Id = id;
                _stepService.UpdateStep(step);
                ret = Ok(step);
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

            var step = _stepService.GetStep(id);

            if (step.Id > 0)
            {
                _stepService.DeleteStep(id);
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