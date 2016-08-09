using System.Web.Mvc;

namespace ApartmentCash.Areas.DataCapture
{
    public class DataCaptureAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DataCapture";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DataCapture_default",
                "DataCapture/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}