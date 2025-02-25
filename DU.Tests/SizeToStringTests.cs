using DU.ConsoleApp;

namespace DU.Tests
{
    public class SizeToStringTests
    {
        [Test]
        public void ByteCorrectLength()
        {
            Assert.Multiple(() =>
            {
                Assert.That(0L.SizeToString(), Is.EqualTo("0,0b"));
                Assert.That(1L.SizeToString(), Is.EqualTo("1,0b"));
                Assert.That(10L.SizeToString(), Is.EqualTo("10,0b"));
                Assert.That(100L.SizeToString(), Is.EqualTo("100,0b"));
                Assert.That(999L.SizeToString(), Is.EqualTo("999,0b"));
            });
        }

        [Test]
        public void OtherCorrectLength()
        {
            Assert.Multiple(() =>
            {
                Assert.That(1000L.SizeToString(), Is.EqualTo("1,0kb"));
                Assert.That(1999L.SizeToString(), Is.EqualTo("2,0kb"));
                Assert.That(2121L.SizeToString(), Is.EqualTo("2,1kb"));
                Assert.That(3670016L.SizeToString(), Is.EqualTo("3,5mb"));
            });
        }
    }
}