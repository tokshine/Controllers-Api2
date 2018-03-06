using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Ninject;
using Wkz.Bgs.MasterCodex.App.Helper;
using Wkz.Bgs.MasterCodex.App.Models;
using Wkz.Bgs.MasterCodex.ViewModel;
using Wkz.Bgs.MasterCodex.ViewModel.Components;
using Wkz.Bgs.MasterCodex.ViewModel.Components.Folder;
using Wkz.Bgs.MasterCodexEditor.Domain.Interfaces;
using Wkz.Bgs.MasterCodexEditor.Persistence.Models;
using Wkz.Bgs.MasterCodexEditor.SharedObjects.Enums;

namespace Wkz.Bgs.MasterCodex.App
{
    [RoutePrefix("api/folder")]
    public class FolderController : ApiController
    {
        private readonly IFolderService _folderService;

        public FolderController(IFolderService folderService)
        {
            _folderService = folderService;
        }



        [Route("")]
        [HttpPost()]
        public IHttpActionResult Post(
               FolderViewModel folder)
        {
            IHttpActionResult ret = null;
            if (ModelState.IsValid)
            {
                folder.Components = new List<BasicComponentViewModel>();
                if (folder.TargetComponentId > 0)
                {
                    folder.Components.Add(new BasicComponentViewModel()
                    {
                        Id = folder.TargetComponentId,
                        ComponentType = BgsComponentsEnum.Task
                    });
                }

                var savedFolder = _folderService.AddFolder(folder);
                ret = Created<FolderViewModel>(
                        Request.RequestUri +
                        savedFolder.Id.ToString(),
                          folder);

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
                    FolderViewModel folder)
        {
            IHttpActionResult ret = null;

            if (ModelState.IsValid)
            {
                folder.Id = id;
                _folderService.UpdateFolder(folder);
                ret = Ok(folder);
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

            var folder = _folderService.GetFolder(id);

            if (folder.Id > 0)
            {
                _folderService.DeleteFolder(id);
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