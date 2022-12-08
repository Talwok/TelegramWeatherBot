using System.IO;
using System.Xml.Serialization;
public static class ParametersSerializer
{
    private static string filename = "Parameters.xml";
    public static void Serialize(this Parameters parameters)
    {
        XmlSerializer xmlSerializerserializer = new XmlSerializer(typeof(Parameters));
        using(TextWriter writer = new StreamWriter(filename))
        {
            xmlSerializerserializer.Serialize(writer, parameters);
        }        
    }
    public static Parameters Deserialize()
    {
        Parameters parameters = new Parameters();
        if(!File.Exists(filename))
        {
            parameters.Serialize();
            return parameters;
        }
        XmlSerializer xmlSerializerserializer = new XmlSerializer(typeof(Parameters));
        using (TextReader reader = new StreamReader(filename))
        {
            parameters = xmlSerializerserializer.Deserialize(reader) as Parameters;
        }
        return parameters;
    }
}