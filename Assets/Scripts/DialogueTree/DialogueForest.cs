using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
// using ComponentBind;

public class DialogueForest
{
    public interface IClient
    {
        // Visit a node
        void Visit(Node node);
        // Set a text node
        void Text(Node node, int level);
        // Save choices variables
        void Choice(Node node, IEnumerable<Node> choices);
        // Set a variable
        void Set(string key, string value);
        // Get a branch variable
        string Get(string key);
    }

    /// <summary>
    /// A node object represented by this class
    /// 
    /// id: uuid value unique to each node
    /// 
    /// name: text of the node if type is of text
    /// 
    /// actor: the speaker of the text
    /// 
    /// next: the following node in a sequence, not if type is text, use choices instead
    /// 
    /// choices: a list of choices following this node
    /// 
    /// branches: a list if options the variable can be, returns id of the choosen node
    /// </summary>
    public class Node
    {
        public string next;
        public List<string> choices;
        public Dictionary<string, string> branches;
        public string id;
        public string title;
        public string name;
        public string actor;
        public string variable;
        public string value;
        public string type;
        public bool first = false;
    }

    private Dictionary<string, Node> nodes = new Dictionary<string, Node>();
    private Dictionary<string, Node> nodesByName = new Dictionary<string, Node>();

    public IClient db;

    public IEnumerable<Node> Load(string data)
    {
        List<Node> nodes = JsonMapper.ToObject<List<Node>>(data);

        foreach (Node node in nodes)
        {
            if (node.actor != null && node.actor.Contains("!"))
            {
                node.actor = node.actor.Replace("!", string.Empty);
                node.first = true;
            }
            this.nodes[node.id] = node;
            if (node.name != null)
                this.nodesByName[node.name] = node;
        }
        return nodes;
    }

    public Node GetFirstNode()
    {
        Node defaultFirst = null;
        foreach (KeyValuePair<string, Node> node in nodes)
        {
            if (defaultFirst == null)
            {
                defaultFirst = node.Value;
            }
            if (node.Value.first)
            {
                return node.Value;
            }
        }
        return defaultFirst;
    }

    public Node this[string id]
    {
        get
        {
            if (id == null)
            {
                return null;
            }
            Node result;
            if(this.nodes.TryGetValue(id, out result))
                return result;
            return null;
        }
    }

    public Node GetByName(string name)
    {
        Node result;
        this.nodesByName.TryGetValue(name, out result);
        return result;
    }

    public IEnumerable<Node> Nodes
    {
        get
        {
            return this.nodes.Values;
        }
    }

    public void Execute(Node node, IClient client, int textLevel = 1)
    {
        if (db == null)
        {
            db = client;
        }
        client.Visit(node);
        string next = null;
        switch (node.type)
        {
            case "Node":
                if (node.choices != null && node.choices.Count > 0)
                    client.Choice(node, node.choices.Select(x => this[x]));
                next = node.next;
                break;
            case "Text":
                client.Text(node, textLevel);
                if (node.choices != null && node.choices.Count > 0)
                    client.Choice(node, node.choices.Select(x => this[x]));
                next = node.next;
                textLevel++;
                break;
            case "Set":
                client.Set(node.variable, node.value);
                next = node.next;
                break;
            case "Branch":
                string key = client.Get(node.variable);
                if (key == null || !node.branches.TryGetValue(key, out next))
                    node.branches.TryGetValue("_default", out next);
                break;
            default:
                break;
        }
        if (next != null)
            this.Execute(this[next], client, textLevel);
    }
}