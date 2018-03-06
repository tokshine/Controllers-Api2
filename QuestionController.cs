using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Ninject;
using Wkz.Bgs.Core.Lib;
using Wkz.Bgs.MasterCodex.App.Helper;
using Wkz.Bgs.MasterCodex.ViewModel;
using Wkz.Bgs.MasterCodex.ViewModel.Components.Process;
using Wkz.Bgs.MasterCodex.ViewModel.Components.Step;
using Wkz.Bgs.MasterCodexEditor.Domain.Interfaces;
using Wkz.Bgs.MasterCodexEditor.Persistence.Models;

namespace Wkz.Bgs.MasterCodex.App
{
    [RoutePrefix("api/question")]
    public class QuestionController : ApiController
    {
        private readonly IQuestionService _questionService;

        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult ret = null;

            var question = _questionService.GetQuestion(id);


            SetEnumData(question);

            ret = Ok(question);

            return ret;
        }

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }



        [HttpGet()]
        [Route("validationTypes")]
        public IHttpActionResult GetValidationTypes()
        {
            IHttpActionResult ret = null;

            List<ValidationViewModel> validations = new List<ValidationViewModel>();
            foreach (
                ControlValidation controlValidation in (ControlValidation[]) Enum.GetValues(typeof (ControlValidation)))
            {
                if (controlValidation == ControlValidation.Novalidation) continue;
                validations.Add(new ValidationViewModel() {Name = controlValidation.ToString()});
            }

            if (validations.Any())
            {
                ret = Ok(validations);
            }
            else
            {
                ret = NotFound();
            }

            return ret;
        }


        [HttpGet()]
        [Route("controlTypes")]
        public IHttpActionResult GetControlTypes()
        {
            IHttpActionResult ret = null;

           var controls = new List<string>();
            foreach (
                ControlType controlType in (ControlType[])Enum.GetValues(typeof(ControlType)))
            {
                
                controls.Add( controlType.ToString() );
            }

            if (controls.Any())
            {
                ret = Ok(controls);
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
               QuestionViewModel question)
        {
            IHttpActionResult ret = null;
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(question.SourceTypeString))
                {
                    question.SourceType = (SourceType) Enum.Parse(typeof (SourceType), question.SourceTypeString);
                 }
                if (!String.IsNullOrEmpty(question.ControlTypeString))
                {
                    question.ControlType = (ControlType)Enum.Parse(typeof(ControlType), question.ControlTypeString);
                }
                if (question.Validations != null)
                {
                    question.SelectedValidations.AddRange(question.Validations.Where(x => x.IsChecked).Select(s => s.Name));
                }
                var id = _questionService.SaveQuestion(question);
                var ques = _questionService.GetQuestion(id);


                SetEnumData(ques);

                ret = Created<QuestionViewModel>(
                        Request.RequestUri +
                        ques.Id.ToString(),
                          ques);

            }
            else
            {
                System.Web.Http.ModelBinding.ModelStateDictionary errors =
                   BgsHelper.ConvertToModelState(ModelState);

                ret = BadRequest(errors);
            }

            return ret;
        }

        private static void SetEnumData(QuestionViewModel ques)
        {
            List<ValidationViewModel> validations = new List<ValidationViewModel>();
            foreach (
                ControlValidation controlValidation in (ControlValidation[]) Enum.GetValues(typeof (ControlValidation)))
            {
                if (controlValidation == ControlValidation.Novalidation) continue;
                if (controlValidation == ((ControlValidation) ques.ValidationTypes & controlValidation))
                {
                    validations.Add(new ValidationViewModel() {Name = controlValidation.ToString(), IsChecked = true});
                }
                else
                {
                    validations.Add(new ValidationViewModel() {Name = controlValidation.ToString()});
                }
            }
            ques.ControlTypeString = ques.ControlType.ToString();
            ques.SourceTypeString = ques.SourceType.ToString();
            ques.Validations = validations;
        }

        [HttpPut()]
        [Route("{id}")]
        public IHttpActionResult Put(int id,
                    QuestionViewModel question)
        {
            IHttpActionResult ret = null;

            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(question.SourceTypeString))
                {
                    question.SourceType = (SourceType)Enum.Parse(typeof(SourceType), question.SourceTypeString);
                }
                if (!String.IsNullOrEmpty(question.ControlTypeString))
                {
                    question.ControlType = (ControlType)Enum.Parse(typeof(ControlType), question.ControlTypeString);
                }
                if (question.Validations != null)
                {
                    question.SelectedValidations.AddRange(question.Validations.Where(x => x.IsChecked).Select(s => s.Name));
                }
                question.Id = id;
                _questionService.SaveQuestion(question);
                var ques = _questionService.GetQuestion(id);
                SetEnumData(ques);
                ret = Ok(ques);
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

            var question = _questionService.GetQuestion(id);

            if (question.Id > 0)
            {
                _questionService.DeleteQuestion(id);
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