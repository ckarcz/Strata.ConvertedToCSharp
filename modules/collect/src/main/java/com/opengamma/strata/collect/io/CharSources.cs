/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{

	using Charsets = com.google.common.@base.Charsets;
	using CharSource = com.google.common.io.CharSource;
	using Files = com.google.common.io.Files;
	using MoreFiles = com.google.common.io.MoreFiles;
	using Resources = com.google.common.io.Resources;

	/// <summary>
	/// Helper that allows {@code CharSource} objects to be created.
	/// </summary>
	public sealed class CharSources
	{

	  private CharSources()
	  {
	  }

	  /// <summary>
	  /// Obtains an instance of <seealso cref="CharSource"/> from a file name, specified as a String.
	  /// </summary>
	  /// <param name="fileName">  the file name, as a String </param>
	  /// <returns>  a new instance of <seealso cref="CharSource"/> with UTF-8 for charset. </returns>
	  public static CharSource ofFileName(string fileName)
	  {
		return Files.asCharSource(new File(fileName), Charsets.UTF_8);
	  }

	  /// <summary>
	  /// Obtains an instance of <seealso cref="CharSource"/> from a file name, specified as a String.
	  /// This also takes in a specific character set, as a <seealso cref="Charset"/>.
	  /// </summary>
	  /// <param name="fileName">  the file name, as a String </param>
	  /// <param name="charset">  the charset to build the new CharSource based on </param>
	  /// <returns>  a new instance of <seealso cref="CharSource"/> </returns>
	  public static CharSource ofFileName(string fileName, Charset charset)
	  {
		return Files.asCharSource(new File(fileName), charset);
	  }

	  //---------------------------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance of <seealso cref="CharSource"/> from a file object, specified as a <seealso cref="File"/>.
	  /// </summary>
	  /// <param name="file">  the file object </param>
	  /// <returns>  a new instance of <seealso cref="CharSource"/> with UTF-8 for charset. </returns>
	  public static CharSource ofFile(File file)
	  {
		return Files.asCharSource(file, Charsets.UTF_8);
	  }

	  /// <summary>
	  /// Obtains an instance of <seealso cref="CharSource"/> from a file object, specified as a <seealso cref="File"/>.
	  /// This also takes in a specific character set, as a <seealso cref="Charset"/>.
	  /// </summary>
	  /// <param name="file">  the file object </param>
	  /// <param name="charset">  the charset to build the new CharSource based on </param>
	  /// <returns>  a new instance of <seealso cref="CharSource"/> </returns>
	  public static CharSource ofFile(File file, Charset charset)
	  {
		return Files.asCharSource(file, charset);
	  }

	  //---------------------------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance of <seealso cref="CharSource"/> from a file path, specified as a <seealso cref="Path"/>.
	  /// </summary>
	  /// <param name="path">  the path to create a <seealso cref="CharSource"/> from </param>
	  /// <returns>  a new instance of <seealso cref="CharSource"/> with UTF-8 for charset. </returns>
	  public static CharSource ofPath(Path path)
	  {
		return MoreFiles.asCharSource(path, Charsets.UTF_8);
	  }

	  /// <summary>
	  /// Obtains an instance of <seealso cref="CharSource"/> from a file path, specified as a <seealso cref="Path"/>.
	  /// This also takes in a specific character set, as a <seealso cref="Charset"/>.
	  /// </summary>
	  /// <param name="path">  the path to create a <seealso cref="CharSource"/> from </param>
	  /// <param name="charset">  the charset to build the new CharSource based on </param>
	  /// <returns>  a new instance of <seealso cref="CharSource"/> </returns>
	  public static CharSource ofPath(Path path, Charset charset)
	  {
		return MoreFiles.asCharSource(path, charset);
	  }

	  //---------------------------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance of <seealso cref="CharSource"/> from a URL, specified as a <seealso cref="URL"/> object.
	  /// </summary>
	  /// <param name="url">  the url to create a <seealso cref="CharSource"/> from </param>
	  /// <returns>  a new instance of <seealso cref="CharSource"/> with UTF-8 for charset. </returns>
	  public static CharSource ofUrl(URL url)
	  {
		return Resources.asCharSource(url, Charsets.UTF_8);
	  }

	  /// <summary>
	  /// Obtains an instance of <seealso cref="CharSource"/> from an URL, specified as a <seealso cref="URL"/> object.
	  /// This also takes in a specific character set, as a <seealso cref="Charset"/>.
	  /// </summary>
	  /// <param name="url">  the url to create a <seealso cref="CharSource"/> from </param>
	  /// <param name="charset">  the charset to build the new CharSource based on </param>
	  /// <returns>  a new instance of <seealso cref="CharSource"/>. </returns>
	  public static CharSource ofUrl(URL url, Charset charset)
	  {
		return Resources.asCharSource(url, charset);
	  }

	  //---------------------------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance of <seealso cref="CharSource"/> from a text variable, specified as a <seealso cref="String"/> object.
	  /// </summary>
	  /// <param name="content">  the text to create a <seealso cref="CharSource"/> for </param>
	  /// <returns>  a new instance of <seealso cref="CharSource"/> with UTF-8 for charset </returns>
	  public static CharSource ofContent(string content)
	  {
		return CharSource.wrap(content);
	  }

	  //---------------------------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance of <seealso cref="CharSource"/> from a text variable, specified as a byte array.
	  /// </summary>
	  /// <param name="content">  the text to create a <seealso cref="CharSource"/> for </param>
	  /// <returns>  a new instance of <seealso cref="CharSource"/> with UTF-8 for charset </returns>
	  public static CharSource ofContent(sbyte[] content)
	  {
		return CharSource.wrap(StringHelper.NewString(content, Charsets.UTF_8));
	  }

	  /// <summary>
	  /// Obtains an instance of <seealso cref="CharSource"/> from a text variable, specified as a byte array.
	  /// This also takes in a specific character set, as a <seealso cref="Charset"/>.
	  /// </summary>
	  /// <param name="content">  the text to create a <seealso cref="CharSource"/> for </param>
	  /// <param name="charset">  the charset to build the new CharSource based on </param>
	  /// <returns>  a new instance of <seealso cref="CharSource"/> </returns>
	  public static CharSource ofContent(sbyte[] content, Charset charset)
	  {
		return CharSource.wrap(StringHelper.NewString(content, charset));
	  }
	}

}