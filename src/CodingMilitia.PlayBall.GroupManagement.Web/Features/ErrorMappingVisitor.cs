using CodingMilitia.PlayBall.GroupManagement.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Features
{
    public readonly struct ErrorMappingVisitor<TModel> : Error.IErrorVisitor<ActionResult<TModel>>
    {
        public ActionResult<TModel> Visit(Error.NotFound result)
            => new NotFoundObjectResult(result.Message);


        public ActionResult<TModel> Visit(Error.Invalid result)
            => new BadRequestObjectResult(result.Message);


        public ActionResult<TModel> Visit(Error.Unauthorized result)
            => new UnauthorizedObjectResult(result.Message);
    }
}