using System.Collections.Generic;
using System.IO;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ByteSource = com.google.common.io.ByteSource;

	/// <summary>
	/// An XML file.
	/// <para>
	/// Represents an XML file together with the ability to parse it from a <seealso cref="ByteSource"/>.
	/// </para>
	/// <para>
	/// This uses the standard StAX API to parse the file.
	/// Once parsed, the XML is represented as a DOM-like structure, see <seealso cref="XmlElement"/>.
	/// This approach is suitable for XML files where the size of the parsed XML file is
	/// known to be manageable in memory.
	/// </para>
	/// <para>
	/// Note that the <seealso cref="XmlElement"/> representation does not express all XML features.
	/// No support is provided for processing instructions, comments or mixed content.
	/// In addition, it is not possible to determine the difference between empty content and no children.
	/// </para>
	/// <para>
	/// There is no support for namespaces.
	/// All namespace prefixes are dropped.
	/// There are cases where this can be a problem, but most of the time lenient parsing is helpful.
	/// </para>
	/// </summary>
	public sealed class XmlFile
	{

	  /// <summary>
	  /// The root element.
	  /// </summary>
	  private readonly XmlElement root;
	  /// <summary>
	  /// The map of references.
	  /// </summary>
	  private readonly ImmutableMap<string, XmlElement> refs;

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Parses the specified source as an XML file to an in-memory DOM-like structure.
	  /// <para>
	  /// This parses the specified byte source expecting an XML file format.
	  /// The resulting instance can be queried for the root element.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="source">  the XML source data </param>
	  /// <returns> the parsed file </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  public static XmlFile of(ByteSource source)
	  {
		return of(source, "");
	  }

	  /// <summary>
	  /// Parses the specified source as an XML file to an in-memory DOM-like structure.
	  /// <para>
	  /// This parses the specified byte source expecting an XML file format.
	  /// The resulting instance can be queried for the root element.
	  /// </para>
	  /// <para>
	  /// This supports capturing attribute references, such as an id/href pair.
	  /// Wherever the parser finds an attribute with the specified name, the element is added
	  /// to the internal map, accessible by calling <seealso cref="#getReferences()"/>.
	  /// </para>
	  /// <para>
	  /// For example, if one part of the XML has {@code <foo id="fooId">}, the references map will
	  /// contain an entry mapping "fooId" to the parsed element {@code <foo>}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="source">  the XML source data </param>
	  /// <param name="refAttrName">  the attribute name that should be parsed as a reference </param>
	  /// <returns> the parsed file </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="IllegalArgumentException"> if the file cannot be parsed </exception>
	  public static XmlFile of(ByteSource source, string refAttrName)
	  {
		ArgChecker.notNull(source, "source");
		return Unchecked.wrap(() =>
		{
		using (Stream @in = source.openBufferedStream())
		{
			XMLStreamReader xmlReader = xmlInputFactory().createXMLStreamReader(@in);
			try
			{
				Dictionary<string, XmlElement> refs = new Dictionary<string, XmlElement>();
				XmlElement root = parse(xmlReader, refAttrName, refs);
				return new XmlFile(root, refs);
			}
			finally
			{
				xmlReader.close();
			}
		}
		});
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses the tree from the StAX stream reader, capturing references.
	  /// <para>
	  /// The reader should be created using the factory returned from <seealso cref="#xmlInputFactory()"/>.
	  /// </para>
	  /// <para>
	  /// This method supports capturing attribute references, such as an id/href pair.
	  /// Wherever the parser finds an attribute with the specified name, the element is added
	  /// to the specified map. Note that the map is mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="reader">  the StAX stream reader, positioned at or before the element to be parsed </param>
	  /// <param name="refAttr">  the attribute name that should be parsed as a reference, null if not applicable </param>
	  /// <param name="refs">  the mutable map of references to update, null if not applicable </param>
	  /// <returns> the parsed element </returns>
	  /// <exception cref="IllegalArgumentException"> if the input cannot be parsed </exception>
	  private static XmlElement parse(XMLStreamReader reader, string refAttr, IDictionary<string, XmlElement> refs)
	  {
		try
		{
		  // parse start element
		  string elementName = parseElementName(reader);
		  ImmutableMap<string, string> attrs = parseAttributes(reader);

		  // parse children or content
		  ImmutableList.Builder<XmlElement> childBuilder = ImmutableList.builder();
		  string content = "";
		  int @event = reader.next();
		  while (@event != XMLStreamConstants.END_ELEMENT)
		  {
			switch (@event)
			{
			  // parse child when start element found
			  case XMLStreamConstants.START_ELEMENT:
				childBuilder.add(parse(reader, refAttr, refs));
				break;
			  // append content when characters found
			  // since XMLStreamReader has IS_COALESCING=true means there should only be one content call
			  case XMLStreamConstants.CHARACTERS:
			  case XMLStreamConstants.CDATA:
				content += reader.Text;
				break;
			  default:
				break;
			}
			@event = reader.next();
		  }
		  ImmutableList<XmlElement> children = childBuilder.build();
		  XmlElement parsed = children.Empty ? XmlElement.ofContent(elementName, attrs, content) : XmlElement.ofChildren(elementName, attrs, children);
		  string @ref = attrs.get(refAttr);
		  if (!string.ReferenceEquals(@ref, null))
		  {
			refs[@ref] = parsed;
		  }
		  return parsed;

		}
		catch (XMLStreamException ex)
		{
		  throw new System.ArgumentException(ex);
		}
	  }

	  // find the start element and parses the name
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private static String parseElementName(javax.xml.stream.XMLStreamReader reader) throws javax.xml.stream.XMLStreamException
	  private static string parseElementName(XMLStreamReader reader)
	  {
		int @event = reader.EventType;
		while (@event != XMLStreamConstants.START_ELEMENT)
		{
		  @event = reader.next();
		}
		return reader.LocalName;
	  }

	  // parses attributes into a map
	  private static ImmutableMap<string, string> parseAttributes(XMLStreamReader reader)
	  {
		ImmutableMap<string, string> attrs;
		int attributeCount = reader.AttributeCount + reader.NamespaceCount;
		if (attributeCount == 0)
		{
		  attrs = ImmutableMap.of();
		}
		else
		{
		  ImmutableMap.Builder<string, string> builder = ImmutableMap.builder();
		  for (int i = 0; i < reader.AttributeCount; i++)
		  {
			builder.put(reader.getAttributeLocalName(i), reader.getAttributeValue(i));
		  }
		  attrs = builder.build();
		}
		return attrs;
	  }

	  //-------------------------------------------------------------------------
	  // creates the XML input factory, recreated each time to avoid JDK-8028111
	  // this also provides some protection against hackers attacking XML
	  private static XMLInputFactory xmlInputFactory()
	  {
		// see https://bugs.openjdk.java.net/browse/JDK-8183519 where JDK deprecated the wrong method
		// to avoid a warning on 9 this code uses newInstance() even though newFactory() is more correct
		// there is no difference in behavior between the two methods
		XMLInputFactory factory = XMLInputFactory.newInstance();
		factory.setProperty(XMLInputFactory.IS_COALESCING, true);
		factory.setProperty(XMLInputFactory.IS_REPLACING_ENTITY_REFERENCES, true);
		factory.setProperty(XMLInputFactory.IS_SUPPORTING_EXTERNAL_ENTITIES, false);
		factory.setProperty(XMLInputFactory.SUPPORT_DTD, false);
		return factory;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private XmlFile(XmlElement root, IDictionary<string, XmlElement> refs)
	  {
		this.root = ArgChecker.notNull(root, "root");
		this.refs = ImmutableMap.copyOf(refs);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the root element of this file.
	  /// </summary>
	  /// <returns> the root element </returns>
	  public XmlElement Root
	  {
		  get
		  {
			return root;
		  }
	  }

	  /// <summary>
	  /// Gets the reference map of id to element.
	  /// <para>
	  /// This is used to decode references, such as an id/href pair.
	  /// </para>
	  /// <para>
	  /// For example, if one part of the XML has {@code <foo id="fooId">}, the map will
	  /// contain an entry mapping "fooId" to the parsed element {@code <foo>}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the map of id to element </returns>
	  public ImmutableMap<string, XmlElement> References
	  {
		  get
		  {
			return refs;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this file equals another.
	  /// <para>
	  /// The comparison checks the content and reference map.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the other section, null returns false </param>
	  /// <returns> true if equal </returns>
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is XmlFile)
		{
		  XmlFile other = (XmlFile) obj;
		  return root.Equals(other.root) && refs.Equals(other.refs);
		}
		return false;
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the file.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return root.GetHashCode() ^ refs.GetHashCode();
	  }

	  /// <summary>
	  /// Returns a string describing the file.
	  /// </summary>
	  /// <returns> the descriptive string </returns>
	  public override string ToString()
	  {
		return root.ToString();
	  }

	}

}