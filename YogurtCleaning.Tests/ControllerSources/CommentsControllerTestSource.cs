using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Tests.ControllerSources;

public class CommentsControllerTestSource : IEnumerable
{
    public IEnumerator GetEnumerator()
    {
        yield return new object[]
        {
            new CommentRequest()
            {
                Summary = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed tempus suscipit tempus.",
                AuthorId = 1,
                OrderId = 1,
                Rating = 6
            },
            ApiErrorMessages.RatingIsOutOfRange
        };

        yield return new object[]
        {
            new CommentRequest()
            {
                Summary = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed tempus suscipit tempus.",
                AuthorId = 1,
                OrderId = 1
            },
            ApiErrorMessages.RatingIsRequred
        };

        yield return new object[]
        {
            new CommentRequest()
            {
                OrderId = 1,
                Rating = 5
            },
            ApiErrorMessages.AuthorIdIsRequred
        };

        yield return new object[]
        {
            new CommentRequest()
            {
                AuthorId = 0,
                Rating = 5
            },
            ApiErrorMessages.OrderIdIsRequred
        };

        yield return new object[]
        {
            new CommentRequest()
            {
                Summary = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                "Diam vel quam elementum pulvinar etiam non quam lacus suspendisse. Eget mi proin sed libero enim sed faucibus turpis. " +
                "In cursus turpis massa tincidunt. In cursus turpis massa tincidunt. Faucibus turpis in eu mi. " +
                "Risus quis varius quam quisque id diam vel quam. Tellus in hac habitasse platea dictumst vestibulum rhoncus. " +
                "Scelerisque purus semper eget duis. A diam sollicitudin tempor id eu. Amet risus nullam eget felis eget nunc lobortis mattis aliquam.",
                AuthorId = 0,
                Rating = 5
            },
            ApiErrorMessages.SummaryMaxLenght
        };

    }
}
