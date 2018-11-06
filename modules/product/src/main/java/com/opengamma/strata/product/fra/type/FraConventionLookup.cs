using System.Collections.Generic;
using System.Collections.Concurrent;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fra.type
{

	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using NamedLookup = com.opengamma.strata.collect.named.NamedLookup;

	/// <summary>
	/// Allows conventions to be created dynamically from an index name.
	/// </summary>
	internal sealed class FraConventionLookup : NamedLookup<FraConvention>
	{

	  /// <summary>
	  /// The singleton instance of the lookup.
	  /// </summary>
	  public static readonly FraConventionLookup INSTANCE = new FraConventionLookup();

	  /// <summary>
	  /// The cache by name.
	  /// </summary>
	  private static readonly ConcurrentMap<string, FraConvention> BY_NAME = new ConcurrentDictionary<string, FraConvention>();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private FraConventionLookup()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public override FraConvention lookup(string name)
	  {
		FraConvention value = BY_NAME.get(name);
		if (value == null)
		{
		  FraConvention created = createByName(name);
		  if (created != null)
		  {
			string correctName = created.Name;
			value = BY_NAME.computeIfAbsent(correctName, k => created);
			BY_NAME.putIfAbsent(correctName.ToUpper(Locale.ENGLISH), value);
		  }
		}
		return value;
	  }

	  public IDictionary<string, FraConvention> lookupAll()
	  {
		return BY_NAME;
	  }

	  private static FraConvention createByName(string name)
	  {
		return IborIndex.extendedEnum().find(name).map(index => ImmutableFraConvention.of(index)).orElse(null);
	  }

	}

}