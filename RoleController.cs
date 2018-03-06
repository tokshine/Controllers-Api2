using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Ninject;
using Wkz.Bgs.MasterCodex.App.Helper;
using Wkz.Bgs.MasterCodex.ViewModel;
using Wkz.Bgs.MasterCodex.ViewModel.Components.Role;
using Wkz.Bgs.MasterCodexEditor.Domain.Interfaces;
using Wkz.Bgs.MasterCodexEditor.Persistence.Models;

namespace Wkz.Bgs.MasterCodex.App
{
    [RoutePrefix("api/role")]
    public class RoleController : ApiController
    {
        private readonly IBgsRoleService _roleService;

        public RoleController(IBgsRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet()]
        [Route("roleTypes")]
        public IHttpActionResult GetRoleTypes()
        {
            IHttpActionResult ret = null;

            var roleTypes = _roleService.GetRoleTypes();

            if (roleTypes.Any())
            {
                ret = Ok(roleTypes);
            }
            else
            {
                ret = NotFound();
            }

            return ret;
        }



        [HttpGet()]
        [Route("levels")]
        public IHttpActionResult GetRoleLevel()
        {
            IHttpActionResult ret = null;

            var roleLevel = _roleService.GetRoleLevel();

            if (roleLevel.Any())
            {
                ret = Ok(roleLevel);
            }
            else
            {
                ret = NotFound();
            }

            return ret;
        }



        [Route("")]
        [HttpPost()]
        public IHttpActionResult Post(
               RoleViewModel role)
        {
            IHttpActionResult ret = null;
           
            if (ModelState.IsValid)
            {
                var savedRole =_roleService.AddRole1(role);
                ret = Created<RoleViewModel>(
                        Request.RequestUri +
                        savedRole.Id.ToString(),
                          role);
                
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
                    RoleViewModel role)
        {
            IHttpActionResult ret = null;
            if (ModelState.IsValid)
            {
                role.Id = id;
                _roleService.UpdateRole1(role);
                ret = Ok(role);
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
            
            var role = _roleService.GetRole1(id);
            
            if (role.Id > 0)
            {
                _roleService.DeleteRole(id);
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