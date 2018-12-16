using System.Collections.Generic;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Kastra.Web.API.Models.Page;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kastra.Web.API.Controllers
{
    [Area("Api")]
    [Authorize("Administration")]
    public class PageController : Controller
    {
        private readonly IViewManager _viewManager;

        public PageController(IViewManager viewManager)
        {
            _viewManager = viewManager;
        }

        [HttpGet]
        public IActionResult List()
        {
            IList<PageInfo> pages = _viewManager.GetPagesList();
            List<PageModel> pageModels = new List<PageModel>(pages.Count);

            foreach (PageInfo page in pages)
            {
                pageModels.Add(ToPageModel(page));
            }

            return Json(pageModels);
        }
        
		[HttpGet]
        public IActionResult Get(int id)
        {
			PageInfo page = _viewManager.GetPage(id);

			if (page == null)
				return NotFound();

			return Json(ToPageModel(page));
        }


        [HttpPost]
        
        public IActionResult Update([FromBody]PageModel model)
        {
            PageInfo page = new PageInfo();
            page.KeyName = model.KeyName;
            page.MetaDescription = model.MetaDescription;
            page.MetaKeywords = model.MetaKeywords;
            page.MetaRobot = model.MetaRobot;
            page.PageId = model.Id;
            page.PageTemplateId = model.TemplateId;
            page.Title = model.Name;

            _viewManager.SavePage(page);

            return Ok(new { page.PageId });
        }

        [HttpDelete]
        
        public IActionResult Delete([FromBody]int id)
        {
            if (_viewManager.GetPage(id) == null)
            {
                return BadRequest("Page not found");
            }

            if (_viewManager.DeletePage(id))
            {
                return Ok();
            }

            return BadRequest();
        }

        #region Private methods

        public static PageModel ToPageModel(PageInfo pageInfo)
        {
            PageModel model = new PageModel();
            model.Id = pageInfo.PageId;
            model.KeyName = pageInfo.KeyName;
            model.Name = pageInfo.Title;
			model.TemplateId = pageInfo.PageTemplateId;
			model.MetaKeywords = pageInfo.MetaKeywords;
			model.MetaDescription = pageInfo.MetaDescription;
			model.MetaRobot = pageInfo.MetaRobot;

            return model;
        }

        #endregion
    }
}
