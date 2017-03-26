namespace ks.model.Entity.Kudu
{
    public class Binding
    {
        public string authLevel { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string direction { get; set; }
        public string path { get; set; }
        public string connection { get; set; }
        public string topicName { get; set; }
        public string subscriptionName { get; set; }
        public string accessRights { get; set; }
    }
}
