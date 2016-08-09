using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WX.Common
{
    public class WebHelper
    {
        public static bool IsMobile()
        {

            System.Web.HttpContext context = System.Web.HttpContext.Current;
            if (context != null)
            {
                System.Web.HttpRequest request = context.Request;
                if (request.Browser.IsMobileDevice)
                    return true;
                string MobileUserAgent = "iphone|android|nokia|zte|huawei|lenovo|samsung|motorola|sonyericsson|lg|philips|gionee|htc|coolpad|symbian|sony|ericsson|mot|cmcc|iemobile|sgh|panasonic|alcatel|cldc|midp|wap|mobile|blackberry|windows ce|mqqbrowser|ucweb";

                Regex MOBILE_REGEX = new Regex(MobileUserAgent);
                if (string.IsNullOrEmpty(request.UserAgent) && MOBILE_REGEX.IsMatch(request.UserAgent.ToLower()))
                    return true;
            }
            return false;
        }
    }
}
