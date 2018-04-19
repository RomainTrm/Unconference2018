using System.Collections.Generic;
using NFluent;
using NUnit.Framework;

namespace Unconference
{
    //List 'a = Nil | Cons 'a * List 'a
    //List 'a = Empty | head * tail

    public class FuncList<T>
    {
        public T Head { get; }
        public FuncList<T> Tail { get; }
        public bool IsEmpty { get; }

        private FuncList()
        {
            IsEmpty = true;
        }

        public static FuncList<T> Empty = new FuncList<T>();

        public FuncList(T head, FuncList<T> tail)
        {
            IsEmpty = false;
            Head = head;
            Tail = tail;
        }

        internal static int Factorial(int i)
        {
            var list = CreateFactorialList(i, FuncList<int>.Empty);
            return Factorial(list);
        }

        private static FuncList<int> CreateFactorialList(int head, FuncList<int> tail)
        {
            if (head <= 1) return tail;
            return new FuncList<int>(head, CreateFactorialList(head - 1, tail));
            // Dans le cas de factoriel de 4, cela revient à créer une liste :
            // 4::3::2::1::[] (head::tail)
            // On peut lire "liste vide qui est la tail de 1, qui est (la liste 1 * []) la tail de 2, etc..."
        }

        private static int Factorial(FuncList<int> list)
        {
            if (list.IsEmpty) return 1;
            return list.Head * Factorial(list.Tail);
        }

        public static int Sum(FuncList<int> list)
        {
            if (list.IsEmpty) return 0;
            return list.Head + Sum(list.Tail);
        }
    }

    [TestFixture]
    public class FuncListShould
    {
        private readonly FuncList<int> _list123 = 
            new FuncList<int>(1, new FuncList<int>(2, new FuncList<int>(3, FuncList<int>.Empty)));

        [Test]
        public void Sum()
        {
            var result = FuncList<int>.Sum(_list123);
            Check.That(result).IsEqualTo(6);
        }

        [Test]
        public void Factorial()
        {
            var result = FuncList<int>.Factorial(4);
            Check.That(result).IsEqualTo(24);
        }

        [Test]
        public void Enumerator()
        {
            var list = new List<int> { 1, 2, 3 };
            var enumerator = list.GetEnumerator();
            var result = Sum(enumerator);
            Check.That(result).IsEqualTo(6);
        }

        public static int Sum(List<int>.Enumerator enumerator)
        {
            var result = enumerator.Current;
            if (!enumerator.MoveNext())
                return result;
            return result + Sum(enumerator);
        }
    }
}