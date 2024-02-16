using Newtonsoft.Json;

namespace TreeViewDemo.Models
{
    public class TreeNode(string key, string value, string parent)
    {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; } = key;

        [JsonProperty(PropertyName = "value")]
        public string Value { get; } = value;

        [JsonIgnore]
        public List<TreeNode> Children { get; } = [];

        [JsonProperty(PropertyName = "parent")]
        public string Parent { get; set; }

        public override string ToString()
        {
            return $"{{value : \"{Value}\", parent : {(Children.Count != 0 ? "" : "null")}}}";
        }
    }
}
