using Core;

namespace ArabicInterpretation.Helpers
{
    public static class AuthorManager
    {
        private static object authorLock = new object();

        private static Author currentAuthor;

        public static void SetCurrentAuthor(Author author)
        {
            lock (authorLock)
            {
                currentAuthor = author;
            }
        }

        public static Author GetCurrentAuthor()
        {
            return currentAuthor;
        }
    }
}
