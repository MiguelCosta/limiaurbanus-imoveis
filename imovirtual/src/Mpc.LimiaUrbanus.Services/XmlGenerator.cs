namespace Mpc.LimiaUrbanus.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Mpc.LimiaUrbanus.DataBase.Models;

    public class XmlGenerator : IXmlGenerator
    {
        private readonly IConfigurationRoot _configuration;
        private readonly ILogger<XmlGenerator> _logger;

        public XmlGenerator(
            ILoggerFactory loggerFactory,
            IConfigurationRoot configuration)
        {
            _logger = loggerFactory.CreateLogger<XmlGenerator>();
            _configuration = configuration;
        }

        public string Generate(IEnumerable<Imovel> imoveis)
        {
            var xml = new StringBuilder();
            xml.AppendLine("<data>");

            foreach (var imovel in imoveis)
            {
                xml.AppendLine("<advert>");
                xml.AppendLine($"<external_id>{imovel.ImovelId}</external_id>");
                xml.AppendLine($"<email>{_configuration["XmlSettings:email"]}</email>");
                xml.AppendLine($"<district>{imovel.Freguesia.Concelho.Distrito.Nome}</district>");
                xml.AppendLine($"<state>{imovel.Freguesia.Concelho.Nome}</state>");
                xml.AppendLine($"<parish>{imovel.Freguesia.Nome}</parish>");
                xml.AppendLine($"<offer_type>{imovel.Objetivo.Nome}</offer_type>");
                xml.AppendLine($"<category>{imovel.Tipo.Nome}</category>");
                xml.AppendLine($"<title>{imovel.Nome}</title>");
                xml.AppendLine("<images>");
                foreach (var file in imovel.FilePath)
                {
                    xml.AppendLine($"<image>{string.Format(_configuration["XmlSettings:url_format_image"], file.FileName)}</image>");
                }
                xml.AppendLine("</images>");
                xml.AppendLine($"<price>{imovel.Preco}</price>");
                xml.AppendLine($"<area>{imovel.Area}</area>");
                xml.AppendLine($"<size>{imovel.Tipologia?.Nome}</size>");
                xml.AppendLine($"<consultant_email>{_configuration["XmlSettings:consultant_email"]}</consultant_email>");
                xml.AppendLine($"<phone>{_configuration["XmlSettings:phone"]}</phone>");
                xml.AppendLine($"<description><![CDATA[{imovel.Descricao}]]></description>");
                xml.AppendLine($"<reference_id>{imovel.Referencia}</reference_id>");
                xml.AppendLine("<attributes>");
                xml.AppendLine($"<attribute><name>Certificado Energético</name><value>{imovel.ClasseEnergetica?.Nome}</value></attribute>");
                xml.AppendLine($"<attribute><name>Condição</name><value>{imovel.Tipo?.Nome}</value></attribute>");
                xml.AppendLine($"<attribute><name>Casas de Banho</name><value>{imovel.Wc}</value></attribute>");
                xml.AppendLine("</attributes>");
                xml.AppendLine("</advert>");
            }

            xml.AppendLine("</data>");
            var xmlText = xml.ToString();
            return PrettyXml(xmlText);
        }

        public string PrettyXml(string xml)
        {
            String result = string.Empty;

            var memoryStream = new MemoryStream();
            var xmlWriter = new XmlTextWriter(memoryStream, Encoding.Unicode);
            var xmlDocument = new XmlDocument();

            try
            {
                // Load the XmlDocument with the XML.
                xmlDocument.LoadXml(xml);

                xmlWriter.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                xmlDocument.WriteContentTo(xmlWriter);
                xmlWriter.Flush();
                memoryStream.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                memoryStream.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader sReader = new StreamReader(memoryStream);

                // Extract the text from the StreamReader.
                var formattedXML = sReader.ReadToEnd();

                result = formattedXML;
            }
            catch (XmlException ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            memoryStream.Close();
            xmlWriter.Close();

            return result;
        }
    }
}
