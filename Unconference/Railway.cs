using System;
using System.Collections.Generic;
using System.Linq;
using NFluent;
using NUnit.Framework;

namespace Unconference
{
    public class Railway
    {
        public void Compose()
        {
            var list = new List<int> {1, 2, 3, 4};
            var result = list
                    .Where(i => i >= 2) // filter   IEnumerable<T> Where<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
                    .Select(i => 10 / i) // map     IEnumerable<TOutput> Select<TInput, TOutput>(this IEnumerable<TInput> collection, Func<TInput, TOutput> morphism)
                    .Sum(); // Suite d'instructions déclaratives
        }

        public IResult<int> Transform1(int value)
        {
            if (value == 0)
                return new Error<int>("Division par 0");
            return new Success<int>(10 / value);
        }

        public IResult<string> Transform2(int value)
        {
            return new Success<string>(value.ToString());
        }

        public IResult<string> Transform3(string prefix, string input)
        {
            return new Success<string>($"{prefix} : {input}");
        }

        public IResult<string> ComposeRailway(int value)
        {
            return Transform1(value)
                .Then(Transform2)
                .Then(input => Transform3("Resultat", input));
        }

        [Test]
        public void ShouldComposeFunctions([Random(1, 10, 3)] int value)
        {
            Check.That(ComposeRailway(value)).IsEqualTo(new Success<string>($"Resultat : {10 / value}"));
        }

        [Test]
        public void ShouldReturnErrorWhenValueIsZero()
        {
            Check.That(ComposeRailway(0)).IsEqualTo(new Error<string>("Division par 0"));
        }
    }

    public interface IResult<T> { }

    public struct Error<T> : IResult<T>
    {
        public string Message { get; }

        public Error(string message)
        {
            Message = message;
        }
    }

    public struct Success<T> : IResult<T>
    {
        public T Result { get; }

        public Success(T result)
        {
            Result = result;
        }
    }

    public static class RailwayExtensions
    {
        public static IResult<TOutput> Then<TInput, TOutput>(
            this IResult<TInput> input,
            Func<TInput, IResult<TOutput>> transform)
        {
            switch (input)
            {
                case Success<TInput> success:
                    return transform(success.Result);
                case Error<TInput> error:
                    return new Error<TOutput>(error.Message);
            }

            throw new NotImplementedException();
        }
    }
}
