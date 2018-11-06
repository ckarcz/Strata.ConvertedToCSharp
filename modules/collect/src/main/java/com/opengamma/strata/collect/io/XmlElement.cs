using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.ensureOnlyOne;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using ImmutableBean = org.joda.beans.ImmutableBean;
	using MetaBean = org.joda.beans.MetaBean;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;

	/// <summary>
	/// A single element in the tree structure of XML.
	/// <para>
	/// This class is a minimal, lightweight representation of an element in the XML tree.
	/// The element has a name, attributes, and either content or children.
	/// </para>
	/// <para>
	/// Note that this representation does not express all XML features.
	/// No support is provided for processing instructions, comments or mixed content.
	/// In addition, it is not possible to determine the difference between empty content and no children.
	/// </para>
	/// <para>
	/// There is no explicit support for namespaces.
	/// When creating instances, the caller may choose to use a convention to represent namespaces.
	/// For example, element and attribute names may use prefixes or the standard <seealso cref="QName"/> format.
	/// </para>
	/// </summary>
	public sealed class XmlElement : ImmutableBean
	{

	  /// <summary>
	  /// The meta-bean.
	  /// This is a manually coded bean.
	  /// </summary>
	  private static MetaBean META_BEAN = LightMetaBean.of(typeof(XmlElement), MethodHandles.lookup(), new string[] {"name", "attributes", "content", "children"}, null, ImmutableMap.of(), null, ImmutableList.of());

	  /// <summary>
	  /// The element name.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final String name;
	  private readonly string name;
	  /// <summary>
	  /// The attributes.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<String, String> attributes;
	  private readonly ImmutableMap<string, string> attributes;
	  /// <summary>
	  /// The element content.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final String content;
	  private readonly string content;
	  /// <summary>
	  /// The child nodes.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<XmlElement> children;
	  private readonly ImmutableList<XmlElement> children;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance with content and no attributes.
	  /// <para>
	  /// Returns an element representing XML with content, but no children.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the element name, not empty </param>
	  /// <param name="content">  the content, empty if the element has no content </param>
	  /// <returns> the element </returns>
	  public static XmlElement ofContent(string name, string content)
	  {
		return ofContent(name, ImmutableMap.of(), content);
	  }

	  /// <summary>
	  /// Obtains an instance with content and attributes.
	  /// <para>
	  /// Returns an element representing XML with content and attributes but no children.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the element name, not empty </param>
	  /// <param name="attributes">  the attributes, empty if the element has no attributes </param>
	  /// <param name="content">  the content, empty if the element has no content </param>
	  /// <returns> the element </returns>
	  public static XmlElement ofContent(string name, IDictionary<string, string> attributes, string content)
	  {
		return new XmlElement(name, ImmutableMap.copyOf(attributes), content, ImmutableList.of());
	  }

	  /// <summary>
	  /// Obtains an instance with children and no attributes.
	  /// <para>
	  /// Returns an element representing XML with children, but no content.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the element name, not empty </param>
	  /// <param name="children">  the children, empty if the element has no children </param>
	  /// <returns> the element </returns>
	  public static XmlElement ofChildren(string name, IList<XmlElement> children)
	  {
		return ofChildren(name, ImmutableMap.of(), children);
	  }

	  /// <summary>
	  /// Obtains an instance with children and attributes.
	  /// <para>
	  /// Returns an element representing XML with children and attributes, but no content.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the element name, not empty </param>
	  /// <param name="attributes">  the attributes, empty if the element has no attributes </param>
	  /// <param name="children">  the children, empty if the element has no children </param>
	  /// <returns> the element </returns>
	  public static XmlElement ofChildren(string name, IDictionary<string, string> attributes, IList<XmlElement> children)
	  {
		return new XmlElement(name, ImmutableMap.copyOf(attributes), "", ImmutableList.copyOf(children));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the element name, not empty </param>
	  /// <param name="attributes">  the attributes, empty if the element has no attributes </param>
	  /// <param name="content">  the content, empty if the element has no content </param>
	  /// <param name="children">  the children, empty if the element has no children </param>
	  private XmlElement(string name, ImmutableMap<string, string> attributes, string content, ImmutableList<XmlElement> children)
	  {

		this.name = ArgChecker.notEmpty(name, "name");
		this.attributes = ArgChecker.notNull(attributes, "attributes");
		this.content = ArgChecker.notNull(content, "content");
		this.children = ArgChecker.notNull(children, "children");
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the element name.
	  /// </summary>
	  /// <returns> the name </returns>
	  public string Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  /// <summary>
	  /// Gets an attribute by name, throwing an exception if not found.
	  /// <para>
	  /// This returns the value of the attribute with the specified name.
	  /// An exception is thrown if the attribute does not exist.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="attrName">  the attribute name to find </param>
	  /// <returns> the attribute value </returns>
	  /// <exception cref="IllegalArgumentException"> if the attribute name does not exist </exception>
	  public string getAttribute(string attrName)
	  {
		string attrValue = attributes.get(attrName);
		if (string.ReferenceEquals(attrValue, null))
		{
		  throw new System.ArgumentException(Messages.format("Unknown attribute '{}' on element '{}'", attrName, name));
		}
		return attrValue;
	  }

	  /// <summary>
	  /// Finds an attribute by name, or empty if not found.
	  /// <para>
	  /// This returns the value of the attribute with the specified name.
	  /// If the attribute is not found, optional empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="attrName">  the attribute name to find </param>
	  /// <returns> the attribute value, optional </returns>
	  public Optional<string> findAttribute(string attrName)
	  {
		return Optional.ofNullable(attributes.get(attrName));
	  }

	  /// <summary>
	  /// Gets the attributes.
	  /// <para>
	  /// This returns all the attributes of this element.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the attributes </returns>
	  public ImmutableMap<string, string> Attributes
	  {
		  get
		  {
			return attributes;
		  }
	  }

	  /// <summary>
	  /// Checks if the element has content.
	  /// <para>
	  /// Content exists if it is non-empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the content </returns>
	  public bool hasContent()
	  {
		return content.Length > 0;
	  }

	  /// <summary>
	  /// Gets the element content.
	  /// <para>
	  /// If this element has no content, the empty string is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the content </returns>
	  public string Content
	  {
		  get
		  {
			return content;
		  }
	  }

	  /// <summary>
	  /// Gets a child element by index.
	  /// </summary>
	  /// <param name="index">  the index to find </param>
	  /// <returns> the child </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public XmlElement getChild(int index)
	  {
		return children.get(index);
	  }

	  /// <summary>
	  /// Gets the child elements.
	  /// <para>
	  /// This returns all the children of this element.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the children </returns>
	  public ImmutableList<XmlElement> Children
	  {
		  get
		  {
			return children;
		  }
	  }

	  /// <summary>
	  /// Gets the child element with the specified name, throwing an exception if not found or more than one.
	  /// <para>
	  /// This returns the child element with the specified name.
	  /// An exception is thrown if there is more than one matching child or the child does not exist.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="childName">  the name to match </param>
	  /// <returns> the child matching the name </returns>
	  /// <exception cref="IllegalArgumentException"> if there is more than one match or no matches </exception>
	  public XmlElement getChild(string childName)
	  {
		return findChild(childName).orElseThrow(() => new System.ArgumentException(Messages.format("Unknown element '{}' in element '{}'", childName, name)));
	  }

	  /// <summary>
	  /// Finds the child element with the specified name, or empty if not found,
	  /// throwing an exception if more than one.
	  /// <para>
	  /// This returns the child element with the specified name.
	  /// If the element is not found, optional empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="childName">  the name to match </param>
	  /// <returns> the child matching the name, optional </returns>
	  /// <exception cref="IllegalArgumentException"> if there is more than one match </exception>
	  public Optional<XmlElement> findChild(string childName)
	  {
		return streamChildren(childName).reduce(ensureOnlyOne());
	  }

	  /// <summary>
	  /// Gets the child elements matching the specified name.
	  /// <para>
	  /// This returns all the child elements with the specified name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="childName">  the name to match </param>
	  /// <returns> the children matching the name </returns>
	  public ImmutableList<XmlElement> getChildren(string childName)
	  {
		return streamChildren(childName).collect(toImmutableList());
	  }

	  /// <summary>
	  /// Gets the child elements matching the specified name.
	  /// <para>
	  /// This returns all the child elements with the specified name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="childName">  the name to match </param>
	  /// <returns> the children matching the name </returns>
	  public Stream<XmlElement> streamChildren(string childName)
	  {
		return children.Where(child => child.Name.Equals(childName));
	  }

	  //-------------------------------------------------------------------------
	  public override MetaBean metaBean()
	  {
		return META_BEAN;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this element equals another.
	  /// <para>
	  /// This compares the entire state of the element, including all children.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the other element, null returns false </param>
	  /// <returns> true if equal </returns>
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is XmlElement)
		{
		  XmlElement other = (XmlElement) obj;
		  return name.Equals(other.name) && Objects.Equals(content, other.content) && attributes.Equals(other.attributes) && children.Equals(other.children);
		}
		return false;
	  }

	  /// <summary>
	  /// Returns a suitable hash code.
	  /// <para>
	  /// This includes the entire state of the element, including all children.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		const int prime = 31;
		int result = 1;
		result = prime * result + name.GetHashCode();
		result = prime * result + content.GetHashCode();
		result = prime * result + attributes.GetHashCode();
		result = prime * result + children.GetHashCode();
		return result;
	  }

	  /// <summary>
	  /// Returns a string summary of the element.
	  /// <para>
	  /// The string form includes the attributes and content, but summarizes the child elements.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the string form </returns>
	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(512);
		buf.Append('<').Append(name);
		foreach (KeyValuePair<string, string> entry in attributes.entrySet())
		{
		  buf.Append(' ').Append(entry.Key).Append('=').Append('"').Append(entry.Value).Append('"');
		}
		buf.Append('>');
		if (children.Empty)
		{
		  buf.Append(content);
		}
		else
		{
		  foreach (XmlElement child in children)
		  {
			buf.Append(Environment.NewLine).Append(" <").Append(child.Name).Append(" ... />");
		  }
		  buf.Append(Environment.NewLine);
		}
		buf.Append("</").Append(name).Append('>');
		return buf.ToString();
	  }

	}

}