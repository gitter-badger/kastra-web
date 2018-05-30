using System.Collections.Generic;
using System.Linq;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Kastra.Web.API.Models.Place;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kastra.Web.API.Controllers
{
	[Area("Api")]
    [Authorize("Administration")]
    public class PlaceController : Controller
    {
        private readonly IViewManager _viewManager;

        public PlaceController(IViewManager viewManager)
        {
            _viewManager = viewManager;
        }

        [HttpGet]
        public IActionResult ListByPageId(int id = 0)
        {
			PageInfo page = null;
			IList<PlaceInfo> places = _viewManager.GetPlacesList(true);
			List<PlaceModel> placeModels = null;
            
            if(id > 0)
			{
				// Get places by page id
				page = _viewManager.GetPage(id);
                
				if (page == null)
					return NotFound();

				places = places
					.Where(p => p.PageTemplateId == page.PageTemplateId 
					       && !p.Modules.Any(m => m.PageId == page.PageId))
					.ToList();
			}

			placeModels = new List<PlaceModel>(places.Count);

            foreach (PlaceInfo place in places)
            {
                placeModels.Add(ToPlaceModel(place));
            }

            return Json(placeModels);
        }

        #region Private methods

        public static PlaceModel ToPlaceModel(PlaceInfo placeInfo)
        {
            PlaceModel model = new PlaceModel();
			model.Id = placeInfo.PlaceId;
			model.Keyname = placeInfo.KeyName;
			model.TemplateId = placeInfo.PageTemplateId;

            return model;
        }

        #endregion
    }
}
