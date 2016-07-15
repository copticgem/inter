using Core;

namespace ArabicInterpretation.Helpers
{
    public static class AuthorManager
    {
        private static object authorLock = new object();

        private static Author currentAuthor;

        public static void Set1CurrentAuthor(Author author)
        {
            lock (authorLock)
            {
                currentAuthor = author;
            }
        }

        public static Author Get1CurrentAuthor()
        {
            return currentAuthor;
        }
    }
}
