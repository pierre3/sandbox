using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqTest
{
  class Program
  {
    static void Main(string[] args)
    {
      //0..9のシーケンスをWrapしたIteratorに変換
      var ite = Enumerable.Range(0, 10).ToIterator(
        n => Console.WriteLine("ite: " + n.ToString()),
        () => Console.WriteLine("ite: Terminated"));

      //Linqの拡張メソッドを使用
      Console.WriteLine("[Where-Select]");
      var query = ite.Where(n => n % 2 == 1)
                     .Select(n => "*" + n.ToString())
                     .ToIterator(s => Console.WriteLine("query: " + s),
                                 () => Console.WriteLine("query: Terminated"));

      Console.WriteLine("[Any]");
      var b = query.Any(s => s == "*5");
      Console.WriteLine("Any: " + b.ToString());

      //foreachで反復処理
      Console.WriteLine("[foreach]");
      foreach (var item in query)
        Console.WriteLine("foreach: " + item);

      Console.Read();
    }
  }
}
