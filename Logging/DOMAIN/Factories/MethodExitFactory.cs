using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PostMortem.Common.DOMAIN.Models;

namespace PostMortem.Logging.DOMAIN.Factories;

internal class MethodExitFactory
{
    public static MethodExit Create(IInvocation invocation) =>
        new()
        {
            Time = DateTime.Now,
            Output = invocation.ReturnValue
        };

    public static MethodExit Create(Exception ex) =>
        new()
        {
            Time = DateTime.Now,
            Output = ex
        };

    public static MethodExit Create(IActionResult result) =>
        new()
        {
            Time = DateTime.Now,
            Output = GetActionResultDetails(result)
        };

    private static object GetActionResultDetails(IActionResult result)
    {
        return result switch
        {
            ObjectResult objectResult => new
            {
                Content = objectResult.Value,
                StatusCode = objectResult.StatusCode
            },
            ContentResult contentResult => new
            {
                Content = contentResult.Content,
                StatusCode = contentResult.StatusCode
            },
            StatusCodeResult statusCodeResult => new
            {
                StatusCode = statusCodeResult.StatusCode
            },
            EmptyResult => new
            {

            },
            RedirectToActionResult redirectToActionResult => new
            {
                ActionName = redirectToActionResult.ActionName,
                ControllerName = redirectToActionResult.ControllerName,
                RouteValues = redirectToActionResult.RouteValues,
                StatusCode = 200
            },
            RedirectToRouteResult redirectToRouteResult => new
            {
                RouteName = redirectToRouteResult.RouteName,
                RouteValues = redirectToRouteResult.RouteValues,
                StatusCode = 200
            },
            RedirectToPageResult redirectToPageResult => new
            {
                Page = redirectToPageResult.PageName,
                RouteValues = redirectToPageResult.RouteValues,
                StatusCode = 200
            },
            FileContentResult fileContentResult => new
            {
                FileContents = fileContentResult.FileContents,
                ContentType = fileContentResult.ContentType,
                FileDownloadName = fileContentResult.FileDownloadName,
                StatusCode = 200
            },
            PhysicalFileResult physicalFileResult => new
            {
                FilePath = physicalFileResult.FileName,
                ContentType = physicalFileResult.ContentType,
                FileDownloadName = physicalFileResult.FileName,
                StatusCode = 200
            },
            VirtualFileResult virtualFileResult => new
            {
                FilePath = virtualFileResult.FileName,
                ContentType = virtualFileResult.ContentType,
                FileDownloadName = virtualFileResult.FileName,
                StatusCode = 200
            },
            ChallengeResult challengeResult => new
            {
                AuthenticationSchemes = challengeResult.AuthenticationSchemes,
                StatusCode = 401
            },
            ForbidResult => new
            {
                StatusCode = 403
            },
            _ => null!
        };
    }

}