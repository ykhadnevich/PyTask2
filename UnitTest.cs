using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using Request1;

namespace Test.nUnitTests
{
    public class Tests
    {
        [Test]
        public void GetLastSeenStatus_OnlineUser_ReturnsOnline()
        {
            
            var user = new UserData
            {
                isOnline = true,
            };

            
            string result = Program1.GetLastSeenStatus(user);

            
            Assert.AreEqual("online", result);
        }

        [Test]
        public void GetLastSeenStatus_NullLastSeen_ReturnsNA()
        {
            
            var user = new UserData
            {
                isOnline = false,
                lastSeenDate = null,
            };

            
            string result = Program1.GetLastSeenStatus(user);

            
            Assert.AreEqual("N/A", result);
        }

        [Test]
        public void GetLastSeenStatus_JustNow_ReturnsJustNow()
        {
            
            var user = new UserData
            {
                isOnline = false,
                lastSeenDate = DateTime.UtcNow.ToString("o"), 
            };

            
            string result = Program1.GetLastSeenStatus(user);

            
            Assert.AreEqual("just now", result);
        }

        [Test]
        public void TestCoupleOfMinutesAgo()
        {
            UserData user = new UserData
            {
                nickname = "Nick37",
                isOnline = false,
                lastSeenDate = DateTime.UtcNow.AddMinutes(-30).ToString("o")
            };
            string lastSeenStatus = Program1.GetLastSeenStatus(user);

            Assert.AreEqual(lastSeenStatus, "a couple of minutes ago");
        }

        [Test]
        public void TestLessThanAMinute()
        {
            UserData user = new UserData
            {
                nickname = "Nick37",
                isOnline = false,
                lastSeenDate = DateTime.UtcNow.AddSeconds(-50).ToString("o")
            };
            string lastSeenStatus = Program1.GetLastSeenStatus(user);

            Assert.AreEqual(lastSeenStatus, "less than a minute ago");
        }

        [Test]
        public void TestAnHourAgo()
        {
            UserData user = new UserData
            {
                nickname = "Nick37",
                isOnline = false,
                lastSeenDate = DateTime.UtcNow.AddMinutes(-71).ToString("o")
            };
            string lastSeenStatus = Program1.GetLastSeenStatus(user);

            Assert.AreEqual(lastSeenStatus, "an hour ago");
        }

        [Test]
        public void TestToday()
        {
            UserData user = new UserData
            {
                nickname = "Nick37",
                isOnline = false,
                lastSeenDate = DateTime.UtcNow.AddHours(-20).ToString("o")
            };
            string lastSeenStatus = Program1.GetLastSeenStatus(user);

            Assert.AreEqual(lastSeenStatus, "today");
        }

        [Test]
        public void TestYesterday()
        {
            UserData user = new UserData
            {
                nickname = "Nick37",
                isOnline = false,
                lastSeenDate = DateTime.UtcNow.AddHours(-26).ToString("o")
            };
            string lastSeenStatus = Program1.GetLastSeenStatus(user);

            Assert.AreEqual(lastSeenStatus, "yesterday");
        }

        [Test]
        public void TestThisWeek()
        {
            UserData user = new UserData
            {
                nickname = "Nick37",
                isOnline = false,
                lastSeenDate = DateTime.UtcNow.AddDays(-5).ToString("o")
            };
            string lastSeenStatus = Program1.GetLastSeenStatus(user);

            Assert.AreEqual(lastSeenStatus, "this week");
        }

        [Test]
        public void TestALongTimeAgo()
        {
            UserData user = new UserData
            {
                nickname = "Nick37",
                isOnline = false,
                lastSeenDate = DateTime.UtcNow.AddDays(-9).ToString("o")
            };
            string lastSeenStatus = Program1.GetLastSeenStatus(user);

            Assert.AreEqual(lastSeenStatus, "a long time ago");
        }
    }
}
