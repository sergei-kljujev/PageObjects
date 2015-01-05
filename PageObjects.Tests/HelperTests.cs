using NUnit.Framework;

namespace PageObjects.Tests
{
    [TestFixture]
    public class HelperTests
    {
        [Test]
        public void If_Arg_0_Then_NumberOfOnes_Returns_0()
        {
            uint arg = 0x00000000;
            
            Assert.That(Helper.NumberOfOnes(arg), Is.EqualTo(0));
        }

        [Test]
        public void If_Arg_Contains_1_Then_NumberOfOnes_Returns_1()
        {
            uint arg = 0x10000000;

            Assert.That(Helper.NumberOfOnes(arg), Is.EqualTo(1));
        }

        [Test]
        public void If_Arg_Contains_F_Then_NumberOfOnes_Returns_4()
        {
            uint arg = 0x000F0000;

            Assert.That(Helper.NumberOfOnes(arg), Is.EqualTo(4));
        }

        [Test]
        public void If_Arg_Is_MAX_Then_NumberOfOnes_Returns_32()
        {
            uint arg = 0xFFFFFFFF;

            Assert.That(Helper.NumberOfOnes(arg), Is.EqualTo(32));
        }

        [Test]
        public void If_Arg_Is_PLUS_0MAX0_Then_NumberOfOnes_Returns_30()
        {
            uint arg = 0x7FFFFFFE;

            Assert.That(Helper.NumberOfOnes(arg), Is.EqualTo(30));
        }

        


    }
}
