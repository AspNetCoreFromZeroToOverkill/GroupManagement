using System;
using CodingMilitia.PlayBall.GroupManagement.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Features
{
    public static class ResultExtensions
    {
        public static ActionResult<TValue> ToActionResult<TValue>(this Either<Error, TValue> result)
            => result switch
            {
                Either<Error, TValue>.Left error
                => error.Value.Accept<ResultMappingVisitor<TValue>, ActionResult<TValue>>(
                    new ResultMappingVisitor<TValue>()),

                Either<Error, TValue>.Right success => Map(success.Value, value => value)
            };
        
        public static ActionResult<TModel> ToActionResult<TValue, TModel>(
            this Either<Error, TValue> result,
            Func<TValue, TModel> valueMapper)
            => result switch
            {
                Either<Error, TValue>.Left error
                => error.Value.Accept<ResultMappingVisitor<TModel>, ActionResult<TModel>>(
                    new ResultMappingVisitor<TModel>()),

                Either<Error, TValue>.Right success => Map(success.Value, valueMapper)
            };

        private static ActionResult<TModel> Map<TValue, TModel>(
            TValue result,
            Func<TValue, TModel> valueMapper)
            => result is Unit
                ? (ActionResult<TModel>) new ContentResult()
                : valueMapper(result);
    }
}