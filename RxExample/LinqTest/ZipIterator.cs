using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqTest
{
  public class ZipIterator<TLeft, TRight, TResult> : IteratorBase<TResult>
  {
    #region fields
    private IEnumerable<TLeft> _left;
    private IEnumerable<TRight> _right;
    private IEnumerator<TLeft> _leftEnumerator;
    private IEnumerator<TRight> _rightEnumerator;
    private Func<TLeft, TRight, TResult> _selector;
    #endregion

    #region constructors
    public ZipIterator(IEnumerable<TLeft> left, IEnumerable<TRight> right,
      Func<TLeft, TRight, TResult> selector)
      : this(left, right, selector, null, null)
    { }

    public ZipIterator(IEnumerable<TLeft> left, IEnumerable<TRight> right,
      Func<TLeft, TRight, TResult> selector, Action<TResult> currentChanged, Action terminated)
      : base(currentChanged, terminated)
    {
      this._left = left;
      this._right = right;
      this._selector = selector;
    }
    #endregion

    #region override methods
    public override IEnumerator<TResult> GetEnumerator()
    {
      if (this._leftEnumerator == null || this._rightEnumerator == null)
      {
        return this;
      }
      return new ZipIterator<TLeft, TRight, TResult>
        (_left, _right, _selector, _currentChanged, _terminated);
    }

    public override bool MoveNext()
    {
      if (this._leftEnumerator == null || this._rightEnumerator == null)
      {
        this._leftEnumerator = this._left.GetEnumerator();
        this._rightEnumerator = this._right.GetEnumerator();
      }
      if (!this._leftEnumerator.MoveNext() || !this._rightEnumerator.MoveNext())
      {
        OnTerminated();
        return false;
      }
      this.Current = _selector(this._leftEnumerator.Current, this._rightEnumerator.Current);
      OnCurrentChanged();
      return true;
    }

    public override void Dispose()
    {
      if (this._leftEnumerator != null)
        this._leftEnumerator.Dispose();
      if (this._rightEnumerator != null)
        this._rightEnumerator.Dispose();
    }
    #endregion
  }
}
