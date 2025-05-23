namespace SeleniumTests.Utils
{
    public class SearchBuilder
    {
        private string query;

        public SearchBuilder WithQuery(string query)
        {
            this.query = query;
            return this;
        }

        public string Build() => query;
    }
}
