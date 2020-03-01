using System;
using CodingMilitia.PlayBall.GroupManagement.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Features
{
    public static class ResultExtensions
    {
        public static ActionResult<TValue> ToActionResult<TValue>(this Either<Error, TValue> result)
            => result.Fold(
                left: error => ToErrorResult<TValue>(error),
                right: success => ToSuccessResult(success, value => value));

        public static ActionResult<TModel> ToActionResult<TValue, TModel>(
            this Either<Error, TValue> result,
            Func<TValue, TModel> valueMapper)
            => result.Fold(
                left: error => ToErrorResult<TModel>(error),
                right: success => ToSuccessResult(success, valueMapper)
            );

        public static ActionResult ToUntypedActionResult<TValue>(
            this Either<Error, TValue> result,
            Func<TValue, ActionResult> successMapper)
            => result.Fold(
                left: error => ToErrorResult(error),
                right: successMapper);

        private static ActionResult<TModel> ToSuccessResult<TValue, TModel>(
            TValue result,
            Func<TValue, TModel> valueMapper)
            => result is Unit
                ? (ActionResult<TModel>) new NoContentResult()
                : valueMapper(result);

        private static ActionResult<TModel> ToErrorResult<TModel>(Error error)
            => error.Accept<ErrorMappingVisitor<TModel>, ActionResult<TModel>>(new ErrorMappingVisitor<TModel>());
        
        private static ActionResult ToErrorResult(Error error)
            => error.Accept<ErrorMappingVisitor<object>, ActionResult<object>>(new ErrorMappingVisitor<object>()).Result;
    }
}