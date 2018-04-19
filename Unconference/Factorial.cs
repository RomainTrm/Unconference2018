using NFluent;
using NUnit.Framework;

namespace Unconference
{
    [TestFixture]
    public class Tests
    {
        public static int Factorial(int i)
        {
            if (i <= 1) return 1; // Vide
            return i * Factorial(i - 1); // head * tail
        }

        [Test]
        public void ShouldComputeFactorial()
        {
            var result = Factorial(6);
            // 6 * 5 * 4 * 3 * 2 * 1
            // [6, 5, 4, 3, 2, 1]
            Check.That(result).IsEqualTo(720);
        }

        [Test]
        public void ShouldComputeFactorialOfZero()
        {
            var result = Factorial(0);
            Check.That(result).IsEqualTo(1);
        }
    }
}