using static MusicSchoolEF.Helpers.PasswordHelper;

namespace TestProject1
{
    public class PasswordHelperTest
    {
        [Fact]
        public void GetHashPasswordTest1()
        {
            string a = "root";
            string hash = GetHashPassword(a);
            string b = "4813494d137e1631bba301d5acab6e7bb7aa74ce1185d456565ef51d737677b2";
            Assert.Equal(b, hash);
        }

        [Fact]
        public void GetHashPasswordTest2()
        {
            string a = "";
            string hash = GetHashPassword(a);
            string b = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
            Assert.Equal(b, hash);
        }

        [Fact]
        public void GetHashPasswordTest3()
        {
            string a = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            string hash = GetHashPassword(a);
            string b = "635361c48bb9eab14198e76ea8ab7f1a41685d6ad62aa9146d301d4f17eb0ae0";
            Assert.Equal(b, hash);
        }


        [Fact]
        public void IsPasswordValidTest1()
        {
            string a = "root";
            string hash = GetHashPassword(a);
            Assert.True(IsPasswordValid(a, hash));
        }

        [Fact]
        public void IsPasswordValidTest2()
        {
            string a = "root";
            string hash = GetHashPassword("test");
            Assert.False(IsPasswordValid(a, hash));
        }

        [Fact]
        public void IsPasswordValidTest3()
        {
            string a = "root";
            string hash = "root";
            Assert.False(IsPasswordValid(a, hash));
        }
    }
}