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

    public class ImovirtualXmlGenerator : IImovirtualXmlGenerator
    {
        private static readonly Dictionary<int, string> _attributeCertificadoEnergetico = new Dictionary<int, string>
        {
            [1] = "",
            [2] = "A",
            [3] = "A+",
            [4] = "B",
            [5] = "B-",
            [6] = "C",
            [7] = "D",
            [8] = "E",
            [9] = "F",
            [10] = "G",
            [11] = "",
            [12] = "Isento"
        };

        private static readonly Dictionary<int, string> _attributeCondicoes = new Dictionary<int, string>
        {
            [1] = "Em construção",
            [2] = "Novo",
            [3] = "Em construção",
            [4] = "Usado",
            [5] = "",
            [6] = "Renovado"
        };

        private static readonly Dictionary<int, string> _categories = new Dictionary<int, string>
        {
            [1] = "Moradias",
            [2] = "Apartamentos",
            [3] = "Armazéns",
            [4] = "Escritórios",
            [5] = "Lojas",
            [6] = "Moradias",
            [7] = "Prédios",
            [8] = "Quintas e herdades",
            [9] = "Quintas e herdades",
            [10] = "Terrenos",
            [11] = "Quintas e herdades",
            [12] = "Lojas",
            [13] = "Garagens e estacionamento",
            [14] = "Terrenos",
            [15] = "Prédios",
            [16] = "Lojas",
            [17] = "Terrenos",
            [18] = "Quintas e herdades",
            [19] = "Moradias",
            [20] = "Quintas e herdades",
            [21] = "Lojas"
        };

        private static readonly Dictionary<int, string> _offerTypes = new Dictionary<int, string>
        {
            [1] = "arrendamento",
            [2] = "férias",
            [3] = "venda",
            [4] = "venda",
            [5] = "férias"
        };

        private static readonly Dictionary<int, string> _postalCodes = new Dictionary<int, string>
        {
            [3] = "4990-540",
            [11] = "4970-500",
            [12] = "4900-001",
            [13] = "4750-001",
            [15] = "4980-610"
        };

        private static readonly Dictionary<int, string> _sizes = new Dictionary<int, string>
        {
            [1] = "",
            [2] = "T0",
            [3] = "T1",
            [4] = "T1",
            [5] = "T2",
            [6] = "T2",
            [7] = "T3",
            [8] = "T3",
            [9] = "T4",
            [10] = "T4",
            [11] = "T5"
        };

        private readonly IConfigurationRoot _configuration;
        private readonly ILogger<ImovirtualXmlGenerator> _logger;

        public ImovirtualXmlGenerator(
            ILoggerFactory loggerFactory,
            IConfigurationRoot configuration)
        {
            _logger = loggerFactory.CreateLogger<ImovirtualXmlGenerator>();
            _configuration = configuration;
        }

        public string Generate(IEnumerable<Imovel> imoveis)
        {
            var xml = new StringBuilder();
            xml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            xml.AppendLine("<data>");

            foreach (var imovel in imoveis)
            {
                xml.AppendLine("<advert>");
                xml.AppendLine($"<external_id>{imovel.ImovelId}</external_id>");
                xml.AppendLine($"<email>{_configuration["XmlSettings:email"]}</email>");
                xml.AppendLine($"<postal_code>{GetPostalCode(imovel)}</postal_code>");
                xml.AppendLine($"<district>{imovel.Freguesia.Concelho.Distrito.Nome}</district>");
                xml.AppendLine($"<state>{imovel.Freguesia.Concelho.Nome}</state>");
                xml.AppendLine($"<parish>{imovel.Freguesia.Nome}</parish>");
                xml.AppendLine($"<offer_type>{GetOfferType(imovel)}</offer_type>");
                xml.AppendLine($"<category>{GetCategory(imovel)}</category>");
                xml.AppendLine($"<title>{GetTitle(imovel)}</title>");
                xml.AppendLine("<images>");
                foreach (var file in imovel.FilePath)
                {
                    xml.AppendLine($"<image>{string.Format(_configuration["XmlSettings:url_format_image"], file.FileName)}</image>");
                }
                xml.AppendLine("</images>");
                xml.AppendLine($"<price>{imovel.Preco}</price>");
                xml.AppendLine($"<area>{imovel.Area}</area>");
                xml.AppendLine(GetSize(imovel));
                xml.AppendLine($"<consultant_email>{_configuration["XmlSettings:consultant_email"]}</consultant_email>");
                xml.AppendLine($"<phone>{_configuration["XmlSettings:phone"]}</phone>");
                xml.AppendLine($"<description><![CDATA[{imovel.Descricao}]]></description>");
                xml.AppendLine($"<reference_id>{imovel.Referencia}</reference_id>");
                xml.AppendLine("<attributes>");
                xml.AppendLine(GetAttributeCertificadoEnergetico(imovel));
                xml.AppendLine(GetAttributeCondicao(imovel));
                xml.AppendLine(GetAttributeWc(imovel));
                xml.AppendLine("</attributes>");
                xml.AppendLine("</advert>");
            }

            xml.AppendLine(GetUser());
            xml.AppendLine(GetConsultant());
            xml.AppendLine("</data>");
            var xmlText = xml.ToString();
            return PrettyXml(xmlText);
        }

        private string GetAttributeCertificadoEnergetico(Imovel imovel)
        {
            if (imovel.ClasseEnergeticaId.HasValue && !string.IsNullOrWhiteSpace(_attributeCertificadoEnergetico[imovel.ClasseEnergeticaId.Value]))
            {
                return $"<attribute><name>Certificado Energético</name><value>{_attributeCertificadoEnergetico[imovel.ClasseEnergeticaId.Value]}</value></attribute>";
            }

            return string.Empty;
        }

        private string GetAttributeCondicao(Imovel imovel)
        {
            if (string.IsNullOrWhiteSpace(_attributeCondicoes[imovel.EstadoId]))
            {
                return string.Empty;
            }

            return $"<attribute><name>Condição</name><value>{imovel.Estado.Nome}</value></attribute>";
        }

        private string GetAttributeWc(Imovel imovel)
        {
            if (imovel.Wc.HasValue && imovel.Wc.Value > 0)
            {
                return $"<attribute><name>Casas de Banho</name><value>{imovel.Wc.Value}</value></attribute>";
            }

            return string.Empty;
        }

        private string GetCategory(Imovel imovel)
        {
            return _categories[imovel.TipoId];
        }

        private string GetConsultant()
        {
            return @"
                <consultant>
                    <email>tiagopita@limiaurbanus.pt</email>
                    <name>Tiago Pita</name>
                    <phone>967205258</phone>
                    <photo>https://raw.githubusercontent.com/MiguelCosta/limiaurbanus/master/LimiaUrbanus.WebSite/Files/FotografiaTiagoCunha.jpg</photo>
                </consultant>
            ";
        }

        private string GetOfferType(Imovel imovel)
        {
            return _offerTypes[imovel.ObjetivoId];
        }

        private string GetPostalCode(Imovel imovel)
        {
            if (_postalCodes.ContainsKey(imovel.Freguesia.ConcelhoId))
            {
                return _postalCodes[imovel.Freguesia.ConcelhoId];
            }
            return "0000-000";
        }

        private string GetSize(Imovel imovel)
        {
            if (imovel.TipologiaId.HasValue && !string.IsNullOrWhiteSpace(imovel.Tipologia.Nome))
            {
                return $"<size>{_sizes[imovel.TipologiaId.Value]}</size>";
            }

            return string.Empty;
        }

        private string GetTitle(Imovel imovel)
        {
            if (imovel.Nome.Length > 79)
            {
                return imovel.Nome.Substring(0, 80);
            }
            return imovel.Nome;
        }

        private string GetUser()
        {
            return @"
                <user>
                    <email>tiagopita@limiaurbanus.pt</email>
                    <first_name>Tiago</first_name>
                    <last_name>Pita</last_name>
                    <address>Rua Doutor Cassiano Baptista 192</address>
                    <postal_code>4990-643</postal_code>
                    <city>Ponte de Lima</city>
                    <country>Portugal</country>
                    <phone>967205258</phone>
                    <ami>11069</ami>
                    <taxid>513447245</taxid>
                    <company_name>Limia Urbanus</company_name>
                </user>
            ";
        }

        private string PrettyXml(string xml)
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
