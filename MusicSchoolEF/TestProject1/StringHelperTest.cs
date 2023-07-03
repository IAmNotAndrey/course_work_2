using MusicSchoolAsp.Helpers;

namespace TestProject1
{
    public class StringHelperTest
    {
        [Fact]
        public void MyTrim1()
        {
            string a = "abc";
            string b = "abc";
            Assert.Equal(a.MyTrim(), b);
        }

        [Fact]
        public void MyTrim2()
        {
            string a = "abc ";
            string b = "abc";
            Assert.Equal(a.MyTrim(), b);
        }

        [Fact]
        public void MyTrim3()
        {
            string a = " abc";
            string b = "abc";
            Assert.Equal(a.MyTrim(), b);
        }

        [Fact]
        public void MyTrim4()
        {
            string a = "  abc  ";
            string b = "abc";
            Assert.Equal(a.MyTrim(), b);
        }

        [Fact]
        public void MyTrim5()
        {
            string a = "a bc";
            string b = "a bc";
            Assert.Equal(a.MyTrim(), b);
        }

        [Fact]
        public void MyTrim6()
        {
            string a = "ab c";
            string b = "ab c";
            Assert.Equal(a.MyTrim(), b);
        }

        [Fact]
        public void MyTrim7()
        {
            string a = "ab  c";
            string b = "ab c";
            Assert.Equal(a.MyTrim(), b);
        }

        [Fact]
        public void MyTrim8()
        {
            string a = "a  bc";
            string b = "a bc";
            Assert.Equal(a.MyTrim(), b);
        }
    }
}
