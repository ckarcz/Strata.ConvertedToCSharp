using System;
using System.Threading;
using System.IO;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.google.common.@base.MoreObjects.firstNonNull;


	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ByteSource = com.google.common.io.ByteSource;
	using CharSource = com.google.common.io.CharSource;
	using Files = com.google.common.io.Files;
	using Resources = com.google.common.io.Resources;

	/// <summary>
	/// A locator for a resource, specified as a file, URL, path or classpath resource.
	/// <para>
	/// An instance of this class provides access to a resource, such as a configuration file.
	/// The resource data is accessed using <seealso cref="CharSource"/> or <seealso cref="ByteSource"/>.
	/// </para>
	/// </summary>
	public sealed class ResourceLocator
	{

	  /// <summary>
	  /// The prefix for classpath resource locators.
	  /// </summary>
	  public const string CLASSPATH_URL_PREFIX = "classpath:";
	  /// <summary>
	  /// The prefix for file resource locators.
	  /// </summary>
	  public const string FILE_URL_PREFIX = "file:";
	  /// <summary>
	  /// The prefix for URL resource locators.
	  /// </summary>
	  public const string URL_PREFIX = "url:";

	  /// <summary>
	  /// The resource locator.
	  /// </summary>
	  private readonly string locator;
	  /// <summary>
	  /// The source.
	  /// </summary>
	  private readonly ByteSource source;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a resource from a string locator.
	  /// <para>
	  /// This accepts locators starting with 'classpath:', 'url:' or 'file:'.
	  /// It also accepts unprefixed locators, treated as files.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="locator">  the string form of the resource locator </param>
	  /// <returns> the resource locator </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static ResourceLocator of(String locator)
	  public static ResourceLocator of(string locator)
	  {
		ArgChecker.notNull(locator, "locator");
		try
		{
		  if (locator.StartsWith(CLASSPATH_URL_PREFIX, StringComparison.Ordinal))
		  {
			string urlStr = locator.Substring(CLASSPATH_URL_PREFIX.Length);
			return ofClasspath(urlStr);

		  }
		  else if (locator.StartsWith(FILE_URL_PREFIX, StringComparison.Ordinal))
		  {
			string fileStr = locator.Substring(FILE_URL_PREFIX.Length);
			return ofFile(new File(fileStr));

		  }
		  else if (locator.StartsWith(URL_PREFIX, StringComparison.Ordinal))
		  {
			string pathStr = locator.Substring(URL_PREFIX.Length);
			return ofUrl(new URL(pathStr));

		  }
		  else
		  {
			return ofFile(new File(locator));
		  }
		}
		catch (Exception ex)
		{
		  throw new System.ArgumentException("Invalid resource locator: " + locator, ex);
		}
	  }

	  /// <summary>
	  /// Creates a resource from a {@code File}.
	  /// <para>
	  /// Windows separators are converted to UNIX style.
	  /// This takes no account of possible '/' characters in the name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="file">  the file to wrap </param>
	  /// <returns> the resource locator </returns>
	  public static ResourceLocator ofFile(File file)
	  {
		ArgChecker.notNull(file, "file");
		string filename = file.ToString();
		// convert Windows separators to unix
		filename = (Path.DirectorySeparatorChar == '\\' ? filename.Replace('\\', '/') : filename);
		return new ResourceLocator(FILE_URL_PREFIX + filename, Files.asByteSource(file));
	  }

	  /// <summary>
	  /// Creates a resource from a {@code Path}.
	  /// <para>
	  /// This will return either a file locator or a URL locator.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="path">  path to the file to wrap </param>
	  /// <returns> the resource locator </returns>
	  /// <exception cref="IllegalArgumentException"> if the path is neither a file nor a URL </exception>
	  public static ResourceLocator ofPath(Path path)
	  {
		ArgChecker.notNull(path, "path");
		try
		{
		  return ofFile(path.toFile());
		}
		catch (System.NotSupportedException)
		{
		  try
		  {
			return ofUrl(path.toUri().toURL());
		  }
		  catch (MalformedURLException)
		  {
			throw new System.ArgumentException("Path could not be converted to a File or URL: " + path);
		  }
		}
	  }

	  /// <summary>
	  /// Creates a resource from a {@code URL}.
	  /// </summary>
	  /// <param name="url">  path to the file to wrap </param>
	  /// <returns> the resource locator </returns>
	  public static ResourceLocator ofUrl(URL url)
	  {
		ArgChecker.notNull(url, "url");
		string filename = url.ToString();
		return new ResourceLocator(URL_PREFIX + filename, Resources.asByteSource(url));
	  }

	  /// <summary>
	  /// Creates a resource from a fully qualified resource name.
	  /// <para>
	  /// If the resource name does not start with a slash '/', one will be prepended.
	  /// Use <seealso cref="#ofClasspath(Class, String)"/> to get a relative resource.
	  /// </para>
	  /// <para>
	  /// In Java 9 and later, resources can be encapsulated due to the module system.
	  /// As such, this method is caller sensitive.
	  /// It finds the class of the method that called this one, and uses that to search for
	  /// resources using <seealso cref="Class#getResource(String)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resourceName">  the resource name, which will have a slash '/' prepended if missing </param>
	  /// <returns> the resource locator </returns>
	  public static ResourceLocator ofClasspath(string resourceName)
	  {
		ArgChecker.notNull(resourceName, "classpathLocator");
		string searchName = resourceName.StartsWith("/", StringComparison.Ordinal) ? resourceName : "/" + resourceName;
		Type caller = Guavate.callerClass(3);
		return ofClasspath(caller, searchName);
	  }

	  /// <summary>
	  /// Creates a resource locator for a classpath resource which is associated with a class.
	  /// <para>
	  /// The classpath is searched using the same method as {@code Class.getResource}.
	  /// <ul>
	  ///   <li>If the resource name starts with '/' it is treated as an absolute path relative to the classpath root</li>
	  ///   <li>Otherwise the resource name is treated as a path relative to the package containing the class</li>
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cls">  the class used to find the resource </param>
	  /// <param name="resourceName">  the resource name </param>
	  /// <returns> the resource locator </returns>
	  public static ResourceLocator ofClasspath(Type cls, string resourceName)
	  {
		ArgChecker.notNull(resourceName, "classpathLocator");
		URL url = cls.getResource(resourceName);
		if (url == null)
		{
		  throw new System.ArgumentException("Resource not found: " + resourceName);
		}
		return ofClasspathUrl(url);
	  }

	  /// <summary>
	  /// Creates a resource from a {@code URL}.
	  /// </summary>
	  /// <param name="url">  the URL to wrap </param>
	  /// <returns> the resource locator </returns>
	  public static ResourceLocator ofClasspathUrl(URL url)
	  {
		ArgChecker.notNull(url, "url");
		string locator = CLASSPATH_URL_PREFIX + url.ToString();
		return new ResourceLocator(locator, Resources.asByteSource(url));
	  }

	  /// <summary>
	  /// Selects a suitable class loader.
	  /// </summary>
	  /// <returns> the class loader </returns>
	  internal static ClassLoader classLoader()
	  {
		return firstNonNull(Thread.CurrentThread.ContextClassLoader, typeof(ResourceConfig).ClassLoader);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance of the locator.
	  /// </summary>
	  /// <param name="locator">  the locator </param>
	  /// <param name="source">  the byte source </param>
	  private ResourceLocator(string locator, ByteSource source) : base()
	  {
		this.locator = locator;
		this.source = source;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the string form of the locator.
	  /// <para>
	  /// The string form of the locator describes the location of the resource.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the locator string </returns>
	  public string Locator
	  {
		  get
		  {
			return locator;
		  }
	  }

	  /// <summary>
	  /// Gets the byte source to access the resource.
	  /// <para>
	  /// A byte source is a supplier of data.
	  /// The source itself is neither opened nor closed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the byte source </returns>
	  public ByteSource ByteSource
	  {
		  get
		  {
			return source;
		  }
	  }

	  /// <summary>
	  /// Gets the char source to access the resource using UTF-8.
	  /// <para>
	  /// A char source is a supplier of data.
	  /// The source itself is neither opened nor closed.
	  /// </para>
	  /// <para>
	  /// This method handles Unicode Byte Order Marks.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the char source </returns>
	  public CharSource CharSource
	  {
		  get
		  {
			return UnicodeBom.toCharSource(source);
		  }
	  }

	  /// <summary>
	  /// Gets the char source to access the resource specifying the character set.
	  /// <para>
	  /// A char source is a supplier of data.
	  /// The source itself is neither opened nor closed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="charset">  the character set to use </param>
	  /// <returns> the char source </returns>
	  public CharSource getCharSource(Charset charset)
	  {
		return source.asCharSource(charset);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this locator equals another locator.
	  /// <para>
	  /// The comparison checks the locator string.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the other locator, null returns false </param>
	  /// <returns> true if equal </returns>
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is ResourceLocator)
		{
		  return locator.Equals(((ResourceLocator) obj).locator);
		}
		return false;
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the locator.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return locator.GetHashCode();
	  }

	  /// <summary>
	  /// Returns a string describing the locator.
	  /// <para>
	  /// This can be parsed using <seealso cref="#of(String)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the descriptive string </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ToString @Override public String toString()
	  public override string ToString()
	  {
		return locator;
	  }

	}

}