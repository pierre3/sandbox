using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqTest
{
  public static class EnumerableExtensions
  {
    public static Iterator<T> ToIterator<T>(this IEnumerable<T> source)
    {
      return new Iterator<T>(source);
    }

    public static Iterator<T> ToIterator<T>(this IEnumerable<T> source,
      Action<T> currentChanged, Action terminated)
    {
      return new Iterator<T>(source, currentChanged, terminated);
    }

    public static IEnumerable<TResult> Zip<TLeft, TRight, TResult>(this IEnumerable<TLeft> left,
      IEnumerable<TRight> right, Func<TLeft, TRight, TResult> selector)
    {
      if (left == null || right == null || selector == null)
        throw new ArgumentNullException();
      //return ZipIterate(left, right, selector);
      return new ZipIterator<TLeft, TRight, TResult>(left, right, selector);
    }
    public static IEnumerable<TResult> Zip<TLeft, TRight, TResult>(this IEnumerable<TLeft> left,
      IEnumerable<TRight> right, Func<TLeft, TRight, TResult> selector,
      Action<TResult> currentChanged, Action terminated)
    {
      if (left == null || right == null || selector == null)
        throw new ArgumentNullException();

      return new ZipIterator<TLeft, TRight, TResult>(left, right, selector, currentChanged, terminated);
    }


    private static IEnumerable<TResult> ZipIterate<TLeft, TRight, TResult>(IEnumerable<TLeft> left,
      IEnumerable<TRight> right, Func<TLeft, TRight, TResult> selector)
    {
      using (var ieLeft = left.GetEnumerator())
      using (var ieRight = right.GetEnumerator())
      {
        while (ieLeft.MoveNext() && ieRight.MoveNext())
        {
          yield return selector(ieLeft.Current, ieRight.Current);
        }
      }
    }

  }
}
