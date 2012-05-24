using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqTest
{
  public class Iterator<T> : IteratorBase<T>
  {
    #region fields
    private IEnumerable<T> _source;
    private IEnumerator<T> _enumerator;
    #endregion

    #region constructors
    public Iterator(IEnumerable<T> source)
      : this(source, null, null)
    { }

    public Iterator(IEnumerable<T> source, Action<T> currentChanged, Action terminated)
      : base(currentChanged, terminated)
    {
      this._source = source;
    }
    #endregion

    #region override methods
    public override IEnumerator<T> GetEnumerator()
    {
      IEnumerator<T> e = this;
      if (_enumerator == null)
        return this;

      return new Iterator<T>(this._source, this._currentChanged, this._terminated);
    }

    public override bool MoveNext()
    {
      if (_enumerator == null)
        _enumerator = _source.GetEnumerator();

      if (!_enumerator.MoveNext())
      {
        OnTerminated();
        return false;
      }
      this.Current = _enumerator.Current;
      OnCurrentChanged();
      return true;
    }

    public override void Dispose()
    {
      if (this._enumerator != null)
        this._enumerator.Dispose();
    }
    #endregion

  }

  public abstract class IteratorBase<T> : IEnumerable<T>, IEnumerator<T>
  {
    #region events
    public event Action<T> CurrentChanged
    {
      add { _currentChanged += value; }
      remove { _currentChanged -= value; }
    }

    public event Action Terminated
    {
      add { _terminated += value; }
      remove { _terminated -= value; }
    }
    #endregion

    #region fields
    protected Action<T> _currentChanged;
    protected Action _terminated;
    #endregion

    #region properties
    public T Current { get; protected set; }
    #endregion

    #region constructors
    public IteratorBase()
    { }
    public IteratorBase(Action<T> currentChaned, Action terminated)
    {
      this._currentChanged = currentChaned;
      this._terminated = terminated;
    }
    #endregion

    #region abstract methods
    public abstract void Dispose();
    public abstract bool MoveNext();
    public abstract IEnumerator<T> GetEnumerator();
    #endregion

    #region public methods
    public void Reset()
    {
      throw new NotSupportedException();
    }
    #endregion

    #region protected methods 
    protected void OnTerminated()
    {
      if (this._terminated != null)
        this._terminated();
    }

    protected void OnCurrentChanged()
    {
      if (this._currentChanged != null)
        this._currentChanged(this.Current);
    }
    #endregion

    #region explicit interface implementation
    object System.Collections.IEnumerator.Current
    {
      get { return this.Current; }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }
    #endregion
  }

  
}
