using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace chat.Helpers
{
    public static class UIMessageHandler
    {
        public static void AddError(this ITempDataDictionary tempData, string message)
        {
            if (message == null)
                message = "Error!";

            tempData["Message"] = new string[] { "alert-danger", message };
        }

        public static void AddSuccess(this ITempDataDictionary tempData, string message)
        {
            if (message == null)
                message = "Success!";

            tempData["Message"] = new string[] { "alert-success", message };
        }
    }
}