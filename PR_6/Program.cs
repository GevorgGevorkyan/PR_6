using Newtonsoft.Json;
using System.Diagnostics;
using System.Xml.Serialization;

class Program
{
    static List<Model> list = new List<Model>()
    {
        new Model("Оружие1",new("Патроны","Калибр")),
        new Model("Оружие2",new("Патроны","Калибр")),
    };

    static void Main()
    {
        Console.Clear();
        Console.WriteLine("Введите путь до файла:");
        step_2(Console.ReadLine());
    }
    static void step_2(string path)
    {
        Console.Clear();
        if (path.Contains(".json"))
            List_to_text(Class.json_read<List<Model>>(list, path));
        else if (path.Contains(".xml"))
            List_to_text(Class.xml_read<List<Model>>(list, path));
        else if (path.Contains(".txt"))
            List_to_text(Class.txt_read(list, path));
        Console.WriteLine("Конвертировать в:\n1. - в txt, 2. - в json, 3. - в xml");
        switch (Console.ReadLine())
        {
            case "1":
                Class.txt_write(list, "save.txt"); break;
            case "2":
                Class.json_write(list, "save.json"); break;
            case "3":
                Class.xml_write(list, "save.xml"); break;
            default:
                Console.WriteLine("Чет пошло не так");
                Thread.Sleep(1000);
                Main();
                break;
        }
        Main();
    }
    static void List_to_text(List<Model> list)
    {
        foreach (var a in list)
            Console.WriteLine($"{a.arg1}\n  {a.Podmodel.attr1}\n  {a.Podmodel.attr2}\n");
    }
}
class Class
{
    public static T json_read<T>(T model, string path)
    {
        if (!File.Exists(path))
            File.WriteAllText(path, JsonConvert.SerializeObject(model));
        return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
    }
    public static void json_write<T>(T model, string path) => File.WriteAllText(path, JsonConvert.SerializeObject(model));
    public static List<Model> xml_read<T>(T model, string path)
    {
        List<Model> obj;
        XmlSerializer xml = new XmlSerializer(typeof(List<Model>));
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "");
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                xml.Serialize(fs, model);
        }
        using (FileStream fs = new FileStream(path, FileMode.Open))
            obj = (List<Model>)xml.Deserialize(fs);
        return obj;
    }
    public static void xml_write<T>(T model, string path)
    {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            xml.Serialize(fs, model);
    }
    public static List<Model> txt_read(List<Model> model, string path)
    {
        if (!File.Exists(path))
        {
            string result = "";
            foreach (var a in model)
                result += $"{a.arg1}\n  {a.Podmodel.attr1}\n  {a.Podmodel.attr2}\n";
            File.WriteAllText(path, result);
        }
        List<string> strings = new List<string>();
        foreach (var a in File.ReadAllLines(path))
            strings.Add(a);
        List<Model> res = new List<Model>();
        for (int i = 0; i < strings.Count; i += 3)
            res.Add(new Model(strings[i], new Podmodel(strings[i + 1], strings[i + 2])));
        return res;
    }
    public static void txt_write(List<Model> model, string path)
    {
        string result = "";
        foreach (var a in model)
            result += $"{a.arg1}\n  {a.Podmodel.attr1}\n  {a.Podmodel.attr2}\n";
        File.WriteAllText(path, result);
    }
}

public class Model
{
    public string arg1 { get; set; }
    public Podmodel Podmodel { get; set; }
    public Model() { }

    public Model(string arg1, Podmodel podmodel)
    {
        this.arg1 = arg1;
        Podmodel = podmodel;
    }
}
public class Podmodel
{
    public string attr1 { get; set; }
    public string attr2 { get; set; }
    public Podmodel() { }

    public Podmodel(string attr1, string attr2)
    {
        this.attr1 = attr1;
        this.attr2 = attr2;
    }
}