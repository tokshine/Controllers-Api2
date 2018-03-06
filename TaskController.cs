using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Ninject;
using Wkz.Bgs.MasterCodex.App.Helper;
using Wkz.Bgs.MasterCodex.ViewModel;
using Wkz.Bgs.MasterCodex.ViewModel.Components.Task;
using Wkz.Bgs.MasterCodexEditor.Domain.Interfaces;
using Wkz.Bgs.MasterCodexEditor.Persistence.Models;

namespace Wkz.Bgs.MasterCodex.App
{
    [RoutePrefix("api/task")]
    public class TaskController : ApiController
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }



        [Route("")]
        [HttpPost()]
        public IHttpActionResult Post(
               BasicTaskViewModel task)
        {
            IHttpActionResult ret = null;
            if (ModelState.IsValid)
            {
                var savedTask = _taskService.AddTask(task);
                ret = Created<BasicTaskViewModel>(
                        Request.RequestUri +
                        savedTask.Id.ToString(),
                          task);

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
                    BasicTaskViewModel task)
        {
            IHttpActionResult ret = null;

            if (ModelState.IsValid)
            {
                task.Id = id;
                _taskService.UpdateTask(task);
                ret = Ok(task);
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

            var task = _taskService.GetTask(id);

            if (task.Id > 0)
            {
                _taskService.DeleteTask(id);
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