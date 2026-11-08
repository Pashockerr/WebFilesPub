using WebFiles.Domain.Entities;

namespace WebFiles.Test
{
    public class Tests
    {
        private Note _note;

        [SetUp]
        public void Setup()
        {
            try
            {
                _note = new Note("Test", "Test", "Test", true, "");
            }
            catch(Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public void NoteChangeNameTest()
        {
            var name = "TestName";
            _note.SetName(name);

            if (_note.Name.Equals(name))
                Assert.Pass();
            else
                Assert.Fail("Имя не соответствует ожидаемому");
        }
    }
}